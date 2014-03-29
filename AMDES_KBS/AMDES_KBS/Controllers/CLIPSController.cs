using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMDES_KBS.Entity;
using Mommosoft.ExpertSystem;
using System.IO;

namespace AMDES_KBS.Controllers
{
    class CLIPSController
    {
        private static Patient pat;

        //for debug purpose, to pull out to test on clips, and to pull out to assert for restore patient
        private static List<StringBuilder> assertLog = new List<StringBuilder>();

        //WARNING MOMOSOFT CLIPS REQUIRED x86 MODE ONLY, ALL OTHER MODE WILL FAIL
        private static Mommosoft.ExpertSystem.Environment env = new Mommosoft.ExpertSystem.Environment();

        private static string dataPath = @"Data\Logs\";

        public static Patient CurrentPatient
        {
            get
            {
                return CLIPSController.pat;
            }
            set
            {
                if (value != null)
                    CLIPSController.pat = value;
                else
                    throw new NullReferenceException("Current Patient is NULL!");
            }
        }

        public static void saveAssertLog()
        {
            StringBuilder sb = new StringBuilder();
            foreach (StringBuilder s in assertLog)
            {
                sb.AppendLine(s.ToString());
            }
            string filePath = dataPath + CurrentPatient.NRIC + ".log";
            File.WriteAllText(filePath, sb.ToString());
        }


        public static void ClearandLoad()
        {
            env.Clear();
            assertLog.Clear();

            env.Load("dementia.clp");
            reset();
            assert(new StringBuilder("(mode 1)"));

            List<QuestionGroup> grps = QuestionController.getAllQuestionGroup();
            FirstQuestion fq = FirstQuestionController.readFirstQuestion();
            List<Rules> rList = NavigationController.getAllRules();
            List<Navigation> defBehavior = DefaultBehaviorController.getAllDefaultBehavior();
            loadQuestions(grps);
            loadNavex(fq, rList, defBehavior);

            run();
        }

        public static void assertAge()
        {
            assert(new StringBuilder("(attribute Age " + CurrentPatient.getAge() + ")"));
        }

        private static void run()
        {
            env.Run();
        }

        public static void reset()
        {
            env.Reset();
        }

        private static void assert(StringBuilder sb)
        {
            env.AssertString(sb.ToString());
            assertLog.Add(sb);
        }

        public static void loadQuestions(List<QuestionGroup> grps)
        {
            //to paste to load questions
            ClearandLoad();
            reset();
            assertAge();


            foreach (QuestionGroup qg in grps)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("(group (GroupId _" + qg.GroupID + ") (SuccessType ");

                if (qg.getQuestionTypeENUM() == QuestionType.COUNT)
                {
                    QuestionCountGroup qcg = (QuestionCountGroup)qg;

                    sb.Append(qcg.getQuestionTypeENUM().ToString() + ") (SuccessArg " + qcg.Threshold + ")) ");
                }
                else
                {
                    sb.Append(qg.getQuestionTypeENUM().ToString() + ")) ");
                }

                assert(sb);
                sb.Clear();

                //grp symptom assertion
                sb.Append("(groupid-symtoms (GroupID _" + qg.GroupID + ") (symtom " + qg.Symptom + ") )");
                assert(sb);


                foreach (Question q in qg.Questions)
                {
                    sb.Clear();

                    sb.Append("(question (Id _" + q.ID + ") (QuestionText " + "\"" +
                        q.Name + "\"" + ") (GroupId _" + qg.GroupID + "))");
                    assert(sb);

                    sb.Clear();
                    //question symptom assertion
                    sb.Append("(questionid-symtoms (QuestionID _" + q.ID + ") (symtom " + q.Symptom + ") )");
                    assert(sb);
                }
            }

        }

