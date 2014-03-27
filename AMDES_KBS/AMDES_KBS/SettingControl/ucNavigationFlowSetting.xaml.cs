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
using AMDES_KBS.Controllers;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucNavigationSetting.xaml
    /// </summary>
    public partial class ucNavigationFlowSetting : UserControl
    {
        bool Result=true;

        public ucNavigationFlowSetting()
        {
            InitializeComponent();
            loadDistinct();
            lblCurrStep.Content = "Step " + 1;
        }

        public ucNavigationFlowSetting(int step)
        {
            InitializeComponent();
            loadDistinct();
            lblCurrStep.Content = "Step " + step;
        }

        public void loadDistinct()
        {
            cboGroupList.ItemsSource = QuestionController.getAllQuestionGroup();
        }

        private void chkRequireAge_Checked(object sender, RoutedEventArgs e)
        {
            stkpnlAgeSetting.Visibility = Visibility.Visible;
        }


        private void chkRequireAge_Unchecked(object sender, RoutedEventArgs e)
        {
            stkpnlAgeSetting.Visibility = Visibility.Hidden;
        }

        bool IsCheckBoxChecked(CheckBox c)
        {
            if (c.IsChecked == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void radY_Checked(object sender, RoutedEventArgs e)
        {
            Result = true;
        }

        private void radN_Checked(object sender, RoutedEventArgs e)
        {
            Result = false;
        }

        public void loadCheckedYN()
        {
            radY.IsChecked = Result;
            radN.IsChecked = !Result;
        }
    }
}
