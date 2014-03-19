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
    public partial class ucNavigationSetting : UserControl
    {
        bool navigationLink;
        QuestionGroup qgParent;

        public ucNavigationSetting()
        {
            InitializeComponent();
        }

        public void loadData(QuestionGroup qg,Boolean NaviPath)
        {
            qgParent = qg;
            navigationLink = NaviPath;
            loadDistinct(qgParent);
            if (navigationLink)
            {
                if (qg.NextTrueLink != null)
                {
                    loadSelectedGroupData(qg.NextTrueLink);
                }
            }
            else
            {
                if (qg.NextFalseLink != null)
                {
                    loadSelectedGroupData(qg.NextFalseLink);
                }
            }
        }

        public void loadSelectedGroupData(Navigation navi)
        {
            chkConclusive.IsChecked = navi.isConclusive;
            List<QuestionGroup> qgList = (List<QuestionGroup>)lstGroupList.ItemsSource;
            for (int i = 0; i < qgList.Count; i++)
            {
                if (qgList[i].GroupID==navi.DestGrpID)
                {
                    lstGroupList.SelectedIndex = i;
                    break;
                }
            }

            chkRequireAge.IsChecked = navi.isRequireAge;
            radless.IsChecked = navi.LessThanAge;
            radMoreEqual.IsChecked = navi.MoreThanEqualAge;
            txtAge.Text = navi.Age.ToString();
            //if (navi.LessThanAge==true)
            //{
            //    radless.IsChecked = true;
            //}
        }

        public void loadDistinct(QuestionGroup qg)
        {
            List<QuestionGroup> qgList = QuestionController.getAllQuestionGroup();
            for (int i = 0; i < qgList.Count; i++)
            {
                if (qgList[i].GroupID==qg.GroupID)
                {
                    qgList.RemoveAt(i);
                    break;
                }
            }

            lstGroupList.ItemsSource = qgList;
        }

        private void chkConclusive_Checked(object sender, RoutedEventArgs e)
        {
                //stkpnlDetail.IsEnabled = false;
                stkpnlDiagnosis.Visibility = Visibility.Visible;
                stkpnlDiagnosis.IsEnabled = true;
                btnAddDiagnosis.IsEnabled = stkpnlDiagnosis.IsEnabled;
                lstGroupList.IsEnabled = !stkpnlDiagnosis.IsEnabled;
                stkpnlLinkCriteria.Visibility = Visibility.Hidden;
                btnSave.Margin = new Thickness(340, 0, 0, 0);
        }


        private void chkConclusive_Unchecked(object sender, RoutedEventArgs e)
        {
            //stkpnlDetail.IsEnabled = true;
            stkpnlDiagnosis.Visibility = Visibility.Hidden;
            stkpnlDiagnosis.IsEnabled = false;
            btnAddDiagnosis.IsEnabled = stkpnlDiagnosis.IsEnabled;
            lstGroupList.IsEnabled = !stkpnlDiagnosis.IsEnabled;
            stkpnlLinkCriteria.Visibility = Visibility.Visible;
            btnSave.Margin = new Thickness(170, 0, 0, 0);
        }

        private void lstGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sidx = lstGroupList.SelectedIndex;

            if (sidx == -1)
            {
                stkpnlLinkCriteria.Visibility = Visibility.Hidden;
                return;
            }

            stkpnlLinkCriteria.Visibility = Visibility.Visible;
            QuestionGroup qg = (QuestionGroup)lstGroupList.Items.GetItemAt(lstGroupList.SelectedIndex);
            lblHeader.Content = qg.Header;
        }

        private void chkRequireAge_Checked(object sender, RoutedEventArgs e)
        {
            stkpnlAgeSetting.Visibility = Visibility.Visible;
        }


        private void chkRequireAge_Unchecked(object sender, RoutedEventArgs e)
        {
            stkpnlAgeSetting.Visibility = Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (SaveNavi())
            {
                string msgdisplay = "Navigation Link - " + navigationLink.ToString().ToUpper() + " Path Saved!";
                MessageBox.Show(msgdisplay);
            }
            else
            {
                MessageBox.Show("Failed to Save!");
            }
        }

        private bool SaveNavi()
        {
            Navigation navi=new Navigation();

            if (lstGroupList.SelectedIndex == -1 && IsCheckBoxChecked(chkConclusive)==false)
            {
                return false;
            }

            if (!IsDigitsOnly(txtAge.Text.ToString().Trim()))
            {
                return false;
            }

            QuestionGroup qg = (QuestionGroup)lstGroupList.Items.GetItemAt(lstGroupList.SelectedIndex);
            
            navi.isConclusive = IsCheckBoxChecked(chkConclusive);
            navi.DestGrpID = qg.GroupID;
            navi.isRequireAge = IsCheckBoxChecked(chkRequireAge);

            if (navi.isRequireAge)
            {
                if (radless.IsChecked == true)
                {
                    navi.setLessThanAge();
                }

                if (radMoreEqual.IsChecked == true)
                {
                    navi.setMoreThanEqualAge();
                }

                navi.Age = int.Parse(txtAge.Text.Trim());                
            }

            if (navigationLink)
            {
                qgParent.NextTrueLink = navi;
            }

            else
            {
                qgParent.NextFalseLink = navi;
            }

            QuestionController.updateQuestionGroup(qgParent);

            return true;
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

        private void btnAddDiagnosis_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
