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

        public static void loadQuestions()
        {
            //to paste to load questions
            ClearandLoad();
            reset();

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

                foreach (Question q in qg.Questions)
                {
                    str2assert = "(question (Id _" + q.ID + ") (QuestionText " + "\"" +
                        q.Name + "\"" + ") (GroupId _" + qg.GroupID + "))";
                    env.AssertString(str2assert);
                }

                if (qg.NextTrueLink != null)
                {
                    string x = createNavigationAssertion(qg.NextTrueLink, qg.GroupID, true);
                    env.AssertString(x);
                }

                if (qg.NextFalseLink != null)
                {
                    env.AssertString(createNavigationAssertion(qg.NextFalseLink, qg.GroupID, false));
                }
            }
            assertAge();

            
            env.AssertString("(Navigation  (DestinationGroupID _1))");

            run();
        }

        //(Navigation (CriteriaGroupID A) (CriteriaAnswer Yes) (DestinationGroupID B))
        //(Navigation (CriteriaGroupID A) (CriteriaAnswer No) (DestinationGroupID C) (AttributeName Age) (AttributeValue 65) (AttributeCompareType >=))
        //(Navigation (CriteriaGroupID A) (CriteriaAnswer No) (DestinationGroupID B) (AttributeName Age) (AttributeValue 65) (AttributeCompareType <))
        //(Navigation (CriteriaGroupID B) (CriteriaAnswer No) (Comment "UR ILL") (Conclusion Yes))
        //(Navigation (CriteriaGroupID C) (CriteriaAnswer No) (Comment "UR ILL") (Conclusion Yes))

        private static string createNavigationAssertion(Navigation nav, int groupID, bool criteria)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(Navigation ");
            sb.Append("(CriteriaGroupID _" + groupID + ") ");
            if (criteria)
            {
                sb.Append("(CriteriaAnswer Yes) ");
            }
            else
            {
                sb.Append("(CriteriaAnswer No) ");
            }

            if (!nav.isConclusive)
            {
                sb.Append("(DestinationGroupID _" + nav.DestGrpID + ") ");

                if (nav.isRequireAge)
                {
                    sb.Append("(AttributeName Age) ");
                    sb.Append("(AttributeValue " + nav.Age.ToString() + ") ");

                    if (nav.MoreThanEqualAge)
                    {
                        sb.Append("(AttributeCompareType >=)");
                    }
                    else if (nav.LessThanAge)
                    {
                        sb.Append("(AttributeCompareType <))");
                    }
                }
            }
            else
            {
                sb.Append("(Conclusion Yes)");
                for (int i = 0; i < nav.getDiagnosis().Count(); i++)
                {
                    Diagnosis d = nav.getDiagnosisAt(i);
                    //TODO Assert Diagnosis result in multifield
                }

            }

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

        public static List<Question> getHistory()
        {
            List<Question> history = new List<Question>();
            String evalStr = " (find-all-facts((?a question)) TRUE)";

            MultifieldValue mv = ((MultifieldValue)env.Eval(evalStr));

            foreach (FactAddressValue fv in mv)
            {
                //question history YES ONLY
                //
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
