using System;
using System.Windows;
using System.Windows.Controls;
using AMDES_KBS.Entity;
using System.Collections.Generic;
using AMDES_KBS.Controllers;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucDiagnosis.xaml
    /// </summary>
    public partial class ucDiagnosisResource : UserControl
    {
        Diagnosis DiaRule;
        public ucDiagnosisResource(Diagnosis Rule)
        {
            InitializeComponent();
            DiaRule = Rule;
            lblRuleID.Content = DiaRule.Header;
            loadComment();
            loadLink();
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
            //this.lblLink.Url = new System.Uri(linkString);
            hlDesc.Text = DiaRule.LinkDesc;
            txtLink.Tag = linkString;
            stkpnlDiagnosisLink.Visibility = Visibility.Visible;
            //string comment = "Likelihood of dementia is low. If the cognitive deficit(s) has been present  for a long time, do consider congenital conditions like mental retardation, cerebral palsy. " +
            //    Environment.NewLine +
            //    "You may consider a referral to a specialist should there be a sudden change or decline in the condition.";
            //this.txtDiagnosisMessage.Text = DiaRule.Comment.Replace("~~", Environment.NewLine);

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
