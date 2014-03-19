using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class Navigation
    {
        private int destGrpID;
        private bool conclusive;
        private bool moreThanEqualAge;
        private bool requireAge;

        public bool isRequireAge
        {
            get { return requireAge; }
            set { requireAge = value; }
        }
        private int age = 65;
        List<Diagnosis> diagnosis;


        public Navigation()
        {
            diagnosis = new List<Diagnosis>();

        }

        public List<Diagnosis> getDiagnosis()
        {
            return diagnosis;
        }

        public void addDiagnosis(Diagnosis d)
        {
            if (conclusive)
            {
                diagnosis.Add(d);
            }
            else
            {
                throw new InvalidOperationException("Cannot add diagnosis when it is inconclusive!");
            }
        }

        public void removeDiagnosisAt(int i)
        {
            if (diagnosis.Count > 0 && i < diagnosis.Count)
            {
                diagnosis.RemoveAt(i);
            }
        }

        public Diagnosis getDiagnosisAt(int i)
        {
            if (diagnosis.Count > 0 && i < diagnosis.Count)
            {
                return diagnosis.ElementAt(i);
            }
            else
            {
                return null;
            }
        }

        public void clearDiagnosis()
        {
            diagnosis.Clear();
        }

        public int DestGrpID
        {
            get { return destGrpID; }
            set
            {
                if (!this.conclusive)
                {
                    destGrpID = value;
                }
                else
                {
                    throw new InvalidOperationException("Cannot set destination when it is conculsive!");
                }
            }
        }

        public bool isConclusive
        {
            get { return conclusive; }
            set { conclusive = value; }
        }

        public void setMoreThanEqualAge()
        {
            this.moreThanEqualAge = true;
            this.lessThanAge = !this.MoreThanEqualAge;
        }

        public void setLessThanAge()
        {
            this.moreThanEqualAge = false;
            this.lessThanAge = !this.MoreThanEqualAge;
        }

        public bool MoreThanEqualAge
        {
            get { return moreThanEqualAge; }
        }
        private bool lessThanAge;

        public bool LessThanAge
        {
            get { return lessThanAge; }
        }

        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }
    }
}
