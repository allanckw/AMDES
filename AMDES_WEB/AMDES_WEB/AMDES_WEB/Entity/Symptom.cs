using System;

namespace AMDES_KBS.Entity
{
    public class Symptom : IComparable<Symptom>
    {
        public Symptom()
        {
        }

        public Symptom(string name, string diagID)
        {
            this.name = name;
            this.diagnosedByID = diagID;
            this.diaDate = DateTime.Now;
        }

        private string diagnosedByID;

        public string DiagnosedByID
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
