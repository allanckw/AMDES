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
            FirstQuestion fq = FirstQuestionController.readFirstQuestion();
            if (fq==null)
            {
                //MessageBox.Show("Please define first setting!");
                frmFirstPageSetting firstPageSetting = new frmFirstPageSetting();
                if (firstPageSetting.DialogResult==false)
                {
                    //new frmFlowToDiagnosis().ShowDialog();
                    MessageBox.Show("Please define first setting!");
                    return;
                }
            }

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

        private void btnDefaultBehaviour_Click(object sender, RoutedEventArgs e)
        {
            FirstQuestion fq = FirstQuestionController.readFirstQuestion();
            if (fq == null)
            {
                //MessageBox.Show("Please define first setting!");
                frmFirstPageSetting firstPageSetting = new frmFirstPageSetting();
                if (firstPageSetting.DialogResult == false)
                {
                    //new frmFlowToDiagnosis().ShowDialog();
                    MessageBox.Show("Please define first setting!");
                    return;
                }
            }
            new frmDefaultBehaviour().ShowDialog();
        }
    }
}
