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
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmPatientDetails.xaml
    /// </summary>
    public partial class frmPatientDetails : AMDESPage
    {
        Frame amdesPageFrame;
        Assessor a;
        public frmPatientDetails(Frame amdesFrame)
        {
            InitializeComponent();
            amdesPageFrame = amdesFrame;

            a = AssessorController.readAssessor();
            txtAssessorName.Text = a.Name;
            txtAssessorLoc.Text = a.ClinicName;
            dtpAss.SelectedDate = DateTime.Today;

        }

        private void btnStartTest_Click(object sender, RoutedEventArgs e)
        {
            if (savePatient())
            {
                AssertQuestions();
                frmSection TestSection = new frmSection(amdesPageFrame, 1);
                amdesPageFrame.Navigate(TestSection);
            }
        }

        private bool savePatient()
        {
            if (txtNRIC.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Patient's Identification Number!", "Missing NRIC", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtNRIC.Focus();
                return false;
            }
            else if (txtLastName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Patient's Surname / Last Name!", "Missing Surname / First Name", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtLastName.Focus();
                return false;
            }
            else if (txtFirstName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please Enter Patient's Given name / First Name!", "Missing Given name / Last Name", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtLastName.Focus();
                return false;
            }

            else if (dtpDOB.SelectedDate == null)
            {
                MessageBox.Show("Please Enter Patient's Date of birth!", "Missing Date of birth", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return false;
            }
            else
            {
                Patient p = new Patient(a, txtNRIC.Text.Trim(), txtFirstName.Text.Trim(),
                                    txtLastName.Text.Trim(), (DateTime)dtpDOB.SelectedDate);

                PatientController.addPatient(p);
                //Dim myWindow As Window1 = TryCast(Application.Current.MainWindow, Window1)
                frmMain myWindow = (frmMain)Application.Current.MainWindow;
                myWindow.loadPatientList();
                CLIPSController.CurrentPatient = p;
                return true;
            }


        }

        private void btnSaveTest_Click(object sender, RoutedEventArgs e)
        {
            if (savePatient())
            {
                amdesPageFrame.Navigate(new frmOverview(amdesPageFrame));

            }
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            amdesPageFrame.Navigate(new frmOverview(amdesPageFrame));
        }

        private void AssertQuestions()
        {
            CLIPSController.loadQuestions();

            //str2assert = str2assert + ")";
            //_theEnv.AssertString(str2assert);
        }
    }
}
