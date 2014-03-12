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

                document.Save(Patient.dataPath);
            }

        }
        //http://www.dreamincode.net/forums/topic/218979-linq-to-xml/
        //http://www.dotnetcurry.com/showarticle.aspx?ID=564
        //http://msdn.microsoft.com/en-us/library/bb387089.aspx
        public static void addNewPatient(Assessor a, string nric, string first, string last, DateTime dob)
        {
            createPatientFile();

            Patient p = new Patient(a, nric, first, last, dob);
           
            //load document
            XDocument document = XDocument.Load(Patient.dataPath);

            document.Element("Patients").Add(

                 new XElement("Patient", new XAttribute("id", p.NRIC),
                        new XElement("Last_Name", p.First_Name),
                        new XElement("First_Name", p.First_Name),
                        new XElement("DOB", p.DOB.Ticks),
                        new XElement("AssessmentDate", DateTime.Now.Ticks),
                        
                            new XElement("Assessor",
                                new XElement("AssessorName"), p.Doctor.Name,
                                new XElement("AssessLocation"), p.Doctor.ClinicName)
                        )

                 );

            document.Save(Patient.dataPath);

            //to get back datetime from ticks, do this : 
            //DateTime myDate = new DateTime(numberOfTicks);
            //String test = myDate.ToString("MMMM dd, yyyy");


        }

        public static void addNewPatient(Patient p)
        {
        }

        public static Patient searchPatientByNRIC(string nric)
        {
            return null;
        }

        public static List<Patient> searchPatientByName(string firstName, string lastName)
        {
            List<Patient> pList = new List<Patient>();

            return pList;
        }

        public static void saveCurrentPatient (string nric)
        {
            //todo search by nric, delete current record, add new record
        }

    }
}
