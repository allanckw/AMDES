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

            foreach (Control ctrl in stkpnlPatientDetail.Children)
            { 
                if (ctrl.GetType() != typeof(Button))
                    ctrl.Background = brush;
            }

            foreach (Control ctrl in stkpnlPatientTestList.Children)
            {
                if (ctrl.GetType() != typeof(Button))
                    ctrl.Background = brush;
            }

            foreach (Control ctrl in stkpnlPatientTestHeader.Children)
            {
                ctrl.Background = brush;
            }

            this.amdesPageFrame = ParentFrame;
            this.pat = p;

            this.txtPatientID.Text = p.NRIC;
            this.txtPatientName.Text = p.Last_Name + " " + p.First_Name;
            //this.txtStatus.Text = p.Status.ToString();
            this.txtTestTime.Text = p.AssessmentDate.ToString("dd MMM yyyy");
            this.txtAssessor.Text = p.Doctor.Name;

            List<History> hList = p.getAllPatientHistory();

            stkpnlPatientTestHeader.Visibility = Visibility.Collapsed;

            if (hList.Count == 0)
            {
                btnShowHideTest.Visibility = Visibility.Hidden;
                stkpnlPatientTestList.Visibility = Visibility.Collapsed;
            }
            else
            {
                loadTest(hList);
            }

        }

        private void loadTest(List<History> lstHistory)
        {
            if (pat != null)
            {

                for (int i = 0; i < lstHistory.Count; i++)
                {
                    ucPatientTest patientTest = new ucPatientTest(lstHistory[i], pat, amdesPageFrame, brush);
                    patientTest.Tag = i;
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
                stkpnlPatientTestHeader.Visibility = Visibility.Visible;
                btnShowHideTest.Content = "-";
            }
            else
            {
                stkpnlPatientTestList.Visibility = Visibility.Collapsed;
                stkpnlPatientTestHeader.Visibility = Visibility.Collapsed;
                btnShowHideTest.Content = "+";
            }
        }

        private void btnContTest_Click(object sender, RoutedEventArgs e)
        {
            CLIPSController.CurrentPatient = pat;

            if (HistoryController.checkForDraft(pat.NRIC))
            {
                MessageBox.Show("You have an existing draft test, please continue from there", "Draft Test Found", MessageBoxButton.OK, MessageBoxImage.Information);
                stkpnlPatientTestList.Visibility = Visibility.Visible;
                btnShowHideTest.Content = "-";
                return;
            }

            CLIPSController.clearAndLoadNew();
            int sectionID = CLIPSController.getCurrentQnGroupID();

            frmSection TestSection = new frmSection(amdesPageFrame, sectionID);
            amdesPageFrame.Navigate(TestSection);
        }

        private void btnResult_Click(object sender, RoutedEventArgs e)
        {
            CLIPSController.CurrentPatient = pat;
            frmRecommendation frmConclusion = new frmRecommendation(amdesPageFrame, pat.getLatestHistory().Diagnoses, null);
            amdesPageFrame.Navigate(frmConclusion);

        }
    }
}
