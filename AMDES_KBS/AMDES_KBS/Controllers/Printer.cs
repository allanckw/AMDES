using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    class Printer
    {

        public static FlowDocument writeFlowDoc(List<Diagnosis> allDiagnoses)
        {
            FlowDocument fdPrint = new FlowDocument();

            Paragraph p = new Paragraph();
            p.FontSize = 16;
            p.FontWeight = FontWeights.Bold;
            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Colors.Blue);
            p.Foreground = brush;
            p.Inlines.Add(new Run("Aid for Dementia Diagnosis (ADD) - Patient Report"));
            fdPrint.Blocks.Add(p);

            if (CLIPSController.savePatient == false)
            {
                p = new Paragraph();
                p.FontSize = 15;
                p.FontWeight = FontWeights.Bold;
                p.Inlines.Add(new Run("Patient's Age: " + CLIPSController.CurrentPatient.getAge().ToString()));
                fdPrint.Blocks.Add(p);
            }
            else
            {
                p = new Paragraph();
                p.FontSize = 15;
                p.FontWeight = FontWeights.Bold;
                p.Inlines.Add(new Run("Patient's ID: " + CLIPSController.CurrentPatient.NRIC));
                p.Inlines.Add(new Run("Patient's Name: " + CLIPSController.CurrentPatient.Last_Name + CLIPSController.CurrentPatient.First_Name));
                p.Inlines.Add(new Run("Patient's Age: " + CLIPSController.CurrentPatient.getAge().ToString()));

                fdPrint.Blocks.Add(p);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////

            if (CLIPSController.CurrentPatient.getLatestHistory().SymptomsList.Count == 0)
            {
                p = new Paragraph();
                p.FontSize = 15;
                p.FontWeight = FontWeights.Bold;
                p.Inlines.Add(new Run("The evaluation does not suggest dementia in this patient"));
                fdPrint.Blocks.Add(p);
            }
            else
            {
                p = new Paragraph();
                p.FontSize = 15;
                p.FontWeight = FontWeights.Bold;
                p.Inlines.Add(new Run("The patient has the following issues uncovered from the questionnaire: " + Environment.NewLine));
                fdPrint.Blocks.Add(p);

                foreach (Symptom sym in CLIPSController.CurrentPatient.getLatestHistory().SymptomsList)
                {
                    Run symP = new Run("   " + App.bulletForm() + sym.SymptomName + Environment.NewLine);
                    symP.FontWeight = FontWeights.Normal;
                    symP.FontSize = 14;
                    p.Inlines.Add(symP);
                }

                fdPrint.Blocks.Add(p);
            }

            ///////////////////////////////////////////////////////////////
            p = new Paragraph();
            p.FontSize = 15;
            p.FontWeight = FontWeights.Bold;
            p.Inlines.Add(new Run("Recommendations"));
            fdPrint.Blocks.Add(p);

            foreach (Diagnosis diaRule in allDiagnoses)
            {
                Paragraph headerP = new Paragraph();
                headerP.FontSize = 14;
                headerP.FontWeight = FontWeights.Bold;
                headerP.Inlines.Add(new Run(diaRule.Header));

                Run x = new Run(Environment.NewLine + diaRule.Comment.Replace(App.bulletForm(), "   " + App.bulletForm()));
                x.FontWeight = FontWeights.Normal;
                headerP.Inlines.Add(x);

                fdPrint.Blocks.Add(headerP);

            }


            ////////////////////////////////////////////////////////////////
            p = new Paragraph();
            p.FontSize = 12;

            p.Inlines.Add(new Run("Disclaimer: ADD serves primarily as a diagnostic aid, the physician should always exercise clinical judgment with respect to the conclusions and recommendations offered by the system. The developers of ADD shall not be liable for any damages arising from using the application."));
            fdPrint.Blocks.Add(p);


            return fdPrint;
        }
    }
}
