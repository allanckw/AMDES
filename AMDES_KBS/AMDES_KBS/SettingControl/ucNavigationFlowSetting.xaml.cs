using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;
using System;

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
            //naviChildAttrGroupList = new List<NaviChildCritAttribute>();
            int selectedIdx = cboGroupList.SelectedIndex;
            int currGrpID = ((QuestionGroup)cboGroupList.Items[selectedIdx]).GroupID;
            naviChildCriteriaGroup.CriteriaGrpID = currGrpID;
            naviChildCriteriaGroup.Ans = Result;
            if (chkOtherAttr.IsChecked==true)
            {
                foreach (NaviChildCritAttribute attr in naviChildAttrGroupList)
                {
                    attr.GroupID = currGrpID;
                }
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
                }
            }

            if (naviChildAttrGroupList.Count > 0)
            {
                chkOtherAttr.IsChecked = true;
                reloadAttribute(naviChildAttrGroupList);
            }
            else
            {
                chkOtherAttr.IsChecked = false;
            }
            //loadCheckedAgeTF();
            //loadCheckedAgeTF();
            //loadCheckedAgeMoreOrLess();
            //loadCheckedYN();
            //loadIsConclusive();
            //AttributeCmpType.MoreThanEqual;
            //if (naviChildAttribute.AttributeNameCMP)
            //{

            //}
        }

        private void reloadAttribute(List<NaviChildCritAttribute> otherAttr)
        {
            lstAttributeList.Items.Clear();
            foreach (NaviChildCritAttribute attr in otherAttr)
            {
                string s = "";
                if (attr.getAttributeTypeENUM() == AttributeCmpType.LessThanEqual)
                {
                    s = "<=";
                }
                else if (attr.getAttributeTypeENUM() == AttributeCmpType.LessThan)
                {
                    s = "<";
                }
                else if (attr.getAttributeTypeENUM() == AttributeCmpType.MoreThanEqual)
                {
                    s = ">=";
                }
                else if (attr.getAttributeTypeENUM() == AttributeCmpType.MoreThan)
                {
                    s = ">";
                }

                if (!App.isAttrCompareNumerical(attr.AttributeName))
                {
                    PatAttribute attrCat = PatAttributeController.searchPatientAttribute(attr.AttributeName);
                    lstAttributeList.Items.Add(attr.AttributeName + " " + s + " " + attrCat.CategoricalVals[int.Parse(attr.AttributeValue)]);
                }
                else
                {
                    lstAttributeList.Items.Add(attr.AttributeName + " " + s + " " + attr.AttributeValue);
                }
            }
        }



        public void loadDistinct()
        {
            cboGroupList.ItemsSource = QuestionController.getAllQuestionGroup();
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

        private void chkOtherAttr_Checked(object sender, RoutedEventArgs e)
        {
            stkpnlOtherAttribute.Visibility = Visibility.Visible;
        }

        private void chkOtherAttr_Unchecked(object sender, RoutedEventArgs e)
        {
            stkpnlOtherAttribute.Visibility = Visibility.Collapsed;
        }

        private void btnOtherAttr_Click(object sender, RoutedEventArgs e)
        {
            frmAttributeAddingToPath cAttribute = new frmAttributeAddingToPath(naviChildAttrGroupList);

            if (cAttribute.ShowDialog()==true)
            {
                naviChildAttrGroupList = cAttribute.getOtherAttribute();
                reloadAttribute(naviChildAttrGroupList);
            }
            //  cAttribute = new frmAttributeSetting(naviChildOtherAttrGroupList);

            //if (cAttribute.ShowDialog() == true)
            //{
            //    naviChildOtherAttrGroupList.
            //}
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
