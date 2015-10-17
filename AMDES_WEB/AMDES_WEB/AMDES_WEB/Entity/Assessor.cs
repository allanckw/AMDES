using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;

namespace AMDES_KBS.Entity
{
   public class Assessor
    {
       public static string dataPath;

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string clinicName;

        public string ClinicName
        {
            get { return clinicName; }
            set { clinicName = value; }
        }

        public Assessor(string name, string location)
        {
            this.name = name;
            this.clinicName = location;
        }

        public Assessor()
        {
        }

    }
}
