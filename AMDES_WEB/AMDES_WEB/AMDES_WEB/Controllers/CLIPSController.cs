using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMDES_KBS.Entity;
using Mommosoft.ExpertSystem;


/* Application Scope: The variables that have application scope are available throughout the application,
 * i.e to all users of the applications across all pages.

 * Session Scope: When many users connect to your site, each of them will have a separate session 
 * (tied to the identity of the user that is recognized by the application.) 
 * When the variable has session scope it will have new instance for each session, even though the users are accessing the same page. 
 * The session variable instance is available across all pages for that session.

 * Page Scope: When you have a instance variable on a Page it is specific to that page only and that session only.

 * Static variables have Application scope. All users of the application will share the same variable instance in your case.

 * For windows application can use static, because it is only 1 user as it is a standalone application, however when converted to web, 
 * static variables cannot be used anymore as it is application scope.
 */

namespace AMDES_KBS.Controllers
{
    public class CLIPSController
    {
        public static bool ExpertUser;
        public static bool enableSavePatient;
        public static bool enablePrev;
        public static bool enableStats;


        public static bool? savePatient = false;
        private List<History> hyList = new List<History>();

        public static bool? readOnly = false;
        //warning only for viewing the data will only be saved for latest test for assertions 
        //all other tests are readonly, do not call clips controller!

        //for debug purpose, to pull out to test on clips, and to pull out to assert for restore patient
        private List<string> assertLog = new List<string>();

        static int count = 0; //just a counter to check the number of assertions... 

        private Patient pat;
        //WARNING MOMOSOFT CLIPS REQUIRED x86 MODE ONLY, ALL OTHER MODE WILL FAIL
        private Mommosoft.ExpertSystem.Environment env = new Mommosoft.ExpertSystem.Environment();

        private string logPath;
        public static string clpPath = System.Web.HttpContext.Current.Server.MapPath(@"engine\dementia.clp");

        private string appName;

        public string ApplicationName
        {
            get
            {
                return appName;
            }
            set
            {
                this.appName = value;
                logPath = System.Web.HttpContext.Current.Server.MapPath(@"Data\" + appName + @"\Logs\");
            }
        }

        public Patient CurrentPatient
        {
            get
            {
                return pat;
            }
            set
            {
                if (value != null)
                    pat = value;
                else
                    pat = new Patient();
            }
        }

        public void saveAssertLog()
        {
            StringBuilder sb = new StringBuilder();
            foreach (String s in assertLog)
            {
                sb.AppendLine(s);
            }
            string filePath = logPath + pat.NRIC + ".log";
            File.WriteAllText(filePath, sb.ToString());
        }

        public History loadSavedAssertions()
        {
            //pat.SymptomsList.Clear();
            //call this f(x) everytime u click a new patient
            env.Clear();
            assertLog.Clear();
            count = 0;

            env.Load(clpPath);
            reset();

            if (File.Exists(logPath) && HistoryController.isHistoryExist(pat.NRIC))
            {
                //File Ops here
                string line;
                StreamReader file = new System.IO.StreamReader(logPath);

                while ((line = file.ReadLine()) != null)
                {
                    assert(new StringBuilder(line));
                    count++;
                }

                file.Close();
                saveAssertLog();
                //End File Op
                run();
                return pat.getLatestHistory();
            }
            else
            {
                clearAndLoadNew();
                return null;
            }
        }

        public void clearAndLoadNew()
        {
            if (pat != null)
            {
                //pat.SymptomsList.Clear();
                //call this f(x) everytime u click a new patient
                env.Clear();
                assertLog.Clear();
                count = 0;

                env.Load(clpPath);
                reset();
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
                    throw new InvalidOperationException("First question is undefined!");
                }
                else
                {
                    loadQuestions(grps);

                    loadDiagnosis();

                    loadNavex(fq, rList, defBehavior);
                    saveAssertLog();
                    assertAllAttributes();
                    run();
                }

            }
            else
            {
                throw new NullReferenceException("Current Patient is Null!, please set pat before loading.");
            }
        }



