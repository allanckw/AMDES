using System;
using System.Xml.Linq;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    class SymptomController
    {
        public static Symptom readPatientSymptoms(XElement x)
        {
            if (x != null)
            {
                Symptom s = new Symptom();
                s.DiagnosedByID = x.Element("DiagnosedByID").Value;
                s.DiagnosisDate = new DateTime(long.Parse(x.Element("DiagnosisDate").Value));
                s.SymptomName = x.Element("SymptomName").Value;
                return s;
            }
            return null;
        }

        public static XElement writeSymptom(Symptom s)
        {
            XElement x = new XElement("Symptom",
                            new XElement("SymptomName", s.SymptomName),
                            new XElement("DiagnosisDate", s.DiagnosisDate.Ticks),
                            new XElement("DiagnosedByID", s.DiagnosedByID));

            return x;
        }
    }

}
