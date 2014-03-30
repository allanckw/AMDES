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
            FirstQuestion fq = FirstQuestionController.readFirstQuestion();
            if (fq != null)
            {
                for (int i = 0; i < cboSectionList.Items.Count; i++)
                {
                    if (fq.GrpID == ((QuestionGroup)cboSectionList.Items[i]).GroupID)
                    {
                        cboSectionList.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            bool x = saveFirstSection();

            if (x)
            {
                DialogResult = true;
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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
                FirstQuestion fq = new FirstQuestion();
                fq.GrpID = -1;
                FirstQuestionController.writeFirstQuestion(fq);
                MessageBox.Show("Please select the first section group!", "Please make selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
        }

    }
}
