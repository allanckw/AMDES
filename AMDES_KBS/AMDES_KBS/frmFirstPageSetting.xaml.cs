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
            DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void saveFirstSection()
        {
           
            //Update first navigation link
            FirstQuestion fq = new FirstQuestion();
            //fq.GrpID = //please fill in the blank
            FirstQuestionController.writeFirstQuestion(fq);
        }
    }
}
