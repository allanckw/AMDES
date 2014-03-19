using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;

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

            if (File.Exists(Assessor.dataPath))
            {
                Assessor a = AssessorController.readAssessor();
                txtName.Text = a.Name;
                txtLocation.Text = a.ClinicName;
            }
            else
            {
                txtName.Clear();
                txtLocation.Clear();
                txtName.Focus();
            }
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

                var admForm = new frmMain();
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
            //List<QuestionGroup> x = QuestionController.getAllQuestionGroup();

            //QuestionGroup miao = new QuestionGroup();
            //miao.GroupID = QuestionController.getNextGroupID();
            //miao.Header = "Amensia";
            //miao.addQuestion("Will you forgive me? ", "GG");
            //miao.addQuestion("please?", "XX");
            //miao.addQuestion("I will do anything for u :(", "ZZ");

            //Navigation nav = new Navigation();
            //nav.isConclusive = true;
            ////nav.DestGrpID = 2;
            //nav.isRequireAge = true;
            //nav.setMoreThanEqualAge();

            //Diagnosis d = new Diagnosis();
            //d.RID = 1;
            //d.Comment = "GG liao la, going to die soon :(((";

            //Diagnosis d2 = new Diagnosis();
            //d.RID = 2;
            //d.Comment = "Maybe can try noobiniser";

            //nav.addDiagnosis(d);
            //nav.addDiagnosis(d2);

            //miao.NextFalseLink = nav;

            //Navigation nav1 = new Navigation();
            //nav1.isConclusive = false;
            //nav1.DestGrpID = 2;
            //nav1.isRequireAge = true;
            //nav1.setMoreThanEqualAge();


            //miao.NextTrueLink = nav1;

            //QuestionController.updateQuestionGroup(miao);

            //PatientController.addNewPatient(new Assessor("sad", "emose"), "s2345436q", "noob", "bie", DateTime.Now.AddYears(-30));

            //Patient p = PatientController.searchPatientByNRIC("s2345906z");

            //PatientController.updatePatientStatus("S8747645Z", PatientStatus.COMPLETED);

            //List<Patient> pats = PatientController.searchPatientByName("natalie");

            //MessageBox.Show(pats.Count.ToString());

            //Diagnosis d = new Diagnosis();
            //d.RID = 1;
            //d.Comment = "GG liao la, going to die soon :(((";
            //d.Link = "www.google.com.sg";
            //d.Header = "bleah";

            //Diagnosis d2 = new Diagnosis();
            //d.RID = 2;
            //d.Comment = "Maybe can try noobiniser";

            //DiagnosisController.updateDiagnosis(d);
            //DiagnosisController.updateDiagnosis(d2);
            
        }
    }
}