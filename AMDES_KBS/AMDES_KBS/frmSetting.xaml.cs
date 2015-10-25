using System;
using System.Windows;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmSetting.xaml
    /// </summary>
    public partial class frmSetting : Window
    {
        private bool initialLoad = true;

        public frmSetting()
        {
            InitializeComponent();
            this.cboAppContexts.ItemsSource = CLIPSController.AllAppContexts;
            this.cboAppContexts.SelectedIndex = 0;
            initialLoad = false;
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
            if (fq == null)
            {
                //MessageBox.Show("Please define first setting!");
                frmFirstPageSetting firstPageSetting = new frmFirstPageSetting();
                if (firstPageSetting.DialogResult == false)
                {
                    //new frmFlowToDiagnosis().ShowDialog();
                    MessageBox.Show("Please define the first section", "No first section found", MessageBoxButton.OK,MessageBoxImage.Information);
                    return;
                }
            }
            try
            {
                new frmFlowToDiagnosis().ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                new frmFlowToDiagnosis().ShowDialog();
            }

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
                    MessageBox.Show("Please define the first section", "No first section found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            new frmDefaultBehaviour().ShowDialog();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            //must have java install
            try
            {//java -jar "amdesrulesgen.jar" where.txt
                String path = "GenerateRulesFromHistory.bat";
                System.Diagnostics.Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAttribute_Click(object sender, RoutedEventArgs e)
        {
            SettingFrame.Navigate(new frmAttributeSetting());
        }

        private void btnEngineFile_Click(object sender, RoutedEventArgs e)
        {
            new frmEngineFile().ShowDialog();
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
