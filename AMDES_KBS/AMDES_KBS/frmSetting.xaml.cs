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
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmSetting.xaml
    /// </summary>
    public partial class frmSetting : Window
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {
            SettingFrame.Navigate(new frmQuestionSetting());
        }

        private void btnExitSetting_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnNavigation_Click(object sender, RoutedEventArgs e)
        {
            //SettingFrame.Navigate(new frmNavigation());
            new frmFlowToDiagnosis().ShowDialog();
        }

        private void btnDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            SettingFrame.Navigate(new frmDiagnosisSetting());
        }

        private void btnStartPage_Click(object sender, RoutedEventArgs e)
        {
            new frmFirstPageSetting().ShowDialog();
        }
    }
}
