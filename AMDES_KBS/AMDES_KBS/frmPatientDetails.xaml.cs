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
            dtpDOB.SelectedDate = dtpAss.SelectedDate = DateTime.Today;
            turnOffPatientDetails();
            loadEnthicGrp();
        }

        private void loadEnthicGrp()
        {
            // Obtain the string names of all the elements within myEnum 
            List<string> enthic = Enum.GetNames(typeof(PatientEthnicGrp)).ToList<String>();

            for (int i = 0; i < enthic.Count; i++)
            {
                string s = enthic[i];
                cboEthnicGrp.Items.Add(s);
            }
            cboEthnicGrp.SelectedIndex = 0;
        }

        private void turnOffPatientDetails()
        {
            if (CLIPSController.savePatient == false)
            {
                stkfirstname.Visibility = Visibility.Collapsed;
                stknric.Visibility = Visibility.Collapsed;
                stksurname.Visibility = Visibility.Collapsed;
                btnQuit.Visibility = Visibility.Collapsed;
                btnSaveTest.Visibility = Visibility.Collapsed;
            }
            else
            {
                stkfirstname.Visibility = Visibility.Visible;
                stknric.Visibility = Visibility.Visible;
                stksurname.Visibility = Visibility.Visible;
                btnQuit.Visibility = Visibility.Visible;
                btnSaveTest.Visibility = Visibility.Visible;
            }
        }

        private void btnStartTest_Click(object sender, RoutedEventArgs e)
        {
            if (savePatient())
            {
                AssertQuestions();
                int sectionID = CLIPSController.getCurrentQnGroupID();

                //MessageBox.Show(sectionID.ToString());

                frmSection TestSection = new frmSection(amdesPageFrame, sectionID);
                amdesPageFrame.Navigate(TestSection);
            }
        }

        private bool savePatient()
        {
            try
            {
                if (CLIPSController.savePatient == true)
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

                        PatientController.updatePatient(p);

                        CLIPSController.CurrentPatient = p;

                        return true;
                    }
                }

                else
                {
                    if (dtpDOB.SelectedDate == null)
                    {
                        MessageBox.Show("Please Enter Patient's Date of birth!", "Missing Date of birth", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return false;
                    }
                    else
                    {
                        Patient p = new Patient(a, (DateTime)dtpDOB.SelectedDate);
                        if (radMale.IsChecked == true)
                            p.Gender = PatientGender.MALE;
                        else
                            p.Gender = PatientGender.FEMALE;

                        p.EthnicGroup = (PatientEthnicGrp)cboEthnicGrp.SelectedIndex;
                        CLIPSController.CurrentPatient = p;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
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
            CLIPSController.clearAndLoadNew();
        }
    }
}
