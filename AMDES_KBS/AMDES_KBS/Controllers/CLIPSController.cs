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
        }

        public static void assertAge(int age)
        {
            env.AssertString("");//todo: assert which var
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
