using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class Symptom
    {
       
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

        public DateTime DiagnosisDate
        {
            get { return diaDate; }
            set { diaDate = value; }
        }
    }
}
