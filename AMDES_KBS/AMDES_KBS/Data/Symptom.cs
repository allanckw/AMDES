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
       
        private string name;

        private bool present;

        private DateTime diaDate;

        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public bool SymptomPresent
        {
            get { return present; }
            set { present = value; }
        }

        public string SymptomName
        {
            get { return name; }
            set { name = value; }
        }

        public SymptomDecisionPoint DecisionPoint
        {
            get { return decisionPoint; }
            set { decisionPoint = value; }
        }

        public DateTime DiagnosisDate
        {
            get { return diaDate; }
            set { diaDate = value; }
        }
    }
}
