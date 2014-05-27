using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    class PatAttributeController
    {
        private static void createDataFile()
        {
            //create xml document from scratch
            if (!File.Exists(PatAttribute.dataPath))
            {
                XDocument document = new XDocument(

                    new XDeclaration("1.0", "utf-8", "yes"),

                    new XComment("AMDES Patient Attribute settings xml"),
                        new XElement("PatientsAttributes")
                );

                document.Save(PatAttribute.dataPath);
            }

        }

        public static void updateAttribute(PatAttribute p)
        {
            createDataFile();

            if (p.AttributeName.Length == 0)
                return;

            if (searchPatientAttribute(p.AttributeName) == null)
            {
                addPatientAttribute(p); //if RID is not present, just add
            }
            else
            {
                deleteAttribute(p.AttributeName); //delete and add
                addPatientAttribute(p);
            }
        }

        private static void addPatientAttribute(PatAttribute p)
        {
            createDataFile();

            XDocument document = XDocument.Load(PatAttribute.dataPath);

            XElement newPat = new XElement("PatAttr", new XAttribute("name", p.AttributeName),
                                    new XElement("Type", p.getAttributeType()),
                                    new XElement("MinNumericValue", p.MinNumericValue),
                                    new XElement("MaxNumericValue", p.MaxNumericValue),
                                    new XElement("Categories")
                                    );

            if (p.CategoricalVals.Count > 0)
            {
                for (int i = 0; i < p.CategoricalVals.Count; i++)
                {
                    XElement diag = new XElement("Category", p.CategoricalVals[i]);

                    newPat.Element("Categories").Add(diag);
                }
            }

            document.Element("PatientsAttributes").Add(newPat);
            document.Save(PatAttribute.dataPath);
        }

        public static void deleteAttribute(string name)
        {
            XDocument document = XDocument.Load(PatAttribute.dataPath);

            if (searchPatientAttribute(name) != null)
            {
                (from pa in document.Descendants("PatAttr")
                 where pa.Attribute("name").Value.ToUpper().CompareTo(name.ToUpper()) == 0
                 select pa).SingleOrDefault().Remove();

                document.Save(PatAttribute.dataPath);
            }
        }

        public static PatAttribute searchPatientAttribute(string name)
        {

            XDocument document = XDocument.Load(PatAttribute.dataPath);

            try
            {
                var patients = (from pa in document.Descendants("PatAttr")
                                where pa.Attribute("name").Value.ToUpper().CompareTo(name.ToUpper()) == 0
                                select pa).SingleOrDefault();

                return readPatAttribute(patients);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


        public static List<PatAttribute> getAllAttributes()
        {
            List<PatAttribute> pList = new List<PatAttribute>();
            if (File.Exists(PatAttribute.dataPath))
            {
                XDocument document = XDocument.Load(PatAttribute.dataPath);

                var attr = (from pa in document.Descendants("PatAttr")
                                select pa).ToList();

                foreach (var x in attr)
                {
                    PatAttribute p = readPatAttribute(x);
                    if (p != null)
                    {
                        pList.Add(p);
                    }
                }
            }

            return pList.OrderBy(x => x.AttributeName).ToList();
        }

        private static PatAttribute readPatAttribute(XElement x)
        {
            PatAttribute p = new PatAttribute();
            if (x != null)
            {
                p.AttributeName = x.Attribute("name").Value;
                p.setAttributeType(int.Parse(x.Element("Type").Value));
                p.MinNumericValue = int.Parse(x.Element("MinNumericValue").Value);
                p.MaxNumericValue = int.Parse(x.Element("MaxNumericValue").Value);



                var cat = (from syms in x.Descendants("Categories").Descendants("Category")
                           select syms).ToList();

                if (cat.Count > 0)
                {
                    foreach (var c in cat)
                    {
                        string cate = readCategory(c);
                        if (cate.Length > 0)
                            p.addCategoricalValue(cate);
                    }
                }

            }
            else
            {
                return null;
            }

            if (p.AttributeName.Length == 0)
                return null;
            else
                return p;
        }


        private static string readCategory(XElement x)
        {
            if (x != null)
            {
                return x.Value;
            }
            else
                return "";
        }
    }
}
