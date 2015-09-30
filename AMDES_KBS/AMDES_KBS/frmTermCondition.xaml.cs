using System.IO;
using System.Windows;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;
using System.Collections.Generic;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmTermCondition.xaml
    /// </summary>
    public partial class frmTermCondition : Window
    {
        public frmTermCondition()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
            App.LoadCFGs();
                        
            //title = App.readAppTitle();
            lblTitle.Content = CLIPSController.selectedAppContext.Name;
            txtTnC.AppendText(CLIPSController.selectedAppContext.Description);

            if (!CLIPSController.enableSavePatient)
                chkSavePat.Visibility = Visibility.Hidden;
            else
                chkSavePat.Visibility = Visibility.Visible;

            if (!CLIPSController.secretUser)
                btnAdminCFG.IsEnabled = false;

           
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter your name ", "Missing input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtName.Focus();
            }
            else if (txtLocation.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter your location (clinic name)", "Missing input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtLocation.Focus();
            }
            else
            {
                Assessor a = new Assessor(txtName.Text.Trim(), txtLocation.Text.Trim());
                AssessorController.writeAssessor(a);

                var admForm = new frmMain(chkSavePat.IsChecked);
                this.Visibility = Visibility.Collapsed;
                Application.Current.MainWindow = admForm;
                admForm.Title = lblTitle.Content.ToString();
                
                admForm.Show();
            }
        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Assessor.dataPath))
            {
                Assessor a = AssessorController.readAssessor();
                txtName.Text = a.Name;
                txtLocation.Text = a.ClinicName;
            }

        }

        private void btnAdminCFG_Click(object sender, RoutedEventArgs e)
        {
            if (CLIPSController.secretUser)
            {
                new frmSetting().ShowDialog();
            }
        }

    }
}