        private void loadDiagnosis()
        {
            List<Diagnosis> diagList = DiagnosisController.getAllDiagnosis();
            StringBuilder sb;

            foreach (Diagnosis d in diagList)
            {
                sb = new StringBuilder();
                //(candidate-diagnosis (RID R9) (Header "Patient OK") (Comment "Patient has no cognitive deficit or dementia"))
                sb.Append("(candidate-diagnosis (RID R" + d.RID.ToString() + ") ");
                sb.Append("(Header " + "\"" + d.Header.Trim().Replace("~~", " ") + "\"" + ") ");
                sb.Append("(Comment " + "\"" + d.Comment.Trim().Replace("~~", " ") + "\"" + ") ");

                if (d.Link.Length > 0)
                    sb.Append("(Link " + "\"" + d.Link + "\"" + ")");

                sb.Append(") ");
                assert(sb);
            }

        }

        private void assertAllAttributes()
        {
            assert(new StringBuilder("(attribute AGE " + pat.getAge() + ")"), false);

            foreach (KeyValuePair<string, double> kvp in pat.getAttributes())
            {
                assert(new StringBuilder("(attribute " + kvp.Key.ToUpper().Replace(" ", "_") + " " + kvp.Value.ToString() + ")"), false);
            }
        }

        private void run()
        {
            env.Run();
            //assertLog.Add("(run)");
        }

        public void reset()
        {
            env.Reset();
            assertLog.Add("(reset)");
        }

        private void assert(StringBuilder sb, bool init = true)
        {
            String a = sb.ToString().Trim();
            env.AssertString(a);
            assertLog.Add(a);


            if (!init)
                saveAssertLog();

            count++;

        }

        //Clear
        //http://msdn.microsoft.com/en-us/library/dd460713(v=vs.100).aspx
        private void loadQuestions(List<QuestionGroup> grps)
        {
            StringBuilder sb;

            foreach (QuestionGroup qg in grps)
            //Parallel.ForEach(grps, qg =>
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

                    sb.Append("(QuestionText " + "\"" + q.Name + "\"" + ") ");
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

            }//);

        }

        private void loadNavex(FirstQuestion fq, List<Rules> rList, List<Navigation> defBehavior)
        {
            //1st navex point
            assert(new StringBuilder("(Navigation  (DestinationGroupID _" + fq.GrpID + ") (NavigationID _0) )"));

            foreach (Navigation n in defBehavior)
            {
                createNavigationAssertion(n);
            }

            List<Navigation> nList = new List<Navigation>();
            int i = 0;
            foreach (Rules r in rList)
            {
                foreach (Navigation n in r.Navigations)
                {
                    if (n != null && !nList.Contains(n))
                        nList.Add(n);
                }

            }
            Navigation.CriteriaSortingComparer comparer = new Navigation.CriteriaSortingComparer();
            nList.Sort(comparer);


            foreach (Navigation n in nList)
            {
                createNavigationAssertion(n);
            }
        }



        private void createNavigationAssertion(Navigation n)
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
                sb = new StringBuilder();
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

