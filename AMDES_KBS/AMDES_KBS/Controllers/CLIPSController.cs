using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AMDES_KBS.Entity;
using Mommosoft.ExpertSystem;

namespace AMDES_KBS.Controllers
{
    class CLIPSController
    {
        private static Patient pat;
        public static bool? savePatient = false;
        private List<History> hyList = new List<History>();

        public static bool? readOnly = false;
        //warning only for viewing the data will only be saved for latest test for assertions 
        //all other tests are readonly, do not call clips controller!

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

        public static History loadSavedAssertions()
        {
            //CurrentPatient.SymptomsList.Clear();
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
                return CurrentPatient.getLatestHistory();
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
                //CurrentPatient.SymptomsList.Clear();
                //call this f(x) everytime u click a new patient
                env.Clear();
                assertLog.Clear();
                count = 0;

                env.Load(clpPath);

                reset();
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
            saveAssertLog();
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

                    sb.Append("(question (ID _" + qg.GroupID + "." + q.ID + ") ");
                    sb.Append("(GroupID _" + qg.GroupID + ") ");

                    //sb.Append("(QuestionText " + "\"" + q.Name + "\"" + ") "); 
                    //irrelevant to dump to clips required only when doing on command prompt

                    sb.Append(")");
                    assert(sb);

                    //question symptom assertion
                    if (q.Symptom.Length > 0)
                    {
                        sb.Clear();
                        sb.Append("(questionid-symptoms (GroupID _" + qg.GroupID + ") (QuestionID _" + qg.GroupID + "." + q.ID + ") (symptom " + "\"" + q.Symptom + "\"" + ") )");
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
                sb.Append("(NaviChildCritQuestion (NavigationID N" + n.NavID + ") ");
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
                sb.Append("(NaviChildCritAttribute (NavigationID N" + n.NavID + ") ");
                sb.Append("(AttributeName " + ncq.AttributeName + ") ");
                sb.Append("(AttributeValue " + ncq.AttributeValue + ") ");
                sb.Append("(AttributeCompareType " + ncq.getCompareTypeString() + ") ");
                sb.Append(")");

                assert(sb);
                sb.Clear();
            }
        }

        public static void assertQuestion(int grpID, int id, bool answer)
        {
            //to paste to load questions
            StringBuilder sb = new StringBuilder("(choice _" + grpID + "." + id + " ");
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

        public static void retractDiagnosis()
        {
            //CurrentPatient.Diagnoses.Clear();
            //CurrentPatient.SymptomsList.Clear();
            assertPrevSection();
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

        public static void getResultingDiagnosis()
        {
            History h = getCurrentPatientHistory();

            String evalStr = "(find-all-facts ((?f diagnosis)) TRUE)"; //" (find-all-facts((?a Currentgroup)) TRUE)";
            //String evalStr = "(find-all-facts ((?f NaviHistory)) TRUE)";
            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));


            foreach (FactAddressValue fv in mv)
            {
                //multi field need to use array choices
                MultifieldValue ArrayChoices = (MultifieldValue)fv.GetFactSlot("RID");

                for (int i = 0; i < ArrayChoices.Count(); i++)
                {
                    string x = ArrayChoices[i].ToString().Remove(0, 1);
                    Diagnosis d = DiagnosisController.getDiagnosisByID(int.Parse(x));
                    h.addDiagnosis(d);
                }

            }

            h = getCurrentPatientSymptom(h);
            h.setCompleted();


            HistoryController.updatePatientNavigationHistory(h, CurrentPatient.AssessmentDate.Date);

        }

        public static void saveCurrentNavex()
        {
            History h = getCurrentPatientHistory();

            String evalStr = "(find-all-facts ((?f diagnosis)) TRUE)"; //" (find-all-facts((?a Currentgroup)) TRUE)";
            //String evalStr = "(find-all-facts ((?f NaviHistory)) TRUE)";
            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));


            foreach (FactAddressValue fv in mv)
            {
                //multi field need to use array choices
                MultifieldValue ArrayChoices = (MultifieldValue)fv.GetFactSlot("RID");

                for (int i = 0; i < ArrayChoices.Count(); i++)
                {

                    string x = ArrayChoices[i].ToString().Remove(0, 1);
                    int diagID = int.Parse(x);
                    if (diagID == -99)
                    {
                        Diagnosis d = new Diagnosis(diagID, fv.GetFactSlot("Comment").ToString(), "Rule was unhandled");
                        h.addDiagnosis(d);
                        break;
                    }
                    else
                    {
                        Diagnosis d = DiagnosisController.getDiagnosisByID(diagID);
                        h.addDiagnosis(d);
                    }
                }

            }

            h = getCurrentPatientSymptom(h);
            if (savePatient.Value)
            {
                HistoryController.updatePatientNavigationHistory(h, CurrentPatient.AssessmentDate.Date);
            }
            else
            {
                HistoryController.updatePatientNavigationHistory(h, new DateTime(0));
            }
        }

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

        private static History getCurrentPatientSymptom(History h)
        {
            //List<Symptom> sList = new List<Symptom>();

            String evalStr = "(find-all-facts((?s symptoms)) TRUE)";
            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));

            foreach (FactAddressValue fv in mv)
            {
                string grpID = fv.GetFactSlot("GroupID").ToString().Remove(0, 1);
                string qID = fv.GetFactSlot("QuestionID").ToString().Remove(0, 1);
                int y;
                bool result = int.TryParse(grpID, out y);


                Symptom s = new Symptom(fv.GetFactSlot("symptom").ToString().Replace('"', ' '),
                                        grpID.ToString());
                if (result)
                {
                    QuestionGroup qg = QuestionController.getGroupByID(y);
                    if (qg.getQuestionTypeENUM() == QuestionType.COUNT)
                    {
                        evalStr = "(find-all-facts((?g group)) TRUE)";
                        MultifieldValue mv1 = ((MultifieldValue)env.Eval(evalStr));
                        foreach (FactAddressValue fav in mv1)
                        {
                            int id = int.Parse(fav.GetFactSlot("GroupID").ToString().Remove(0, 1));
                            if (id == y)
                            {
                                int maxQn = int.Parse(fav.GetFactSlot("SuccessArg").ToString());
                                int trueCount = int.Parse(fav.GetFactSlot("TrueCount").ToString());
                                s.SymptomName += " - Required Score: " + maxQn + ", Patient Score: " + trueCount;
                                break;
                            }
                        }
                    }
                }

                h.addSymptom(s);
            }

            return h;
        }

        private static History getCurrentPatientHistory()
        {
            History history;
            List<int> navHistory = getNaviHistory();
            if (savePatient.Value)
            {
                history = new History(CurrentPatient.NRIC, CurrentPatient.AssessmentDate);
            }
            else
            {
                history = new History(CurrentPatient.NRIC, new DateTime(0));
            }

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
                    string qid = fv.GetFactSlot("ID").ToString().Remove(0, 1);
                    string answer = fv.GetFactSlot("answer").ToString();
                    if (answer.ToUpper().CompareTo("YES") == 0)
                    {
                        //add history item
                        history.updateHistoryItem(x, int.Parse(qid.Split('.')[1]), true);
                    }
                    else
                    {
                        history.updateHistoryItem(x, int.Parse(qid.Split('.')[1]), false);
                    }
                }
            }
            return history;

        }

    }
}