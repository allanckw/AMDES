using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Data
{
    public enum SymptomDecisionPoint
    {
        A,
        B1, B2, B3, B4,
        C1, C2, C3_14,
        D
    }
    class Symptom
    {
        private SymptomDecisionPoint decisionPoint;

        public SymptomDecisionPoint DecisionPoint
        {
            get { return decisionPoint; }
            set { decisionPoint = value; }
        }

        
        private string name;

        public string SymptomName
        {
            get { return name; }
            set { name = value; }
        }

        private bool present;

        public bool SymptomPresent
        {
            get { return present; }
            set { present = value; }
        }

    }
}
