using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
   public class Assessor
    {
        public static string dataPath = System.Web.HttpContext.Current.Server.MapPath(@"Data\Add\Assessor.xml");

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
