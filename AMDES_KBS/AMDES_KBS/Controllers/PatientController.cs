using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using AMDES_KBS.Entity;

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
            if (PatientController.searchPatientByNRIC(nric) == null)    //Search if nric exist, if yes throw exception!
            {
                createPatientFile();

                Patient p = new Patient(a, nric, first, last, dob);

                //load document
                XDocument document = XDocument.Load(Patient.dataPath);

                XElement newPat = new XElement("Patient", new XAttribute("id", p.NRIC),
                                       new XElement("Last_Name", p.Last_Name),
                                       new XElement("First_Name", p.First_Name),
                                       new XElement("DOB", p.DOB.Ticks),
                                       new XElement("AssessmentDate", DateTime.Now.Ticks),
                                       new XElement("Status", p.Status),
                                       new XElement("Tests"),
                    //new XElement("Symptoms"),

                                           new XElement("Assessor",
                                               new XElement("AssessorName", p.Doctor.Name),
                                               new XElement("AssessLocation", p.Doctor.ClinicName)
                                           )
                                       );

                document.Element("Patients").Add(newPat);
                document.Save(Patient.dataPath);

                //to get back datetime from ticks, do this : 
                //DateTime myDate = new DateTime(numberOfTicks);
                //String test = myDate.ToString("MMMM dd, yyyy");
            }
            else
            {
                throw new InvalidOperationException("Patient with ID " + "already exist!");
            }

        }

        public static void updatePatientStatus(string id, PatientStatus s)
        {
            XDocument document = XDocument.Load(Patient.dataPath);
            if (PatientController.searchPatientByNRIC(id) != null)
            {
                (from pa in document.Descendants("Patient")
                 where pa.Attribute("id").Value.ToUpper().CompareTo(id.ToUpper()) == 0
                 select pa).Select(e => e.Element("Status")).Single().SetValue(s);

                document.Save(Patient.dataPath);
            }
        }

        public static void addPatient(Patient p)
        {
            createPatientFile();
            XDocument document = XDocument.Load(Patient.dataPath);


            XElement newPat = new XElement("Patient", new XAttribute("id", p.NRIC),
                                    new XElement("Last_Name", p.Last_Name),
                                    new XElement("First_Name", p.First_Name),
                                    new XElement("DOB", p.DOB.Ticks),
                                    new XElement("AssessmentDate", p.AssessmentDate.Ticks),
                                    new XElement("Status", p.Status),
                                    new XElement("Tests"),
                //new XElement("Symptoms"),

                                        new XElement("Assessor",
                                            new XElement("AssessorName", p.Doctor.Name),
                                            new XElement("AssessLocation", p.Doctor.ClinicName)
                                        )
                                    );

            if (p.TestsList.Count > 0)
            {
                for (int i = 0; i < p.TestsList.Count; i++)
                {
                    Test t = p.TestsList.ElementAt(i);

                    newPat.Element("Tests").Add(
                        new XElement("Test",
                            new XElement("TestName", t.TestName),
                            new XElement("Status", t.Status),
                            new XElement("OrderedDate", t.OrderedDate.GetValueOrDefault().Ticks),
                            new XElement("ReportDate", t.ReportDate.GetValueOrDefault().Ticks),
                                    new XElement("Assessor",
                                        new XElement("AssessorName", p.Doctor.Name),
                                        new XElement("AssessLocation", p.Doctor.ClinicName)
                                    )
                            )
                        );
                }
            }

            //if (p.SymptomsList.Count > 0)
            //{
            //    for (int i = 0; i < p.SymptomsList.Count; i++)
            //    {
            //        Symptom s = p.SymptomsList.ElementAt(i);
            //        newPat.Element("Symptoms").Add(
            //            new XElement("Symptom", new XAttribute("id", s.ID),
            //                new XElement("SymptomName", s.SymptomName),
            //                new XElement("Exist", s.SymptomPresent),
            //                new XElement("DiagnosisDate", s.DiagnosisDate.Ticks)

            //                )
            //            );

            //    }
            //}
            document.Element("Patients").Add(newPat);
            document.Save(Patient.dataPath);
        }

        public static Patient searchPatientByNRIC(string nric)
        {
            XDocument document = XDocument.Load(Patient.dataPath);

            try
            {
                var patients = (from pa in document.Descendants("Patient")
                                where pa.Attribute("id").Value.ToUpper().CompareTo(nric.ToUpper()) == 0
                                select pa).SingleOrDefault();

                return readPatientData(patients);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        private static Patient readPatientData(XElement x)
        {
            Patient p = new Patient();
            if (x != null)
            {
                p.NRIC = x.Attribute("id").Value;

                p.First_Name = x.Element("First_Name").Value;
                p.Last_Name = x.Element("Last_Name").Value;

                p.Doctor = AssessorController.readAssessor(x.Element("Assessor"));


                p.AssessmentDate = new DateTime(long.Parse(x.Element("AssessmentDate").Value));
                p.DOB = new DateTime(long.Parse(x.Element("DOB").Value));

                //TODO: TEST AND SYMPTOM LOAD
            }
            else
            {
                return null;
            }
            return p;
        }

        public static List<Patient> searchPatientByName(string name = "")
        {
            //var a = from h in xdoc.Root.Elements()
            //where h.Element().Value.Contains("1234") // like '%1234%'
            //select h;
            //For the SQL-ish like '%value' you can use EndsWith, and for like 'value%' StartsWith
            //list of names of the people below 60 years of age

            XDocument document = XDocument.Load(Patient.dataPath);
            List<Patient> pList = new List<Patient>();

            var patients = (from pat in document.Descendants("Patient")
                            select pat).ToList();

            if (name.Length > 0)
            {
                var first = (from pat in patients
                             where pat.Element("First_Name").Value.ToUpper().Contains(name.ToUpper())
                             select pat).ToList();


                foreach (var x in first)
                {
                    Patient p = readPatientData(x);
                    if (!pList.Contains(p))
                    {
                        pList.Add(p);
                    }
                }

                var last = (from pat in patients
                            where pat.Element("Last_Name").Value.ToUpper().Contains(name.ToUpper())
                            select pat).ToList();

                foreach (var x in last)
                {
                    Patient p = readPatientData(x);
                    if (!pList.Contains(p))
                    {
                        pList.Add(p);
                    }
                }
            }
            else
            {
                foreach (var x in patients)
                {
                    pList.Add(readPatientData(x));
                }

            }

            return pList;
        }

        public static List<Patient> getAllPatients()
        {
            List<Patient> pList = new List<Patient>();
            if (File.Exists(Patient.dataPath))
            {
                XDocument document = XDocument.Load(Patient.dataPath);

                var patients = (from pa in document.Descendants("Patient")
                                select pa).ToList();

                foreach (var x in patients)
                {
                    pList.Add(readPatientData(x));
                }
            }

            return pList;
        }

        public static void saveCurrentPatient(Patient p)
        {
            PatientController.deletePatient(p.NRIC);
            PatientController.addPatient(p);
        }

        public static void deletePatient(string nric)
        {
            XDocument document = XDocument.Load(Patient.dataPath);

            if (PatientController.searchPatientByNRIC(nric) != null)
            {
                (from pa in document.Descendants("Patient")
                 where pa.Attribute("id").Value.ToUpper().CompareTo(nric.ToUpper()) == 0
                 select pa).SingleOrDefault().Remove();

                document.Save(Patient.dataPath);
            }
        }

    }
}
