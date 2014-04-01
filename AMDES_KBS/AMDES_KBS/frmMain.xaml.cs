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
            frameDisplay.Navigate(new frmOverview(frameDisplay));
        }

        public frmMain(bool? savePatient)
        {
            CLIPSController.savePatient = savePatient;
            InitializeComponent();
            if (CLIPSController.savePatient == false)
            {
                //frameDisplay.Navigate(new frmOverview(frameDisplay));
                frameDisplay.Navigate(new frmPatientDetails(frameDisplay));
                btnPatients.Visibility = Visibility.Hidden;
                stkpnlSearchBox.Visibility = Visibility.Hidden;
            }
            else
            {
                frameDisplay.Navigate(new frmOverview(frameDisplay));
                btnPatients.Visibility = Visibility.Visible;
                stkpnlSearchBox.Visibility = Visibility.Visible;
            }
        }

        private void listPatients()
        {
            frameDisplay.Navigate(new frmOverview(frameDisplay));
        }

        private void btnNewTest_Click(object sender, RoutedEventArgs e)
        {
            frameDisplay.Navigate(new frmPatientDetails(frameDisplay));
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult result =
            //    MessageBox.Show("This configurations is for expert users only for the setting up of AMDES, "
            //                    + Environment.NewLine +
            //                    "Changing the settings (any one of the them) will cause all existing data to be archived and the system may fail if it is incorrectly configured"
            //                    + Environment.NewLine +
            //                    "Are you sure you want to continue??? ", "Confirmation of entering configuration form",
            //                    MessageBoxButton.YesNo, MessageBoxImage.Question);

            // if (result == MessageBoxResult.Yes)
            // {
            frmSetting SettingForm = new frmSetting();
            SettingForm.ShowDialog();
            //}
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmOverview patientResultDisplay;
            string criteria = txtSearchCriteria.Text.Trim();
            List<Patient> plist = PatientController.searchPatient(criteria);
            patientResultDisplay = new frmOverview(frameDisplay, plist);
            frameDisplay.Navigate(patientResultDisplay);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnPatients_Click(object sender, RoutedEventArgs e)
        {
            listPatients();
        }

    }
}
