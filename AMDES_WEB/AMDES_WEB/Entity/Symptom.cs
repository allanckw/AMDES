using System;

namespace AMDES_KBS.Entity
{
    public class Symptom : IComparable<Symptom>
    {

        private string name;
        private string diagnosedByID;
        private DateTime diaDate;
        private string scoreString;

        public Symptom()
        {
        }

        public Symptom(string name, string diagID)
        {
            this.name = name;
            this.diagnosedByID = diagID;
            this.diaDate = DateTime.Now;
            this.scoreString = "";
        }


        public string ScoreString
        {
            get { return scoreString; }
            set { scoreString = value; }
        }

        public string DiagnosedByID
        {
            get { return diagnosedByID; }
            set { diagnosedByID = value; }
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

        public int CompareTo(Symptom s)
        {
            return this.name.ToUpper().CompareTo(s.SymptomName.ToUpper());
        }
    }
}
