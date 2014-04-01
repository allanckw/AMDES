using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucDiagnosis.xaml
    /// </summary>
    public partial class ucDiagnosis : UserControl
    {
        Diagnosis DiaRule;
        public ucDiagnosis(Diagnosis Rule)
        {
            InitializeComponent();
            DiaRule = Rule;
            lblRuleID.Content = DiaRule.Header;
            loadComment();
            loadLink();
            addSymptons();
        }

        public void addSymptons()
        {
            
            foreach (Symptom sym in CLIPSController.CurrentPatient.SymptomsList)
            {
                Label lblSymptons = new Label();
                lblSymptons.Content = "Symptoms - " + sym.SymptomName;
                stkpnlSymptons.Children.Add(lblSymptons);
            }
            if (stkpnlSymptons.Children.Count==0)
            {
                lblSymptonsText.Content = "The patient has no symptoms.";
            }
            //updateHeight();
        }

        private void loadComment()
        {
            this.txtDiagnosisMessage.Text = DiaRule.Comment.Replace("~~", Environment.NewLine);
        }

        private void loadLink()
        {
            string linkString = DiaRule.Link;
            if (linkString.Length == 0)
            {
                return;
            }
            //http://www.blagoev.com/blog/post/building-a-wpf-linklabel-control.aspx
            this.lblLink.Url = new System.Uri(linkString);
            stkpnlDiagnosisLink.Visibility = Visibility.Visible;
            //string comment = "Likelihood of dementia is low. If the cognitive deficit(s) has been present  for a long time, do consider congenital conditions like mental retardation, cerebral palsy. " +
            //    Environment.NewLine +
            //    "You may consider a referral to a specialist should there be a sudden change or decline in the condition.";
            //this.txtDiagnosisMessage.Text = DiaRule.Comment.Replace("~~", Environment.NewLine);

        }

        public void setVisibility(Visibility v)
        {
            this.gridDiagnosis.Visibility = v;
        }

    }
}
