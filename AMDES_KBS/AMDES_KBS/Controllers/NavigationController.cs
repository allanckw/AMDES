using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMDES_KBS.Entity;
using System.Xml.Linq;

namespace AMDES_KBS.Controllers
{
    public class NavigationController
    {
        public static Navigation getNavigation(XElement x)
        {
            if (x != null)
            {
                Navigation nav = new Navigation();

                nav.DestGrpID = int.Parse(x.Element("DestGrp").Value);
                nav.isConclusive = bool.Parse(x.Element("isConclusive").Value);
                nav.isRequireAge = bool.Parse(x.Element("RequireAge").Value);
                nav.Age = (int.Parse(x.Element("Age").Value));

                bool a = bool.Parse(x.Element("LessThanAge").Value);

                if (a)
                {
                    nav.setLessThanAge();
                }
                else
                {
                    nav.setMoreThanEqualAge();
                }

                var diag = (from pa in x.Descendants("Diagnoses").Descendants("Diagnosis")
                            select pa).ToList();

                foreach (var d in diag)
                {
                    nav.addDiagnosis(DiagnosisController.readDiagnosis(d));
                }

                return nav;
            }
            else
            {
                return null;
            }
        }

        public static XElement convertToXML(Navigation nav, string root = "Navigation")
        {
            XElement x = new XElement(root,
                                         new XElement("DestGrp", nav.DestGrpID),
                                         new XElement("RequireAge", nav.isRequireAge),
                                         new XElement("Age", nav.Age),
                                         new XElement("isConclusive", nav.isConclusive),
                                         new XElement("LessThanAge", nav.LessThanAge),
                                         new XElement("MoreThanEqualAge", nav.MoreThanEqualAge),
                                         new XElement("Diagnoses")
                                         );

            if (nav.getDiagnosis().Count > 0)
            {
                for (int k = 0; k < nav.getDiagnosis().Count; k++)
                {
                    Diagnosis d = nav.getDiagnosisAt(k);
                    x.Element("Diagnoses").Add(DiagnosisController.convertToXML(d));
                }
            }

            return x;
        }
    }
}
