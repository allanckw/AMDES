using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Data
{
    class Symptom
    {
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
