using System.IO;
using System.Windows;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

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

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            if (!Directory.Exists(@"Data\Logs"))
            {
                Directory.CreateDirectory(@"Data\Logs");
            }

            if (File.Exists(Assessor.dataPath))
            {
                Assessor a = AssessorController.readAssessor();
                txtName.Text = a.Name;
                txtLocation.Text = a.ClinicName;
            }

            CLIPSController.ExpertUser = File.Exists(@"Data\e.miao");
            CLIPSController.enablePrev = File.Exists(@"Data\e.prev");
            CLIPSController.enableSavePatient = File.Exists(@"Data\e.spat");
            CLIPSController.enableStats = File.Exists(@"Data\e.stats");

            if (!CLIPSController.enableSavePatient)
                chkSavePat.Visibility = Visibility.Hidden;
            else
                chkSavePat.Visibility = Visibility.Visible;
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
                admForm.Show();
            }
        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
 
        }
    }
}