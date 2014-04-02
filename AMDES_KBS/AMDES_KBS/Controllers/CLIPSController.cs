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
        public static bool? savePatient = false;

        //for debug purpose, to pull out to test on clips, and to pull out to assert for restore patient
        private static List<String> assertLog = new List<String>();
        static int count = 0; //just a counter to check the number of assertions... 

        //WARNING MOMOSOFT CLIPS REQUIRED x86 MODE ONLY, ALL OTHER MODE WILL FAIL
        private static Mommosoft.ExpertSystem.Environment env = new Mommosoft.ExpertSystem.Environment();

        private static string dataPath = @"Data\Logs\";
        private static string clpPath = @"engine\dementia.clp";

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
            foreach (String s in assertLog)
            {
                sb.AppendLine(s.ToString());
            }
            string filePath = dataPath + CurrentPatient.NRIC + ".log";
            File.WriteAllText(filePath, sb.ToString());
        }

        //@Allan, Not Tested yet
        public static History loadSavedAssertions()
        {
            CurrentPatient.SymptomsList.Clear();
            //call this f(x) everytime u click a new patient
            env.Clear();
            assertLog.Clear();
            count = 0;

            env.Load(clpPath);
            reset();

            if (File.Exists(dataPath) && HistoryController.isHistoryExist(pat.NRIC))
            {
                //File Ops here
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(dataPath);

                while ((line = file.ReadLine()) != null)
                {
                    assertLog.Add(line);
                    Console.WriteLine(line);
                    count++;
                }

                file.Close();
                //End File Op
                foreach (string s in assertLog)
                {
                    env.AssertString(s);
                }

                run();
                return HistoryController.getHistoryByID(pat.NRIC);
            }
            else
            {
                clearAndLoadNew();
                return null;
            }
        }

        public static void clearAndLoadNew()
        {
            if (CurrentPatient != null)
            {
                CurrentPatient.SymptomsList.Clear();
                //call this f(x) everytime u click a new patient
                env.Clear();
                assertLog.Clear();
                count = 0;

                env.Load(clpPath);

                reset();
                assert(new StringBuilder("(mode 1)"));
                assertAge();
                run();

                List<QuestionGroup> grps = QuestionController.getAllQuestionGroup();

                FirstQuestion fq = FirstQuestionController.readFirstQuestion();
                List<Rules> rList = NavigationController.getAllRules();
                List<Navigation> defBehavior = DefaultBehaviorController.getAllDefaultBehavior();

                if (grps.Count == 0)
                {
                }
                else if (fq == null)
                {
                }
                else if (rList.Count == 0)
                {
                }
                else
                {
                    loadQuestions(grps); //load question pass all assertions
                    run();
                    loadNavex(fq, rList, defBehavior);
                    run();
                    saveAssertLog();
                    //run();
                }

            }
            else
            {
                throw new NullReferenceException("Current Patient is Null!, please set CurrentPatient before loading.");
            }
        }

        private static void assertAge()
        {
            assert(new StringBuilder("(attribute AGE " + CurrentPatient.getAge() + ")"));
        }

        private static void run()
        {
            env.Run();
            //assertLog.Add("(run)");
        }

        public static void reset()
        {
            env.Reset();
            assertLog.Add("(reset)");
        }

        private static void assert(StringBuilder sb)
        {
            String a = sb.ToString().Trim();
            env.AssertString(a);
            assertLog.Add(a);
            count++;
        }

        //Clear
        private static void loadQuestions(List<QuestionGroup> grps)
        {
            StringBuilder sb;

            foreach (QuestionGroup qg in grps)
            {
                sb = new StringBuilder();
                sb.Append("(group (GroupID _" + qg.GroupID + ") (SuccessType ");

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
                if (qg.Symptom.Length > 0)
                {
                    sb.Append("(groupid-symptoms (GroupID _" + qg.GroupID + ") (symptom " + "\"" + qg.Symptom + "\"" + ") )");
                    assert(sb);
                    sb.Clear();
                }


                foreach (Question q in qg.Questions)
                {
                    sb.Clear();

                    sb.Append("(question (ID _" + q.ID + ") ");
                    sb.Append("(GroupID _" + qg.GroupID + ") ");

                    //sb.Append("(QuestionText " + "\"" + q.Name + "\"" + ") "); 
                    //irrelevant to dump to clips required only when doing on command prompt

                    sb.Append(")");
                    assert(sb);

                    //question symptom assertion
                    if (q.Symptom.Length > 0)
                    {
                        sb.Clear();
                        sb.Append("(questionid-symptoms (QuestionID _" + q.ID + ") (symptom " + "\"" + q.Symptom + "\"" + ") )");
                        assert(sb);
                    }
                }
            }

        }

        private static void loadNavex(FirstQuestion fq, List<Rules> rList, List<Navigation> defBehavior)
        {

            //1st navex point
            assert(new StringBuilder("(Navigation  (DestinationGroupID _" + fq.GrpID + ") (NavigationID _0) )"));

            foreach (Navigation n in defBehavior)
            {
                createNavigationAssertion(n);
            }
            List<Navigation> nList = new List<Navigation>();
            foreach (Rules r in rList)
            {
                foreach (Navigation n in r.Navigations)
                {
                    nList.Add(n);
                }
            }

            nList.Sort();

            foreach (Navigation n in nList)
            {
                createNavigationAssertion(n);
            }
        }

        private static void createNavigationAssertion(Navigation n)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("(Navigation " + "(NavigationID N" + n.NavID + ") ");


            if (n.DestGrpID != -1)
            {
                sb.Append("(DestinationGroupID _" + n.DestGrpID + ") ");
            }

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
                sb.Append("(NaviChildCritQ (NavigationID N" + n.NavID + ") ");
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
                sb.Append("(NaviChildCritA (NavigationID N" + n.NavID + ") ");
                sb.Append("(AttributeName " + ncq.AttributeName + ") ");
                sb.Append("(AttributeValue " + ncq.AttributeValue + ") ");
                sb.Append("(AttributeCompareType " + ncq.getCompareTypeString() + ") ");
                sb.Append(")");

                assert(sb);
                sb.Clear();
            }
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
            assert(new StringBuilder("(next)"));
            run();
        }

        public static void assertPrevSection()
        {
            assert(new StringBuilder("(previous)"));
            run();
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
            x = x.Remove(0, 1); //remove _

            if (x.CompareTo("RESULT") == 0) //
            {
                return -1; //when -1 call getResultingDiagnosis()
            }
            else
            {
                return int.Parse(x);
            }
        }

        public static List<Diagnosis> getResultingDiagnosis()
        {
            String evalStr = "(find-all-facts ((?f diagnosis)) TRUE)"; //" (find-all-facts((?a Currentgroup)) TRUE)";
            //String evalStr = "(find-all-facts ((?f NaviHistory)) TRUE)";
            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));
            List<Diagnosis> dList = new List<Diagnosis>();

            foreach (FactAddressValue fv in mv)
            {
                //multi field need to use array choices
                MultifieldValue ArrayChoices = (MultifieldValue)fv.GetFactSlot("RID");

                for (int i = 0; i < ArrayChoices.Count(); i++)
                {
                    string x = ArrayChoices[i].ToString().Remove(0, 1);
                    Diagnosis d = DiagnosisController.getDiagnosisByID(int.Parse(x));
                    CurrentPatient.addDiagnosis(d);
                    dList.Add(d);
                }

            }

            getCurrentPatientSymptom();

            CurrentPatient.setCompleted();

            PatientController.updatePatient(CurrentPatient);
            HistoryController.updatePatientNavigationHistory(getCurrentPatientHistory());
            return dList;
        }

        //TODO: Get back symptom
        //TODO: Get back QnHistory
        //TODO: Get back Navihistory
        //TODO: Get back Symptom.

        private static List<int> getNaviHistory()
        {
            List<int> naviHistory = new List<int>();
            String evalStr = "(find-all-facts((?a NaviHistory)) TRUE)";
            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));

            foreach (FactAddressValue fv in mv)
            {
                //multi field need to use array choices
                MultifieldValue ArrayChoices = (MultifieldValue)fv.GetFactSlot("ID");

                for (int i = 1; i < ArrayChoices.Count() - 1; i++)
                {
                    string x = ArrayChoices[i].ToString().Remove(0, 1);
                    if (x.CompareTo("RESULT") != 0) //
                    {
                        naviHistory.Add(int.Parse(x));
                    }
                }
            }

            return naviHistory;
        }

        public static List<Symptom> getCurrentPatientSymptom()
        {
            List<Symptom> sList = new List<Symptom>();

            String evalStr = "(find-all-facts((?g symptoms)) TRUE)";
            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));

            foreach (FactAddressValue fv in mv)
            {
                Symptom s = new Symptom(fv.GetFactSlot("symptom").ToString().Replace('"', ' '),
                                        fv.GetFactSlot("ID").ToString().Remove(0, 1));

                CurrentPatient.addSymptom(s);
            }

            return sList;
        }

        public static History getCurrentPatientHistory()
        {

            List<int> navHistory = getNaviHistory();
            History history = new History(CurrentPatient.NRIC);
            String evalStr = " (find-all-facts((?a question)) TRUE)";

            for (int i = 0; i < navHistory.Count; i++)
            {
                history.createNewHistory(navHistory[i]);
            }

            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));

            foreach (FactAddressValue fv in mv)
            {
                //question history YES ONLY
                int x = int.Parse(fv.GetFactSlot("GroupID").ToString().Remove(0, 1));
                //natalie :(
                if (navHistory.Contains(x))
                {
                    string qid = fv.GetFactSlot("ID").ToString();
                    string answer = fv.GetFactSlot("answer").ToString();
                    if (answer.ToUpper().CompareTo("YES") == 0)
                    {
                        //add history item
                        history.updateHistoryItem(x, qid.Remove(0, 1), true);
                    }
                    else
                    {
                        history.updateHistoryItem(x, qid.Remove(0, 1), false);
                    }
                }
            }
            return history;

        }

    }
}