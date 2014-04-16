using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    class PatientController
    {
        private static void createDataFile()
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
            if (PatientController.searchPatientByID(nric) == null)    //Search if nric exist, if yes throw exception!
            {
                createDataFile();

                Patient p = new Patient(a, nric, first, last, dob);

                //load document
                XDocument document = XDocument.Load(Patient.dataPath);

                XElement newPat = new XElement("Patient", new XAttribute("id", p.NRIC),
                                       new XElement("Last_Name", p.Last_Name),
                                       new XElement("First_Name", p.First_Name),
                                       new XElement("DOB", p.DOB.Ticks),
                                       new XElement("AssessmentDate", DateTime.Now.Ticks),

                                       new XElement("Tests"),
                                       new XElement("Assessor",
                                       new XElement("AssessorName", p.Doctor.Name),
                                       new XElement("AssessLocation", p.Doctor.ClinicName),
                                       new XElement("Symptoms")
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


        //example for update on the fly
        private static void updatePatientStatus(string id, PatientStatus s)
        {
            XDocument document = XDocument.Load(Patient.dataPath);
            if (PatientController.searchPatientByID(id) != null)
            {
                (from pa in document.Descendants("Patient")
                 where pa.Attribute("id").Value.ToUpper().CompareTo(id.ToUpper()) == 0
                 select pa).Select(e => e.Element("Status")).Single().SetValue(s);

                document.Save(Patient.dataPath);
            }
        }

        private static void addPatient(Patient p)
        {
            XDocument document = XDocument.Load(Patient.dataPath);


            XElement newPat = new XElement("Patient", new XAttribute("id", p.NRIC),
                                    new XElement("Last_Name", p.Last_Name),
                                    new XElement("First_Name", p.First_Name),
                                    new XElement("DOB", p.DOB.Ticks),
                                    new XElement("AssessmentDate", p.AssessmentDate.Ticks),
                                    //new XElement("Status", p.getStatus()),
                                    //new XElement("Tests"),
                                    new XElement("Assessor",
                                    new XElement("AssessorName", p.Doctor.Name),
                                    new XElement("AssessLocation", p.Doctor.ClinicName)),
                                    new XElement("Symptoms"),
                                    new XElement("Diagnoses")
                                    );

            //this test is for phase 2
            //if (p.TestsList.Count > 0)
            //{
            //    for (int i = 0; i < p.TestsList.Count; i++)
            //    {
            //        Test t = p.TestsList.ElementAt(i);
            //        newPat.Element("Tests").Add(TestController.writeTest(t, p.Doctor));
            //    }
            //}

            //if (p.SymptomsList.Count > 0)
            //{
            //    for (int i = 0; i < p.SymptomsList.Count; i++)
            //    {
            //        Symptom s = p.SymptomsList.ElementAt(i);
            //        newPat.Element("Symptoms").Add(SymptomController.writeSymptom(s));
            //    }
            //}

            //if (p.Diagnoses.Count > 0)
            //{
            //    for (int i = 0; i < p.Diagnoses.Count; i++)
            //    {
            //        newPat.Element("Diagnoses").Add(DiagnosisController.convertToXML(p.Diagnoses[i]));
            //    }
            //}

            document.Element("Patients").Add(newPat);
            document.Save(Patient.dataPath);
        }

        public static List<Patient> searchPatient(string criteria)
        {
            createDataFile();

            Patient p = searchPatientByID(criteria);
            if (p != null)
            {
                List<Patient> pList = new List<Patient>();
                pList.Add(p);
                return pList;
            }
            else
            {
                return searchPatientByName(criteria).OrderBy(x => x.Last_Name).ToList();
            }
        }

        public static Patient searchPatientByID(string nric)
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



                //var tests = (from test in x.Descendants("Tests").Descendants("Test")
                //             select test).ToList();

                //foreach (var t in tests)
                //{
                //    p.addTest(TestController.readPatientTest(t));
                //}

                var symptoms = (from syms in x.Descendants("Symptoms").Descendants("Symptom")
                                select syms).ToList();

                //foreach (var s in symptoms)
                //{
                //    p.addSymptom(SymptomController.readPatientSymptoms(s));
                //}

                //var diagnoses = (from syms in x.Descendants("Diagnoses").Descendants("Diagnosis")
                //                 select syms).ToList();

                //foreach (var d in diagnoses)
                //{
                //    p.addDiagnosis(DiagnosisController.readDiagnosis(d));
                //}
            }
            else
            {
                return null;
            }

            if (p.NRIC.Length == 0)
                return null;
            else
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
                    if (x != null)
                    {
                        pList.Add(readPatientData(x));
                    }
                }

            }

            return pList.OrderBy(x => x.Last_Name).ToList(); 
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
                    Patient p = readPatientData(x);
                    if (p != null)
                    {
                        pList.Add(p);
                    }
                }
            }

            return pList.OrderBy(x => x.Last_Name).ToList();
        }

        public static void updatePatient(Patient p)
        {
            createDataFile();

            if (p.NRIC.Length == 0)
                return;

            if (searchPatientByID(p.NRIC) == null)
            {
                addPatient(p); //if RID is not present, just add
            }
            else
            {
                deletePatient(p.NRIC); //delete and add
                addPatient(p);
            }
        }

        public static void deletePatient(string pid)
        {
            XDocument document = XDocument.Load(Patient.dataPath);

            if (PatientController.searchPatientByID(pid) != null)
            {
                (from pa in document.Descendants("Patient")
                 where pa.Attribute("id").Value.ToUpper().CompareTo(pid.ToUpper()) == 0
                 select pa).SingleOrDefault().Remove();

                document.Save(Patient.dataPath);
            }
        }

    }
}
