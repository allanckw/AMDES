using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMDES_KBS.Entity;


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
            
        }

        public static void reset()
        {
            env.AssertString("(reset)");
        }

        public static void loadQuestions(List<QuestionGroup> qg)
        {
            //to paste to load questions
            string str2assert;

            foreach (QuestionGroup qG in qg)
            {
                str2assert = "(group (GroupId _" + qG.GroupID +
                        ") (SuccessType ";
                if (qG.getQuestionTypeENUM() == QuestionType.COUNT)
                {
                    QuestionCountGroup qcg = (QuestionCountGroup)qG;
                    str2assert = str2assert + qcg.getQuestionTypeENUM().ToString() +
                        ") (SuccessArg " + qcg.Threshold + "))";
                }
                else
                {
                    str2assert = str2assert + qG.getQuestionTypeENUM().ToString() + "))";
                }

                env.AssertString(str2assert);

                foreach (Question q in qG.Questions)
                {
                    str2assert = "(question (Id _" + q.ID + ") (QuestionText " + "\"" +
                        q.Name + "\"" + ") (GroupId _" + qG.GroupID + "))";
                    env.AssertString(str2assert);
                }
            }
        }

        public static void assertAge(int age)
        {
            env.AssertString("");//todo: assert which var
        }

        public static void assertQuestion(double id, bool answer)
        {
            //to paste to load questions
            string str2assert="(choice _" + id + " ";
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
        }

        public static void run()
        {
            env.AssertString("(run)");
        }

        public static void getSymptom()
        {
            //to trace history
        }
    }
}
