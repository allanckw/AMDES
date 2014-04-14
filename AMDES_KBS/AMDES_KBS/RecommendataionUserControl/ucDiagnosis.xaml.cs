using System;
using System.Windows;
using System.Windows.Controls;
using AMDES_KBS.Entity;

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
            //addSymptons();
        }

        //public void addSymptons()
        //{
            
        //    foreach (Symptom sym in CLIPSController.CurrentPatient.getLatestHistory().SymptomsList)
        //    {
        //        Label lblSymptons = new Label();
        //        lblSymptons.Content = "Symptoms - " + sym.SymptomName;
        //        stkpnlSymptons.Children.Add(lblSymptons);
        //    }
        //    if (stkpnlSymptons.Children.Count==0)
        //    {
        //        lblSymptonsText.Content = "The patient has no symptoms.";
        //    }
        //    //updateHeight();
        //}

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
            //this.lblLink.Url = new System.Uri(linkString);
            txtLink.Tag = linkString;
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

        private void lblLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(txtLink.Tag.ToString());
            }
            catch { }
            {
                //MessageBox.Show("Please check your internet connection!");
            }
        }

    }
}
