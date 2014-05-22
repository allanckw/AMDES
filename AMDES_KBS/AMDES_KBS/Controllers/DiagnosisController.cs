using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    public class DiagnosisController
    {

        private static int groupIDCounter = 1;

        private static void createDataFile()
        {
            //create xml document from scratch
            if (!File.Exists(Diagnosis.dataPath))
            {
                XDocument document = new XDocument(

                    new XDeclaration("1.0", "utf-8", "yes"),

                    new XComment("AMDES Diagnoses xml"),
                        new XElement("Diagnoses")
                );

                document.Save(Diagnosis.dataPath);
            }
        }

        public static int getNextDiagnosisID()
        {   //Call this function whenever you click a new Diagnosis !!! 
            //Linkage Determine here and set using constructor
            groupIDCounter++;
            return groupIDCounter - 1;
        }

        public static void updateDiagnosis(Diagnosis d)
        {
            createDataFile();

            if (DiagnosisController.getDiagnosisByID(d.RID) == null)
            {
                DiagnosisController.addDiagnosis(d); //if id is not present, just add
            }
            else
            {
                DiagnosisController.deleteDiagnosis(d.RID, true); //delete and add
                DiagnosisController.addDiagnosis(d);
            }
        }

        private static void addDiagnosis(Diagnosis d)
        {
            XDocument document = XDocument.Load(Diagnosis.dataPath);


            document.Element("Diagnoses").Add(convertToXML(d));
            document.Save(Diagnosis.dataPath);
        }

        public static XElement convertToXML(Diagnosis d)
        {
            XElement x = new XElement("Diagnosis", new XAttribute("diagID", d.RID),
                             new XElement("Comment", d.Comment),
                             new XElement("Header", d.Header),
                             new XElement("Link", d.Link),
                             new XElement("LinkDesc", d.LinkDesc),
                             new XElement("RetrieveSymptom", d.RetrieveSym),
                             new XElement("RetrieveFrom"),
                             new XElement("RetrieveBelow65", d.AgeBelow65),
                             new XElement("isRes", d.IsResource)

                             );

            if (d.RetrieveSym)
            {
                for (int i = 0; i < d.RetrievalIDList.Count; i++)
                {

                    XElement ccq = new XElement("QGrpID", d.RetrievalIDList[i]);
                    x.Element("RetrieveFrom").Add(ccq);
                }

            }

            return x;

        }

        public static List<Diagnosis> getAllDiagnosis() //call this on form onload in settings
        {
            List<Diagnosis> pList = new List<Diagnosis>();

            if (File.Exists(Diagnosis.dataPath))
            {
                XDocument document = XDocument.Load(Diagnosis.dataPath);

                var diags = (from pa in document.Descendants("Diagnosis")
                             select pa).ToList();

                foreach (var x in diags)
                {
                    Diagnosis d = readDiagnosis(x);
                    pList.Add(d);
                    if (groupIDCounter <= d.RID)
                    {
                        groupIDCounter = d.RID + 1;
                    }
                }

                return pList.OrderBy(x => x.Header).ToList(); 
            }
            else
            {
                createDataFile();
                return pList; // return empty list
            }
        }

        public static Diagnosis readDiagnosis(XElement x)
        {
            if (x != null)
            {
                Diagnosis d = new Diagnosis();
                d.RID = int.Parse(x.Attribute("diagID").Value);
                d.Comment = x.Element("Comment").Value;
                d.Header = x.Element("Header").Value;
                d.Link = x.Element("Link").Value;
                d.LinkDesc = x.Element("LinkDesc").Value;

                //RetrieveBelow65
                d.AgeBelow65 = bool.Parse(x.Element("RetrieveBelow65").Value);

                d.RetrieveSym = bool.Parse(x.Element("RetrieveSymptom").Value);

                d.IsResource = bool.Parse(x.Element("isRes").Value);

                var cq = (from pa in x.Descendants("RetrieveFrom").Descendants("QGrpID")
                          select pa).ToList();

                foreach (var cq1 in cq)
                {
                    d.addRetrievalID(int.Parse(cq1.Value));
                }

                return d;
            }
            else
            {
                return null;
            }
        }

        public static Diagnosis getDiagnosisByID(int id)
        {
            XDocument document = XDocument.Load(Diagnosis.dataPath);

            try
            {
                var diag = (from pa in document.Descendants("Diagnosis")
                            where int.Parse(pa.Attribute("diagID").Value) == id
                            select pa).SingleOrDefault();

                return readDiagnosis(diag);

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static List<Diagnosis> getResourceRules()
        {
            XDocument document = XDocument.Load(Diagnosis.dataPath);
            List<Diagnosis> resList = new List<Diagnosis>();
            try
            {
                var res = (from pa in document.Descendants("Diagnosis")
                            where bool.Parse(pa.Attribute("isRes").Value) == true
                           select pa).ToList();

                foreach (var x in res)
                {
                    Diagnosis d = readDiagnosis(x);
                    if (!resList.Contains(d))
                    {
                        resList.Add(d);
                    }
                }

                return resList.OrderBy(x => x.RID).ToList();

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static void deleteDiagnosis(int id, bool update = false)
        {
            XDocument document = XDocument.Load(Diagnosis.dataPath);

            if (DiagnosisController.getDiagnosisByID(id) != null)
            {
                (from pa in document.Descendants("Diagnosis")
                 where int.Parse(pa.Attribute("diagID").Value) == id
                 select pa).SingleOrDefault().Remove();

                document.Save(Diagnosis.dataPath);
                if (!update)
                {
                    NavigationController.deleteDiagnosisID(id);
                }

            }
        }
    }
}
