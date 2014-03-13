using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMDES_KBS.Data;
using System.Xml.Linq;

namespace AMDES_KBS.Controllers
{
    class AssessorController
    {
        public static void writeAssessor(Assessor a)
        {
            //create xml document from scratch

            XDocument document = new XDocument(

                new XDeclaration("1.0", "utf-8", "yes"),

                new XComment("AMDES ASSESSOR xml"),
                    new XElement("Assessor",
                        new XElement("Name", a.Name),
                        new XElement("Location", a.ClinicName)
                        )
            );

            //save constructed document
            document.Save(Assessor.dataPath);

        }

        public static Assessor readAssessor()
        {
            XElement xelement = XElement.Load(Assessor.dataPath);
            IEnumerable<XElement> assessors = xelement.Elements();

            Assessor a = new Assessor();

            a.Name = xelement.Element("Name").Value.ToString();
            a.ClinicName = xelement.Element("Location").Value.ToString();

            return a;
        }

        public static Assessor readAssessor(XElement x)
        {
            Assessor a = new Assessor(x.Element("AssessorName").Value, x.Element("AssessLocation").Value);

            return a;
        }
    }
}
