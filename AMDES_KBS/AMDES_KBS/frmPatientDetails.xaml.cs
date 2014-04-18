using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;
using System.Windows.Input;

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
            dtpDOB.SelectedDate = new DateTime(DateTime.Today.Year, 1, 1);
            dtpAss.SelectedDate = DateTime.Today;
            turnOffPatientDetails();
            //loadEnthicGrp();
        }

        //private void loadEnthicGrp()
        //{
        //    // Obtain the string names of all the elements within myEnum 
        //    List<string> enthic = Enum.GetNames(typeof(PatientEthnicGrp)).ToList<String>();

        //    for (int i = 0; i < enthic.Count; i++)
        //    {
        //        string s = enthic[i];
        //        cboEthnicGrp.Items.Add(s);
        //    }
        //    cboEthnicGrp.SelectedIndex = -1;
        //}

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
                try
                {
                    this.Cursor = Cursors.Wait;
                    AssertQuestions();
                    int sectionID = CLIPSController.getCurrentQnGroupID();
                    if (sectionID != -1)
                    {
                        //MessageBox.Show(sectionID.ToString());
                        frmSection TestSection = new frmSection(amdesPageFrame, sectionID);
                        amdesPageFrame.Navigate(TestSection);
                    }
                    else
                    {
                        MessageBox.Show("No Navigation rules has been defined, please seek help from the Expert or Knowledge Engineer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                        
                        p.NRIC = "ANON";

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

        //private void btnStats_Click(object sender, RoutedEventArgs e)
        //{
        //    //This data is based on patient's history that have been extracted
        //    //Just a POC that it works, not saying doctors are racist!!
        //    if (radFemale.IsChecked == false && radMale.IsChecked == false)
        //    {
        //        MessageBox.Show("Please select the patient's gender before viewing statistics");
        //    }
        //    else if (cboEthnicGrp.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Please select the patient's Ethnic Group before viewing statistics");
        //    }
        //    else
        //    {
        //        //CLIPSController.???
        //    }
        //}
    }
}
