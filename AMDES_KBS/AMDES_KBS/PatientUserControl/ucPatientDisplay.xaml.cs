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
    /// Interaction logic for ucPatientDisplay.xaml
    /// </summary>
    public partial class ucPatientDisplay : UserControl
    {
        Patient pat;
        Frame amdesPageFrame;
        SolidColorBrush brush;
        public ucPatientDisplay(Patient p, Frame ParentFrame, Color c)
        {
            InitializeComponent();
            brush = new SolidColorBrush(c);

            this.gridPatient.Background = brush;
            txtAssessor.Background = brush;
            txtPatientID.Background = brush;
            txtPatientName.Background = brush;
            txtStatus.Background = brush;
            txtTestTime.Background = brush;
               

            this.amdesPageFrame = ParentFrame;
            this.pat = p;

            this.txtPatientID.Text = p.NRIC;
            this.txtPatientName.Text = p.Last_Name + " " + p.First_Name;
            //this.txtStatus.Text = p.Status.ToString();
            this.txtTestTime.Text = p.AssessmentDate.ToString("dd MMM yyyy");
            this.txtAssessor.Text = p.Doctor.Name;

            List<History> hList = p.getAllPatientHistory();

            if (hList.Count == 0)
                btnShowHideTest.Visibility = Visibility.Hidden;
            else
                loadTest(hList);

            //if (p.Status == PatientStatus.COMPLETED)
            //{
            //    btnResult.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    btnResult.Visibility = Visibility.Collapsed;
            //}
        }

        private void loadTest(List<History> lstHistory)
        {
            if (pat != null)
            {
              
                for (int i = 0; i < lstHistory.Count; i++)
                {
                    ucPatientTest patientTest = new ucPatientTest(lstHistory[i],pat,amdesPageFrame ,brush);
                    //patientTest.Visibility = Visibility.Collapsed;
                    stkpnlPatientTestList.Children.Add(patientTest);
                }
                stkpnlPatientTestList.Visibility = Visibility.Collapsed;
            }
        }

        private void btnShowHideTest_Click(object sender, RoutedEventArgs e)
        {
            if (btnShowHideTest.Content.ToString().Trim() == "+")
            {
                stkpnlPatientTestList.Visibility = Visibility.Visible;
                btnShowHideTest.Content = "-";
            }
            else
            {
                stkpnlPatientTestList.Visibility = Visibility.Collapsed;
                btnShowHideTest.Content = "+";
            }
        }

        private void btnContTest_Click(object sender, RoutedEventArgs e)
        {
            CLIPSController.CurrentPatient = pat;
            CLIPSController.clearAndLoadNew();
            //Allan ToDo: load history here
           
            //@Kai history here.. use this to populate back 
            //TODO: Use Assert Log to assert back previous user choices to Clips
            //History h = CLIPSController.loadSavedAssertions();
            
            //if (h != null)
            //{
            //    //load back here
            //}

            int sectionID = CLIPSController.getCurrentQnGroupID();
            //MessageBox.Show(sectionID.ToString()); - DONE

            frmSection TestSection = new frmSection(amdesPageFrame, sectionID);
            amdesPageFrame.Navigate(TestSection);
        }

        private void btnResult_Click(object sender, RoutedEventArgs e)
        {
            //KAI Result button no longer here, please remove, move to test section
            //new button will either start a new test or continue from a saved test depending on whether there is a history or not
            //I cant move status to history yet cos your controls are using , please remove all references before i can continue
            CLIPSController.CurrentPatient = pat;

            frmRecommendation frmConclusion = new frmRecommendation(amdesPageFrame, pat.getLatestHistory().Diagnoses, null);
            amdesPageFrame.Navigate(frmConclusion);

        }
    }
}
