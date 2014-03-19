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
            Navigation nav = new Navigation();

            nav.DestGrpID = (int.Parse(x.Attribute("id").Value));
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

    }
}
