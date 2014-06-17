using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    public class HistoryController
    {
        private static void createDataFile()
        {
            //create xml document from scratch
            if (!File.Exists(History.dataPath))
            {
                XDocument document = new XDocument(

                    new XDeclaration("1.0", "utf-8", "yes"),

                    new XComment("AMDES Histories xml"),
                        new XElement("Histories")
                );

                document.Save(History.dataPath);
            }
        }

        public static void updatePatientNavigationHistory(History h, DateTime assDate)
        {
            createDataFile();

            if (getHistoryByID(h.PatientID, assDate) == null)
            {
                addPatientNavigationHistory(h); //if id is not present, just add
            }
            else
            {
                deletePatientNavigationHistory(h.PatientID, h.AssessmentDate); //delete and add
                addPatientNavigationHistory(h);
            }

        }

        private static void addPatientNavigationHistory(History h)
        {
            createDataFile();

            XDocument document = XDocument.Load(History.dataPath);

            XElement newPat = new XElement("History", new XAttribute("pid", h.PatientID),
                                new XAttribute("AssessmentDate", h.AssessmentDate.Date.Ticks));

            foreach (KeyValuePair<int, List<QnHistory>> kvp in h.getHistory())
            {
                //Console.WriteLine("Key : " + kvp.Key.ToString() + ", Value : " + kvp.Value);
                XElement hist = new XElement("Group", new XAttribute("histID", kvp.Key.ToString()));

                for (int i = 0; i < kvp.Value.Count(); i++)
                {
                    QnHistory qnHistory = kvp.Value[i];
                    XElement qhx = new XElement("Question");
                    qhx.Add(new XAttribute("GrpID", kvp.Key));
                    qhx.Add(new XElement("QID", qnHistory.QuestionID));
                    qhx.Add(new XElement("Answer", qnHistory.Answer));

                    hist.Add(qhx);

                }

                newPat.Add(hist);


            }

            if (h.SymptomsList.Count > 0)
            {
                XElement sy = new XElement("Symptoms");
                foreach (Symptom s in h.SymptomsList)
                {
                    if (s != null)
                        sy.Add(SymptomController.writeSymptom(s));
                }
                newPat.Add(sy);
            }

            if (h.Diagnoses.Count > 0)
            {
                XElement hy = new XElement("Diagnoses");
                foreach (Diagnosis d in h.Diagnoses)
                {
                    if (d != null)
                        hy.Add(DiagnosisController.convertToXML(d));
                }


                newPat.Add(hy);
            }

            newPat.Add(new XElement("Status", h.getStatus()));

            if (h.CompletedDate != null)
            {
                newPat.Add(new XElement("CompletedDate", h.CompletedDate.Value.Ticks));
            }
            else
            {
                newPat.Add(new XElement("CompletedDate", 0));
            }
            document.Element("Histories").Add(newPat);
            document.Save(History.dataPath);
        }

        private static void deletePatientNavigationHistory(string pid, DateTime assDate)
        {

            XDocument document = XDocument.Load(History.dataPath);

            if (getHistoryByID(pid, assDate) != null)
            {
                (from pa in document.Descendants("History")
                 where pa.Attribute("pid").Value.ToUpper().CompareTo(pid.ToUpper()) == 0 &&
                 long.Parse(pa.Attribute("AssessmentDate").Value) == assDate.Date.Ticks
                 select pa).SingleOrDefault().Remove();

                document.Save(History.dataPath);
            }
        }

        public static bool checkForDraft(string pid)
        {
            XDocument document = XDocument.Load(History.dataPath);
            var hist = (from pa in document.Descendants("History")
                        where pa.Attribute("pid").Value.ToUpper().CompareTo(pid.ToUpper()) == 0 &&
                        int.Parse(pa.Element("Status").Value) == (int)PatientStatus.DRAFT
                        select pa).ToList();

            return hist.Count > 0;
        }

        public static List<History> getHistoryByID(string pid)
        {
            createDataFile();
            XDocument document = XDocument.Load(History.dataPath);
            List<History> hList = new List<History>();

            try
            {
                var hist = (from pa in document.Descendants("History")
                            where pa.Attribute("pid").Value.ToUpper().CompareTo(pid.ToUpper()) == 0
                            select pa).ToList();

                foreach (var h in hist)
                {
                    History hy = readHistoryData(h);
                    if (hy != null)
                        hList.Add(hy);
                }

                return hList.OrderBy(x => x.AssessmentDate).ToList();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static History getHistoryByID(string pid, DateTime assDate)
        {
            XDocument document = XDocument.Load(History.dataPath);
            List<History> hList = new List<History>();

            try
            {
                if (CLIPSController.savePatient == false)
                {
                    var h = (from pa in document.Descendants("History")
                             where pa.Attribute("pid").Value.ToUpper().CompareTo(pid.ToUpper()) == 0 &&
                             long.Parse(pa.Attribute("AssessmentDate").Value) == 0
                             select pa).SingleOrDefault();
                    return readHistoryData(h);
                }
                else
                {
                    var h = (from pa in document.Descendants("History")
                             where pa.Attribute("pid").Value.ToUpper().CompareTo(pid.ToUpper()) == 0 &&
                             long.Parse(pa.Attribute("AssessmentDate").Value) == assDate.Date.Ticks
                             select pa).SingleOrDefault();
                    return readHistoryData(h);
                }

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static bool isHistoryExist(string pid)
        {
            XDocument document = XDocument.Load(History.dataPath);

            var hist = (from pa in document.Descendants("History")
                        where pa.Attribute("pid").Value.ToUpper().CompareTo(pid.ToUpper()) == 0
                        select pa).SingleOrDefault();

            return hist != null; //not null means got so exist is true
        }

        private static History readHistoryData(XElement x)
        {
            if (x != null)
            {
                History h = new History();
                h.PatientID = x.Attribute("pid").Value;
                h.AssessmentDate = new DateTime(long.Parse(x.Attribute("AssessmentDate").Value));

                var hists = (from pa in x.Descendants("Group")
                             select pa).ToList();

                foreach (var g in hists)
                {
                    int gid = int.Parse(g.Attribute("histID").Value);
                    h.createNewHistory(gid);

                    var qns = (from q in x.Descendants("Question")
                               where int.Parse(q.Attribute("GrpID").Value) == gid
                               select q).ToList();

                    foreach (var q in qns)
                    {
                        h.updateHistoryItem(gid, int.Parse(q.Element("QID").Value), bool.Parse(q.Element("Answer").Value));
                    }
                    h.setStatus(int.Parse(x.Element("Status").Value));
                    long d = long.Parse(x.Element("CompletedDate").Value);
                    if (d > 0)
                        h.CompletedDate = new DateTime(d);
                    else
                        h.CompletedDate = null;
                }

                var symptoms = (from syms in x.Descendants("Symptoms").Descendants("Symptom")
                                select syms).ToList();

                foreach (var s in symptoms)
                {
                    h.addSymptom(SymptomController.readPatientSymptoms(s));
                }

                var diagnoses = (from syms in x.Descendants("Diagnoses").Descendants("Diagnosis")
                                 select syms).ToList();

                foreach (var d in diagnoses)
                {
                    h.addDiagnosis(DiagnosisController.readDiagnosis(d));
                }

                return h;
            }
            else
                return null;
        }

    }
}
