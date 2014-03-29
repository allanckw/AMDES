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
           // Rules r = new Rules();

           // List<Rules> rList = NavigationController.getAllRules();
           // r.RuleID = 1;

           // r.Description = "nat i am sorry";

           // Navigation n1 = new Navigation();
           // Navigation n2 = new Navigation();

           // n2.DestGrpID = 2;

           // NaviChildCritAttribute a1 = new NaviChildCritAttribute();
           // a1.AttributeName = "a";
           // a1.AttributeValue = "1";
           // a1.Ans = false;
           // a1.setRuleType(1);

           // NaviChildCritAttribute a2 = new NaviChildCritAttribute();
           // a2.AttributeName = "b";
           // a2.AttributeValue = "2";
           // a2.Ans = false;
           // a2.setRuleType(3);

           // n1.addNavCriteriaAttribute(a1);
           // n1.addNavCriteriaAttribute(a2);

           // n2.addNavCriteriaAttribute(a1);
           // n2.addNavCriteriaAttribute(a2);

           // NaviChildCriteriaQuestion nq1 = new NaviChildCriteriaQuestion();
           // nq1.CriteriaGrpID = "1";
           // nq1.Ans = true;


           // NaviChildCriteriaQuestion nq2 = new NaviChildCriteriaQuestion();
           // nq2.CriteriaGrpID = "2";
           // nq2.Ans = false;

           // n1.addNavCriteriaQuestion(nq1);
           // n1.addNavCriteriaQuestion(nq2);
           // n2.addNavCriteriaQuestion(nq1);
           // n2.addNavCriteriaQuestion(nq2);

           // r.Navigations.Add(n1);
           // r.Navigations.Add(n2);


           // //n.DestGrpID = "-";
           // n1.addDiagnosisID(1);
           // n1.addDiagnosisID(2);
           // n1.addDiagnosisID(3);
           //// r.Navigations.Add(n1);


            //NavigationController.updateRules(r);

            FirstQuestion fq = new FirstQuestion();
            //fq.NextGrpID = 1;
            //fq.GrpID = 2;

            //FirstQuestionController.writeFirstQuestion(fq);
              fq = FirstQuestionController.readFirstQuestion();
              Console.WriteLine();
        }
    }
}