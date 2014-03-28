using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMDES_KBS.Entity;

using Mommosoft.ExpertSystem;

namespace AMDES_KBS.Controllers
{
    class CLIPSController
    {
        private static Patient pat;

        public static Patient CurrentPatient
        {
            get { return CLIPSController.pat; }
            set { CLIPSController.pat = value; }
        }

        private static Mommosoft.ExpertSystem.Environment env = new Mommosoft.ExpertSystem.Environment();

        public static void ClearandLoad()
        {
            env.Clear();
            env.Load("dementia.clp");
            reset();
            env.AssertString("(mode 1)");

        }

        public static void reset()
        {
            env.Reset();
        }

        public static void loadEverything()
        {
            //to paste to load questions
            ClearandLoad();
            reset();
            assertAge();

            string str2assert;
            List<QuestionGroup> grps = QuestionController.getAllQuestionGroup();
            foreach (QuestionGroup qg in grps)
            {
                str2assert = "(group (GroupId _" + qg.GroupID +
                        ") (SuccessType ";
                if (qg.getQuestionTypeENUM() == QuestionType.COUNT)
                {
                    QuestionCountGroup qcg = (QuestionCountGroup)qg;
                    str2assert = str2assert + qcg.getQuestionTypeENUM().ToString() +
                        ") (SuccessArg " + qcg.Threshold + "))";

                }
                else
                {
                    str2assert = str2assert + qg.getQuestionTypeENUM().ToString() + "))";
                }

                env.AssertString(str2assert);

                //grp symptom assertion
                str2assert = "(groupid-symtoms (GroupID _" + qg.GroupID + ") (symtom " + qg.Symptom + ") )";
                env.AssertString(str2assert);

                foreach (Question q in qg.Questions)
                {
                    str2assert = "(question (Id _" + q.ID + ") (QuestionText " + "\"" +
                        q.Name + "\"" + ") (GroupId _" + qg.GroupID + "))";
                    env.AssertString(str2assert);

                    //question symptom assertion
                    str2assert = "(questionid-symtoms (QuestionID _" + q.ID + ") (symtom " + q.Symptom + ") )";
                    env.AssertString(str2assert);
                }
            }


            loadNavex();
            run();
        }

        private static void loadNavex()
        {
            FirstQuestion fq = FirstQuestionController.readFirstQuestion();

            env.AssertString("(Navigation  (DestinationGroupID _" + fq.GrpID + ") (NavigationID starting) )");

            env.AssertString("(Navigation  (DestinationGroupID _" + fq.NextGrpID + ") " +
                             "(NavigationID S1_" + fq.NextGrpID + ") )");

            env.AssertString("(NaviChildCritQ (NavigationID S1_" + fq.NextGrpID + ")  " +
                             " (CriteriaGroupID _" + fq.GrpID + ") (CriteriaAnswer Yes) )");


            env.AssertString("(Navigation  (DestinationGroupID _" + fq.NextGrpID + ")  " +
                             "(NavigationID S2_" + fq.NextGrpID + ") )");

            env.AssertString("(NaviChildCritQ (NavigationID S2_" + fq.NextGrpID +
                             " (CriteriaGroupID _" + fq.GrpID + ") (CriteriaAnswer No) )");


            List<Rules> navList = NavigationController.getAllRules();

            foreach (Rules r in navList)
            {
                StringBuilder sb = new StringBuilder();

                //TODO:
                foreach (Navigation n in r.Navigations)
                {
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
                    env.AssertString(sb.ToString());

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

                        env.AssertString(sb.ToString());
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

                        env.AssertString(sb.ToString());
                        sb.Clear();
                    }

                }

            }
        }
        private static string createNavigationAssertion(Navigation nav)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(Navigation ");

            //TODO

            sb.Append(")");

            return sb.ToString();
        }

        public static int getCurrentQnGroupID()
        {
            //http://stackoverflow.com/questions/9862085/finding-facts-of-a-template-which-has-something-in-common-with-another-template
            String evalStr = " (find-all-facts((?a Currentgroup)) TRUE)";
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
        //

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

            //if some jisiao go and add / delete question , archive and reset system. 
            return history;
        }



        public static void assertAge()
        {
            env.AssertString("(attribute Age " + CurrentPatient.getAge() + ")");//todo: assert which var
        }

        public static void assertQuestion(string id, bool answer)
        {
            //to paste to load questions
            string str2assert = "(choice _" + id + " ";
            if (answer)
            {
                str2assert += "Yes";
            }
            else
            {
                str2assert += "No";
            }
            str2assert += ")";

            env.AssertString(str2assert);
            run();
        }

        public static void run()
        {
            env.Run();
        }


        public static void assertNextSection()
        {
            env.AssertString("(Next)");
            run();
        }

        public static void assertPrevSection()
        {
            env.AssertString("(Previous)");
            run();
        }

    }
}
