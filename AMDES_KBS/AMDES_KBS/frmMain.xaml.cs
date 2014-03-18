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
            frameDisplay.Navigate(new frmOverview(frameDisplay));
        }

        private void btnNewTest_Click(object sender, RoutedEventArgs e)
        {
            frameDisplay.Navigate(new frmPatientDetails(frameDisplay));
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            frmSetting SettingForm = new frmSetting();
            SettingForm.ShowDialog();
        }
    }
}
