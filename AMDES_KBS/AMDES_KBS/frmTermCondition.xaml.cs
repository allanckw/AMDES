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
            }
        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //QuestionController.getAllQuestionGroup();

            //QuestionGroup miao = new QuestionGroup();
            //miao.GroupID = QuestionController.getNextGroupID();
            //miao.Header = "Amensia";
            //miao.addQuestion("Will you forgive me? ");
            //miao.addQuestion("please?");
            //miao.addQuestion("I will do anything for u :(");
            //miao.Symptom = "GG";

            //QuestionController.updateQuestionGroup(miao);

            //PatientController.addNewPatient(new Assessor("sad", "emose"), "s2345436q", "noob", "bie", DateTime.Now.AddYears(-30));

            //Patient p = PatientController.searchPatientByNRIC("s2345906z");

            //List<Patient> pats = PatientController.searchPatientByName("natalie", "tay");

            //MessageBox.Show(pats.Count.ToString());

            
        }
    }
}