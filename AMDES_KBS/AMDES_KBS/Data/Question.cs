using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Data
{
    //public enum QuestionType
    //{
    //    BOOL_TYPE,
    //    COUNT_TYPE
    //}

    class Question
    {
        private string name;
        private string id;
        private SymptomDecisionPoint decisionPoint;
        //private QuestionType qType;

        //public QuestionType QnType
        //{
        //    get { return qType; }
        //    set { qType = value; }
        //}

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
       

        public SymptomDecisionPoint DecisionPoint
        {
            get { return decisionPoint; }
            set { decisionPoint = value; }
        }
    }
}
