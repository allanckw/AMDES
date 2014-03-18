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
using System.Windows.Shapes;
using AMDES_KBS.Controllers;
using System.Text.RegularExpressions;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmMain.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        //private Type currPageType;
        //private AMDESPage currPage;

        public frmMain()
        {
            InitializeComponent();
            //frmSection newSection = new frmSection("A");
            //frameDisplay.Navigate(new frmRecommendation());
            frameDisplay.Navigate(new frmOverview(frameDisplay));
        }

        private void btnNewTest_Click(object sender, RoutedEventArgs e)
        {
            frameDisplay.Navigate(new frmPatientDetails(frameDisplay));
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            frmSetting SettingForm = new frmSetting();
            SettingForm.ShowDialog();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmOverview patientResultDisplay;
            string criteria = txtSearchCriteria.Text.Trim();
            if (Regex.IsMatch(criteria, @"^[a-zA-Z0-9]+$"))
            {
                Patient p = PatientController.searchPatientByNRIC(criteria);
                if (p != null)
                {
                    patientResultDisplay = new frmOverview(frameDisplay, p);
                }
                else
                {
                    MessageBox.Show("No such record! Returning all patients records...", " No record found", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    patientResultDisplay = new frmOverview(frameDisplay);
                }
            }
            else
            {
                List<Patient> plist = PatientController.searchPatientByName();
                patientResultDisplay = new frmOverview(frameDisplay, plist);
            }

            frameDisplay.Navigate(patientResultDisplay);
        }
    }
}