            foreach (NaviChildCritAttribute nca in n.ChildCriteriaAttributes)
            {
                sb = new StringBuilder();
                //(NaviChildCritA (NavigationID GO_C) (AttributeName Age) (AttributeValue 50) (AttributeCompareType <) )
                sb.Append("(NaviChildCritAttribute (NavigationID N" + n.NavID + ") ");
                sb.Append("(AttributeName " + nca.AttributeName.ToUpper().Replace(" ", "_") + ") ");
                sb.Append("(AttributeValue " + nca.AttributeValue + ") ");
                sb.Append("(AttributeCompareType " + NaviChildCritAttribute.getCompareTypeString(nca.getAttributeTypeENUM()) + ") ");
                sb.Append(")");

                assert(sb);

                sb.Clear();
            }
        }

        public void assertQuestion(int grpID, int id, bool answer)
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

            assert(sb, false);
            run();
        }

        public void assertNextSection()
        {
            assert(new StringBuilder("(next)"), false);
            run();
        }

        public void assertPrevSection()
        {
            assert(new StringBuilder("(previous)"), false);
            run();
        }

        public void retractDiagnosis()
        {
            assertPrevSection();
        }

        public int getCurrentQnGroupID()
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

        public void getResultingDiagnosis()
        {
            History h = getpatHistory();

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

                        foreach (CmpAttribute kvp in d.getAttributes())
                        {
                            if (kvp.Key.ToUpper().CompareTo("AGE") == 0)
                            {
                                bool match = false;
                                switch (kvp.CompareType)
                                {
                                    case AttributeCmpType.Equal:
                                        if (pat.getAge() == kvp.Value)
                                            match = true;
                                        break;
                                    case AttributeCmpType.LessThan:
                                        if (pat.getAge() < kvp.Value)
                                            match = true;
                                        break;

                                    case AttributeCmpType.LessThanEqual:
                                        if (pat.getAge() <= kvp.Value)
                                            match = true;
                                        break;

                                    case AttributeCmpType.MoreThan:
                                        if (pat.getAge() > kvp.Value)
                                            match = true;
                                        break;

                                    case AttributeCmpType.MoreThanEqual:
                                        if (pat.getAge() >= kvp.Value)
                                            match = true;
                                        break;

                                }
                                if (match == true)
                                {
                                    if (d.Comment.Trim().Length > 0)
                                    {
                                        d.Comment += "~~";
                                    }
                                    d.Comment += "&emsp;&emsp;" + App.bulletForm() + " Age "
                                        + NaviChildCritAttribute.getCompareTypeString(kvp.CompareType) + " " + kvp.Value.ToString();
                                }

                            }
                            else
                            {
                                //Get the key value from pat.
                                PatAttribute pa = PatAttributeController.searchPatientAttribute(kvp.Key.ToUpper());
                                CmpAttribute ca = kvp;

                                if (pa.AttrType == PatAttribute.AttributeType.CATEGORICAL)
                                {
                                    if (pat.retrieveAttribute(pa.AttributeName) == ca.Value)
                                    {
                                        if (d.Comment.Trim().Length > 0)
                                        {
                                            d.Comment += d.Comment += "~~";
                                        }
                                        d.Comment += "   " + App.bulletForm() + " "
                                            + kvp.Key + " is of " + pa.getCategoryByID(ca.Value);
                                    }
                                }
                                else if (pa.AttrType == PatAttribute.AttributeType.NUMERIC)
                                {
                                    bool match = false;
                                    switch (ca.CompareType)
                                    {
                                        case AttributeCmpType.Equal:
                                            if (pat.retrieveAttribute(pa.AttributeName) == ca.Value)
                                                match = true;
                                            break;

                                        case AttributeCmpType.LessThan:
                                            if (pat.retrieveAttribute(pa.AttributeName) < ca.Value)
                                                match = true;
                                            break;

                                        case AttributeCmpType.LessThanEqual:
                                            if (pat.retrieveAttribute(pa.AttributeName) <= ca.Value)
                                                match = true;
                                            break;

                                        case AttributeCmpType.MoreThan:
                                            if (pat.retrieveAttribute(pa.AttributeName) > ca.Value)
                                                match = true;
                                            break;

                                        case AttributeCmpType.MoreThanEqual:
                                            if (pat.retrieveAttribute(pa.AttributeName) >= ca.Value)
                                                match = true;
                                            break;
                                    }
                                    if (match == true)
                                    {
                                        if (d.Comment.Trim().Length > 0)
                                        {
                                            d.Comment += "~~";
                                        }
                                        d.Comment += "&emsp;&emsp;" + App.bulletForm() + " " + kvp.Key + " "
                                            + NaviChildCritAttribute.getCompareTypeString(ca.CompareType) + " " + ca.Value.ToString();
                                    }
                                }
                            }
                        }


                        if (d.RetrieveSym && d.RetrievalIDList.Count > 0)
                        {
                            foreach (int qgID in d.RetrievalIDList)
                            {
                                List<Symptom> grpSymptoms = getPatientSymptomByQG(qgID);
                                for (int j = 0; j < grpSymptoms.Count; j++)
                                {
                                    Symptom s = grpSymptoms[j];
                                    if (d.Comment.Trim().Length == 0)
                                    {
                                        d.Comment += "&emsp;&emsp;" + App.bulletForm() + " " + s.SymptomName.Trim();
                                    }
                                    else
                                    {
                                        d.Comment += "~~&emsp;&emsp;" + App.bulletForm() + " " + s.SymptomName.Trim();
                                    }
                                }
                            }

                        }
                        d.Comment = d.Comment.Trim();
                        h.addDiagnosis(d);
                    }
                }

            }

            h = getpatSymptom(h);
            h.setCompleted();

            HistoryController.updatePatientNavigationHistory(h, pat.AssessmentDate.Date);
        }

        public void saveCurrentNavex()
        {
            History h = getpatHistory();

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
                        if (d.RetrieveSym && d.RetrievalIDList.Count > 0)
                        {
                            foreach (int k in d.RetrievalIDList)
                            {
                                List<Symptom> grpSymptoms = getPatientSymptomByQG(k);
                                foreach (Symptom s in grpSymptoms)
                                {
                                    if (d.Comment.Trim().Length == 0)
                                    {
                                        d.Comment += s.SymptomName.Trim();
                                    }
                                    else
                                    {
                                        d.Comment += ", " + s.SymptomName.Trim();
                                    }

                                    d.Comment = d.Comment.Trim();
                                }
                            }
                        }
                        d.Comment = d.Comment.Trim();
                        h.addDiagnosis(d);
                    }
                }

            }

            h = getpatSymptom(h);
            if (savePatient.Value)
                HistoryController.updatePatientNavigationHistory(h, pat.AssessmentDate.Date);

            else
                HistoryController.updatePatientNavigationHistory(h, new DateTime(0));

        }

        private List<int> getNaviHistory()
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

        private List<Symptom> getPatientSymptomByQG(int qgID)
        {
            List<Symptom> sList = new List<Symptom>();

            String evalStr = "(find-all-facts((?s symptoms)) TRUE)";
            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));

            foreach (FactAddressValue fv in mv)
            {
                string grpID = fv.GetFactSlot("GroupID").ToString().Remove(0, 1);
                string qID = fv.GetFactSlot("QuestionID").ToString().Remove(0, 1);
                int y;
                bool result = int.TryParse(grpID, out y);

                if (result && qgID == y)
                {
                    Symptom s = new Symptom(fv.GetFactSlot("symptom").ToString().Replace('"', ' '), grpID.ToString());

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
                                int succArg = int.Parse(fav.GetFactSlot("SuccessArg").ToString());
                                int trueCount = int.Parse(fav.GetFactSlot("TrueCount").ToString());
                                s.SymptomName += "- Patient's Score: " + trueCount + ", Normal Score: " + succArg + " or more";
                                break;
                            }
                        }
                    }

                    sList.Add(s);
                }
            }
            return sList;
        }

        private History getpatSymptom(History h)
        {
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
                                int succArg = int.Parse(fav.GetFactSlot("SuccessArg").ToString());
                                int trueCount = int.Parse(fav.GetFactSlot("TrueCount").ToString());
                                s.SymptomName += "- Patient's Score: " + trueCount + ", Normal Score: " + succArg + " or more";
                                break;
                            }
                        }
                    }
                }

                h.addSymptom(s);
            }

            return h;
        }

        private History getpatHistory()
        {
            History history;
            List<int> navHistory = getNaviHistory();
            if (savePatient.Value)
            {
                history = new History(pat.NRIC, pat.AssessmentDate);
            }
            else
            {
                history = new History(pat.NRIC, new DateTime(0));
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