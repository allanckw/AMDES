using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class Symptom : IComparable<Symptom>
    {
        public Symptom()
        {
        }

        public Symptom(string name, int diagID)
        {
            this.name = name;
            this.diaDate = DateTime.Now;
        }

        private int diagnosedByID;

        public int DiagnosedByID
        {
            get { return diagnosedByID; }
            set { diagnosedByID = value; }
        }

        private string name;

        private DateTime diaDate;

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

        public int CompareTo(Symptom s)
        {
            return this.name.ToUpper().CompareTo(s.SymptomName.ToUpper());
        }
    }
}
