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
    /// Interaction logic for frmFirstPageSetting.xaml
    /// </summary>
    public partial class frmFirstPageSetting : Window
    {
        public frmFirstPageSetting()
        {
            InitializeComponent();
            loadAllSection();
        }

        private void loadAllSection()
        {
            cboSectionList.ItemsSource = QuestionController.getAllQuestionGroup();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            saveFirstSection();

            if (saveFirstSection())
            {
                DialogResult = true;
                this.Close();
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private bool saveFirstSection()
        {

            if (cboSectionList.SelectedIndex >= 0)
            {
                //Update first navigation link
                QuestionGroup qg = (QuestionGroup)cboSectionList.SelectedItem;
                FirstQuestion fq = new FirstQuestion();
                fq.GrpID = qg.GroupID;
                FirstQuestionController.writeFirstQuestion(fq);
                return true;
            }
            else
            {
                MessageBox.Show("Please select the first section group!", "Please make selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
        }

    }
}
