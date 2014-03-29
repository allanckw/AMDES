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
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmDefaultBehaviour.xaml
    /// </summary>
    public partial class frmDefaultBehaviour : Window
    {
        List<NaviChildCriteriaQuestion> naviChildQuestion;
        List<NaviChildCritAttribute> naviChildAttribute;

        public frmDefaultBehaviour()
        {
            InitializeComponent();
            newForm();
            loadQuestionGroup();
        }

        private void newForm()
        {
            naviChildQuestion = new List<NaviChildCriteriaQuestion>();
            naviChildAttribute = new List<NaviChildCritAttribute>();
            cboDiagnosisList.SelectedIndex = -1;
            txtDescription.Text = "";
            reloadAttribute();
            reloadCriteria();
        }

        private void reloadCriteria()
        {
            //lstCriteriaList.ItemsSource = naviChildQuestion;  
            lstCriteriaList.Items.Clear();
            foreach (NaviChildCriteriaQuestion criteria in naviChildQuestion)
            {
                QuestionGroup qg = QuestionController.getGroupByID(criteria.CriteriaGrpID);
                lstCriteriaList.Items.Add(qg.Header + " - " + criteria.Ans);
            }
        }

        private void reloadAttribute()
        {
            lstAttributeList.Items.Clear();
            foreach (NaviChildCritAttribute attr in naviChildAttribute)
            {
                string s = "";
                if (attr.getAttributeTypeENUM() == NaviChildCritAttribute.AttributeCmpType.LessThan)
                {
                    s = "<";
                }
                else
                {
                    s = ">=";
                }
                lstAttributeList.Items.Add("AGE" + s + " " + attr.AttributeValue);
            }
        }

        private void loadQuestionGroup()
        {
            cboGroupList.ItemsSource = QuestionController.getAllQuestionGroup();
        }

        private void btnAddCriteria_Click(object sender, RoutedEventArgs e)
        {
            int sIdx = cboGroupList.SelectedIndex;
            bool criteriaResult=false;
            if (sIdx==-1)
            {
                return;
            }

            QuestionGroup qg = (QuestionGroup)cboGroupList.Items[sIdx];

            if (radY.IsChecked == true)
            {
                criteriaResult = true;
            }
            else
            {
                criteriaResult = false;
            }

            NaviChildCriteriaQuestion newCriteria = new NaviChildCriteriaQuestion();
            newCriteria.CriteriaGrpID = qg.GroupID;
            newCriteria.Ans = criteriaResult;
            naviChildQuestion.Add(newCriteria);
            reloadCriteria();
        }

        private void btnDeleteCriteria_Click(object sender, RoutedEventArgs e)
        {
            int sIdx = lstCriteriaList.SelectedIndex;
            if (sIdx == -1)
            {
                return;
            }
            naviChildQuestion.RemoveAt(sIdx);
            reloadCriteria();
        }

        private void btnModifyCriteria_Click(object sender, RoutedEventArgs e)
        {
            bool criteriaResult = false;

            int sIdx = lstCriteriaList.SelectedIndex;
            if (sIdx == -1)
            {
                return;
            }

            QuestionGroup qg = (QuestionGroup)cboGroupList.Items[sIdx];

            if (radY.IsChecked == true)
            {
                criteriaResult = true;
            }
            else
            {
                criteriaResult = false;
            }

            NaviChildCriteriaQuestion oldCriteria = naviChildQuestion[sIdx];
            oldCriteria.CriteriaGrpID = qg.GroupID;
            oldCriteria.Ans = criteriaResult;
            reloadCriteria();
        }

        private void btnAddAttribute_Click(object sender, RoutedEventArgs e)
        {
            NaviChildCritAttribute newAttribute = new NaviChildCritAttribute();
            newAttribute.AttributeName = "AGE";
            newAttribute.AttributeValue = txtAge.Text;
            if (radless.IsChecked == true)
            {
                newAttribute.setRuleType((int)NaviChildCritAttribute.AttributeCmpType.LessThan);
            }
            else
            {
                newAttribute.setRuleType((int)NaviChildCritAttribute.AttributeCmpType.MoreThanEqual);
            }

            naviChildAttribute.Add(newAttribute);
            reloadAttribute();
        }

        private void btnAddDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            List<Diagnosis> dList = (List<Diagnosis>)lstDiagnosisList.ItemsSource;
            frmDiagnosisAddingToPath cDiagnosis = new frmDiagnosisAddingToPath(dList);
            if (cDiagnosis.ShowDialog() == true)
            {
                lstDiagnosisList.ItemsSource = cDiagnosis.getAddedDiagnosis();
            }
        }

        private void btnDeleteAttribute_Click(object sender, RoutedEventArgs e)
        {
            int sIdx = lstAttributeList.SelectedIndex;
            if (sIdx == -1)
            {
                return;
            }
            naviChildAttribute.RemoveAt(sIdx);
            reloadAttribute();
        }

        private void btnModifyAttribute_Click(object sender, RoutedEventArgs e)
        {
            int sIdx = lstAttributeList.SelectedIndex;
            if (sIdx == -1)
            {
                return;
            }
            NaviChildCritAttribute oldAttribute = naviChildAttribute[sIdx];
            oldAttribute.AttributeName = "AGE";
            oldAttribute.AttributeValue = txtAge.Text;
            if (radless.IsChecked == true)
            {
                oldAttribute.setRuleType((int)NaviChildCritAttribute.AttributeCmpType.LessThan);
            }
            else
            {
                oldAttribute.setRuleType((int)NaviChildCritAttribute.AttributeCmpType.MoreThanEqual);
            }

            reloadAttribute();
        }

        private void loadExistingCriteria(NaviChildCriteriaQuestion criteria)
        {
            int sIdx = -1;
            for (int i = 0; i < cboGroupList.Items.Count; i++)
            {
                QuestionGroup qg = (QuestionGroup)cboGroupList.Items[i];
                if (criteria.CriteriaGrpID == qg.GroupID)
                {
                    sIdx = i;
                    break;
                }
            }
            cboGroupList.SelectedIndex = sIdx;
            radY.IsChecked = criteria.Ans;
            radN.IsChecked = !criteria.Ans;
        }

        private void loadExistingAttribute(NaviChildCritAttribute attribute)
        {
            if (attribute.getAttributeTypeENUM() == NaviChildCritAttribute.AttributeCmpType.MoreThanEqual)
            {
                radMoreEqual.IsChecked = true;
            }
            else
            {
                radless.IsChecked = true;
            }
            txtAge.Text = attribute.AttributeValue;
        }

        private void lstCriteriaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sIdx = lstCriteriaList.SelectedIndex;
            if (sIdx==-1)
            {
                return;
            }
            NaviChildCriteriaQuestion existingCriteria = naviChildQuestion[sIdx];
            loadExistingCriteria(existingCriteria);
        }

        private void lstAttributeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sIdx = lstAttributeList.SelectedIndex;
            if (sIdx == -1)
            {
                return;
            }
            NaviChildCritAttribute existingAttribute = naviChildAttribute[sIdx];
            loadExistingAttribute(existingAttribute);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            newForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstDiagnosisList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboDiagnosisList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
