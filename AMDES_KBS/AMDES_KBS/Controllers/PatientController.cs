using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using AMDES_KBS.Data;

namespace AMDES_KBS.Controllers
{
    class PatientController
    {
        private static void createPatientFile()
        {
            //create xml document from scratch
            if (!File.Exists(Patient.dataPath))
            {
                XDocument document = new XDocument(

                    new XDeclaration("1.0", "utf-8", "yes"),

                    new XComment("AMDES Patients xml"),
                        new XElement("Patients")
                );
            }

        }

        public static void addNewPatient(Assessor a, string nric, string first, string last, DateTime dob)
        {
            createPatientFile();

            Patient p = new Patient(a, nric, first, last, dob);
            XDocument document = XDocument.Load(Patient.dataPath);


        }
    }
}
