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
            loadPatientList();
            frameDisplay.Navigate(new frmOverview(frameDisplay));
        }

        public void loadPatientList()
        {
            List<Patient> plist = PatientController.getAllPatients();
            lstPatientList.ItemsSource = plist;
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
            Regex regex = new Regex(@"
                        ^           # anchor at the start
                       (?=.*\d)     # must contain at least one numeric character
                       (?=.*[a-z])  # must contain one lowercase character
                       (?=.*[A-Z])  # must contain one uppercase character
                       .{8,10}      # From 8 to 10 characters in length
                       \s           # allows a space 
                       $            # anchor at the end",
                       RegexOptions.IgnorePatternWhitespace);
            if (regex.IsMatch(criteria))
            {
                Patient p = PatientController.searchPatientByID(criteria);
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
                List<Patient> plist = PatientController.searchPatientByName(criteria);
                patientResultDisplay = new frmOverview(frameDisplay, plist);
            }

            frameDisplay.Navigate(patientResultDisplay);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void lstPatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = lstPatientList.SelectedIndex;
            if (selectedIndex != -1)
            {
                Patient p = (Patient)lstPatientList.Items.GetItemAt(selectedIndex);
                frmOverview patientView = new frmOverview(frameDisplay, p);
                frameDisplay.Navigate(patientView);
            }

        }
    }
}
