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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucPatientDisplay.xaml
    /// </summary>
    public partial class ucPatientDisplay : UserControl
    {
        Patient pat;
        public ucPatientDisplay(Patient p)
        {
            InitializeComponent();
            loadTest();

            this.pat = p;

            this.txtPatientID.Text = p.NRIC;
            this.txtPatientName.Text = p.Last_Name + " " + p.First_Name;
            this.txtStatus.Text = p.Status.ToString();
            this.txtTestTime.Text = p.AssessmentDate.ToString("dd MMM yyyy");
            this.txtAssessor.Text = p.Doctor.Name;

        }

        private void loadTest()
        {
            if (pat != null)
            {
                for (int i = 0; i < pat.TestsList.Count; i++)
                {
                    ucPatientTest patientTest = new ucPatientTest(pat.TestsList.ElementAt(i));
                    //patientTest.Visibility = Visibility.Collapsed;
                    stkpnlPatientTestList.Children.Add(patientTest);
                }
                stkpnlPatientTestList.Visibility = Visibility.Collapsed;
            }
        }

        private void btnShowHideTest_Click(object sender, RoutedEventArgs e)
        {
            if (btnShowHideTest.Content.ToString().Trim() == "+")
            {
                stkpnlPatientTestList.Visibility = Visibility.Visible;
                btnShowHideTest.Content = "-";
            }
            else
            {
                stkpnlPatientTestList.Visibility = Visibility.Collapsed;
                btnShowHideTest.Content = "+";
            }
        }
    }
}