        private static void loadNavex(FirstQuestion fq, List<Rules> rList, List<Navigation> defBehavior)
        {

            //1st navex point
            assert(new StringBuilder("(Navigation  (DestinationGroupID _" + fq.GrpID + ") (NavigationID _0) )"));

            //2nd navex point A
            assert(new StringBuilder("(Navigation  (DestinationGroupID _" + fq.NextGrpID + ") " +
                            "(NavigationID S1_" + fq.NextGrpID + ") )"));
            
            assert(new StringBuilder("(NaviChildCritQ (NavigationID S1_" + fq.NextGrpID + ")  " +
                            " (CriteriaGroupID _" + fq.GrpID + ") (CriteriaAnswer Yes) )"));

            //2nd navex point B
            assert(new StringBuilder("(Navigation  (DestinationGroupID _" + fq.NextGrpID + ")  " +
                             "(NavigationID S2_" + fq.NextGrpID + ") )"));

            assert(new StringBuilder("(NaviChildCritQ (NavigationID S2_" + fq.NextGrpID +
                            " (CriteriaGroupID _" + fq.GrpID + ") (CriteriaAnswer No) )"));


            foreach (Rules r in rList)
            {
                foreach (Navigation n in r.Navigations)
                {
                    createNavigationAssertion(n);
                }
            }

            foreach (Navigation n in defBehavior)
            {
                createNavigationAssertion(n);
            }
        }
        private static void createNavigationAssertion(Navigation n)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(Navigation  (DestinationGroupID _");
            sb.Append(n.DestGrpID + ")");
            sb.Append(" (NavigationID _" + n.NavID + ") ");

            if (n.isConclusive())
            {
                sb.Append("(RID");
                foreach (int x in n.DiagnosesID)
                {
                    sb.Append(" R" + x.ToString());
                }
                sb.Append(") ");
            }

            sb.Append(")");
            assert(sb);

            sb.Clear();
            foreach (NaviChildCriteriaQuestion ncq in n.ChildCriteriaQuestion)
            {
                sb.Append("(NaviChildCritQ (NavigationID _" + n.NavID + ") ");
                sb.Append("(CriteriaGroupID _" + ncq.CriteriaGrpID + ") ");
                if (ncq.Ans == false)
                {
                    sb.Append("(CriteriaAnswer No)");
                }
                else
                {
                    sb.Append("(CriteriaAnswer Yes)");
                }
                sb.Append(")");

                assert(sb);
                sb.Clear();
            }

            foreach (NaviChildCritAttribute ncq in n.ChildCriteriaAttributes)
            {
                //(NaviChildCritA (NavigationID GO_C) (AttributeName Age) (AttributeValue 50) (AttributeCompareType <) )
                sb.Append("(NaviChildCritA (NavigationID _" + n.NavID + ") ");
                sb.Append("(AttributeName " + ncq.AttributeName + ") ");
                sb.Append("(AttributeValue " + ncq.AttributeValue + ") ");
                sb.Append("(AttributeCompareType " + ncq.getCompareTypeString() + ") ");
                sb.Append(")");

                assert(sb);
                sb.Clear();
            }
        }

        public static int getCurrentQnGroupID()
        {
            //WARNING: Require Navigation to be SETUP OR INSTANT FAIL!

            String evalStr = "(find-all-facts ((?f Currentgroup)) TRUE)"; //" (find-all-facts((?a Currentgroup)) TRUE)";
            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));

            string x = "-1";
            foreach (FactAddressValue fv in mv)
            {
                x = fv.GetFactSlot("GroupID").ToString();
            }

            return int.Parse(x.Remove(0, 1)); //remove _
        }

        //TODO: Get back symptom
        //TODO: Get back QnHistory
        //TODO: Get back Navihistory
        //TODO: Get back Symptom.

        private static List<int> getNaviHistory()
        {
            string x = "";

            List<int> naviHistory = new List<int>();
            String evalStr = " (find-all-facts((?a NaviHistory)) TRUE)";
            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));

            foreach (FactAddressValue fv in mv)
            {
                x = fv.GetFactSlot("ID").ToString();
            }

            Console.WriteLine(x);

            return naviHistory;
        }

        public static List<Symptom> getCurrentPatientSymptom()
        {
            List<Symptom> sList = new List<Symptom>();

            String evalStr = " (find-all-facts((?a question)) TRUE)";

            return sList;
        }

        public static History getCurrentPatientHistory()
        {

            List<int> navHistory = new List<int>();
            History history = new History(CurrentPatient.NRIC);
            String evalStr = " (find-all-facts((?a question)) TRUE)";

            for (int i = 0; i < navHistory.Count; i++)
            {
                history.createNewHistory(navHistory[i]);
            }

            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));
            string x = "";

            foreach (FactAddressValue fv in mv)
            {
                //question history YES ONLY
                x = fv.GetFactSlot("GroupID").ToString();
                //natalie :(


                if (navHistory.Contains(int.Parse(x.Remove(0, 1))))
                {
                    string qid = fv.GetFactSlot("Id").ToString();
                    string answer = fv.GetFactSlot("answer").ToString();
                    if (answer.ToUpper().CompareTo("YES") == 0)
                    {
                        //add history item
                        history.updateHistoryItem(int.Parse(x), int.Parse(qid.Remove(0, 1)), true);
                    }
                    else
                    {
                        history.updateHistoryItem(int.Parse(x), int.Parse(qid.Remove(0, 1)), false);
                    }
                }

            }

            return history;
        }

        public static void assertQuestion(string id, bool answer)
        {
            //to paste to load questions
            StringBuilder sb = new StringBuilder("(choice _" + id + " ");
            if (answer)
            {
                sb.Append("Yes");
            }
            else
            {
                sb.Append("No");
            }
            sb.Append(")");

            assert(sb);
            run();
        }

        public static void assertNextSection()
        {
            assert(new StringBuilder("(Next)"));
            run();
        }

        public static void assertPrevSection()
        {
            assert(new StringBuilder("(Previous)"));
            run();
        }

    }
}