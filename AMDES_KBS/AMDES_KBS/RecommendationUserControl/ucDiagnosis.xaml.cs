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
            if (DiaRule.RetrieveSym)
            {
                foreach (int groupId in DiaRule.RetrievalIDList)
                {
                    List<String> SymptomsListOfQuestionGroup = getSymptomsFromQuestionGroup(groupId);
                    foreach (String symptoms in SymptomsListOfQuestionGroup)
                    {
                        Label lblSymptons = new Label();
                        lblSymptons.FontSize = 14;
                        lblSymptons.Content = App.bulletForm() + symptoms;
                    }
                }
            }

        }

        private List<String> getSymptomsFromQuestionGroup(int groupID)
        {
            List<String> SymptomsList = new List<String>();

            QuestionGroup qg = QuestionController.getGroupByID(groupID);
            if (qg.Symptom.Trim() != "")
            {
                SymptomsList.Add(qg.Symptom.Trim());
            }

            foreach (Question q in qg.Questions)
            {
                String symptom = q.Symptom.Trim();
                if (symptom.Length > 0)
                {
                    if (!SymptomsList.Contains(symptom))
                    {
                        SymptomsList.Add(symptom);
                    }
                }
            }

            return SymptomsList;
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
