using System.IO;
using System.Windows;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;
using System.Collections.Generic;
using System;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmTermCondition.xaml
    /// </summary>
    public partial class frmTermCondition : Window
    {
        private bool initialLoad = true;

        public frmTermCondition()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
            App.LoadCFGs();
                        
            //title = App.readAppTitle();
            this.cboAppContexts.ItemsSource = CLIPSController.AllAppContexts;
            this.cboAppContexts.SelectedIndex = 0;
            initialLoad = false;

            //lblTitle.Content = CLIPSController.selectedAppContext.Name;
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
                admForm.Title = cboAppContexts.SelectedValue.ToString();//lblTitle.Content.ToString();
                
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


        private void cboAppContexts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!initialLoad)
            {
                try
                {
                    ApplicationContext app = (ApplicationContext)cboAppContexts.SelectedItem;
                    ApplicationContextController.setApplicationContext(app);
                    ApplicationContextController.GetAllApplications();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}