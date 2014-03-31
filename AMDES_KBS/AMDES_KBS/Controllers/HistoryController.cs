using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMDES_KBS.Entity;
using System.IO;
using System.Xml.Linq;

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

        public static void updatePatientNavigationHistory(History h)
        {
            createDataFile();

            if (getHistoryByID(h.PatientID) == null)
            {
                addPatientNavigationHistory(h); //if id is not present, just add
            }
            else
            {
                deletePatientNavigationHistory(h.PatientID); //delete and add
                addPatientNavigationHistory(h);
            }

        }

        private  static void addPatientNavigationHistory(History h)
        {
            createDataFile();

            XDocument document = XDocument.Load(History.dataPath);

            XElement newPat = new XElement("History", new XAttribute("pid", h.PatientID));

            foreach (KeyValuePair<int, List<QnHistory>> kvp in h.getHistory())
            {
                //Console.WriteLine("Key : " + kvp.Key.ToString() + ", Value : " + kvp.Value);
                XElement hist = new XElement("Group", new XAttribute("histID", kvp.Key.ToString()));

                for (int i = 0; i < kvp.Value.Count(); i++)
                {
                    QnHistory qnHistory = kvp.Value[i];
                    XElement qhx = new XElement("Question");
                    qhx.Add(new XElement("QID", qnHistory.QuestionID));
                    qhx.Add(new XElement("Answer", qnHistory.Answer));

                    hist.Add(qhx);

                }

                newPat.Add(hist);
            }

            document.Element("Histories").Add(newPat);
            document.Save(History.dataPath);
        }

        private static void deletePatientNavigationHistory(string pid)
        {

            XDocument document = XDocument.Load(History.dataPath);

            if (getHistoryByID(pid) != null)
            {
                (from pa in document.Descendants("History")
                 where pa.Attribute("pid").Value.ToUpper().CompareTo(pid.ToUpper()) == 0
                 select pa).SingleOrDefault().Remove();

                document.Save(History.dataPath);
            }
        }

        public static History getHistoryByID(string pid)
        {
            XDocument document = XDocument.Load(History.dataPath);

            try
            {
                var hist = (from pa in document.Descendants("History")
                           where pa.Attribute("pid").Value.ToUpper().CompareTo(pid.ToUpper()) == 0
                           select pa).SingleOrDefault();

                return readHistoryData(hist);

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

                var hists = (from pa in x.Descendants("Group")
                            select pa).ToList();

                foreach (var g in hists)
                {
                    int gid = int.Parse(g.Attribute("histID").Value);
                    h.createNewHistory(gid);

                    var qns = (from q in x.Descendants("Question")
                               select q).ToList();

                    foreach (var q in qns)
                    {
                        h.updateHistoryItem(gid, q.Element("QID").Value, bool.Parse(q.Element("Answer").Value));
                    }
                }
                return h;
            }
            else
                return null;
        }

    }
}
