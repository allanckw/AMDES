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
        bool Result = true;
        int ageCompareResult = 0;

        bool isConclusive = false;
        NaviChildCriteriaQuestion naviChildCriteriaGroup;
        List<NaviChildCritAttribute> naviChildAttrGroupList;

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

        public ucNavigationFlowSetting(int step, Navigation navi)
        {
            InitializeComponent();
            loadDistinct();
            lblCurrStep.Content = "Step " + step;
            loadAnswer(step, navi);
        }

        public void setGroupBox(int grpID)
        {
            QuestionGroup qg = QuestionController.getGroupByID(grpID);
            for (int i = 0; i < cboGroupList.Items.Count; i++)
            {
                QuestionGroup g = (QuestionGroup)cboGroupList.Items[i];
                if (g.Equals(qg))
                {
                    cboGroupList.SelectedIndex = i;
                    break;
                }
            }
        }

        public void getAnswer()
        {
            naviChildCriteriaGroup = new NaviChildCriteriaQuestion();
            naviChildAttrGroupList = new List<NaviChildCritAttribute>();

            int selectedIdx = cboGroupList.SelectedIndex;
            int currGrpID = ((QuestionGroup)cboGroupList.Items[selectedIdx]).GroupID;
            naviChildCriteriaGroup.CriteriaGrpID = currGrpID;
            naviChildCriteriaGroup.Ans = Result;

            bool ageExists = false;
            foreach (NaviChildCritAttribute attr in naviChildAttrGroupList)
            {
                switch (attr.AttributeName)
                {
                    case "AGE":
                        {
                            ageExists = true;
                            attr.GroupID = currGrpID;
                            attr.AttributeValue = txtAge.Text;
                            //attr.Ans = ageResult;
                            if (radMoreEqual.IsChecked == true)
                            {
                                attr.setRuleType((int)NaviChildCritAttribute.AttributeCmpType.MoreThan);
                            }
                            else
                            {
                                attr.setRuleType((int)NaviChildCritAttribute.AttributeCmpType.LessThanEqual);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            if (!ageExists && this.chkRequireAge.IsChecked == true)
            {
                NaviChildCritAttribute ageAttr = new NaviChildCritAttribute();
                //ageExists = true;
                ageAttr.GroupID = currGrpID;
                ageAttr.AttributeValue = txtAge.Text;
                ageAttr.AttributeName = "AGE";
                //ageAttr.Ans = ageResult;
                if (radMoreEqual.IsChecked == true)
                {
                    ageAttr.setRuleType((int)NaviChildCritAttribute.AttributeCmpType.MoreThan);
                }
                else
                {
                    ageAttr.setRuleType((int)NaviChildCritAttribute.AttributeCmpType.LessThanEqual);
                }
                naviChildAttrGroupList.Add(ageAttr);
            }
        }

        public NaviChildCriteriaQuestion getCriteria()
        {
            return naviChildCriteriaGroup;
        }

        public List<NaviChildCritAttribute> getAttrList()
        {
            return naviChildAttrGroupList;
        }

        public int getGroupID()
        {
            int selectedIdx = cboGroupList.SelectedIndex;

            if (selectedIdx == -1)
            {
                return -1;
            }

            int currGrpID = ((QuestionGroup)cboGroupList.Items[selectedIdx]).GroupID;
            return currGrpID;
        }

        public void loadAnswer(int step, Navigation navi)
        {
            naviChildCriteriaGroup = navi.ChildCriteriaQuestion[step - 1];
            naviChildAttrGroupList = new List<NaviChildCritAttribute>();
            int grpID = naviChildCriteriaGroup.CriteriaGrpID;
            //int destGRP = navi.DestGrpID;
            int selectedIdx = -1;
            for (int i = 0; i < cboGroupList.Items.Count; i++)
            {
                QuestionGroup qg = (QuestionGroup)cboGroupList.Items[i];
                if (qg.GroupID == grpID)
                {
                    selectedIdx = i;
                    break;
                }
            }

            cboGroupList.SelectedIndex = selectedIdx;
            Result = naviChildCriteriaGroup.Ans;

            if (navi.DestGrpID == -1)
            {
                isConclusive = true;
            }
            else
            {
                isConclusive = false;
            }

            List<NaviChildCritAttribute> navigroupList = new List<NaviChildCritAttribute>();
            foreach (NaviChildCritAttribute naviAttr in navi.ChildCriteriaAttributes)
            {
                if (naviAttr.GroupID == grpID)
                {
                    //navigroupList.Add(naviAttr);
                    naviChildAttrGroupList.Add(naviAttr);
                    string attributeName = naviAttr.AttributeName;
                    switch (attributeName)
                    {
                        case "AGE":
                            {
                                chkRequireAge.IsChecked = true;
                                txtAge.Text = naviAttr.AttributeValue;
                                //ageResult = naviAttr.Ans;
                                if (naviAttr.getAttributeTypeENUM() == NaviChildCritAttribute.AttributeCmpType.MoreThan)
                                {
                                    radMoreEqual.IsChecked = true;
                                    ageCompareResult = 1;
                                }
                                else
                                {
                                    this.radlessThaneqal.IsChecked = true;
                                    ageCompareResult = 0;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

           
            loadCheckedAgeMoreOrLess();
            loadCheckedYN();
            //loadCheckedAgeTF();
            //loadCheckedAgeTF();
            //loadCheckedAgeMoreOrLess();
            //loadCheckedYN();
            //loadIsConclusive();
            //NaviChildCritAttribute.AttributeCmpType.MoreThanEqual;
            //if (naviChildAttribute.AttributeNameCMP)
            //{

            //}
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

        public void loadCheckedAgeMoreOrLess()
        {
            if (ageCompareResult == 0)
            {
                this.radlessThaneqal.IsChecked = true;
            }
            else
            {
                radMoreEqual.IsChecked = true;
            }
        }

        public void loadIsConclusive()
        {
            chkConclusive.IsChecked = isConclusive;
        }

        private void radless_Checked(object sender, RoutedEventArgs e)
        {
            ageCompareResult = 0;
        }

        private void radMoreEqual_Checked(object sender, RoutedEventArgs e)
        {
            ageCompareResult = 1;
        }

        //private void radAgeTrue_Checked(object sender, RoutedEventArgs e)
        //{
        //    ageResult = true;
        //}

        //private void radAgeFalse_Checked(object sender, RoutedEventArgs e)
        //{
        //    ageResult = false;
        //}
    }
}
