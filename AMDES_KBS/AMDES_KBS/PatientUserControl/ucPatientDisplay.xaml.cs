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

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucPatientDisplay.xaml
    /// </summary>
    public partial class ucPatientDisplay : UserControl
    {
        public ucPatientDisplay()
        {
            InitializeComponent();
            loadTest();
        }

        private void loadTest()
        {
            for (int i = 0; i < 5; i++)
			{
                ucPatientTest patientTest = new ucPatientTest();
                //patientTest.Visibility = Visibility.Collapsed;
                stkpnlPatientTestList.Children.Add(patientTest);
			}
            stkpnlPatientTestList.Visibility = Visibility.Collapsed;
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
