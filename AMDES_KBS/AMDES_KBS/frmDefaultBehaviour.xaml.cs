using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmDefaultBehaviour.xaml
    /// </summary>
    public partial class frmDefaultBehaviour : Window
    {
        Navigation navi;

        List<NaviChildCriteriaQuestion> naviChildQuestion;
        List<NaviChildCritAttribute> naviChildAttribute;
        List<Navigation> AllBehaviour;

        public frmDefaultBehaviour()
        {
            InitializeComponent();
            newForm();
            loadQuestionGroup();
        }

        private void loadAllBehaviour()
        {
            cboBehaviourList.Items.Clear();
            AllBehaviour = DefaultBehaviorController.getAllDefaultBehavior();
            for (int i = 0; i < AllBehaviour.Count; i++)
            {
                Navigation behaviour = AllBehaviour[i];
                bool conclusive = behaviour.isConclusive();
                string s="B" + (i+1) + " - ";
                s += "Conclusive - " + conclusive.ToString().ToUpper();
                if (conclusive)
                {
                    foreach (int rID in behaviour.DiagnosesID)
                    {
                        Diagnosis dia = DiagnosisController.getDiagnosisByID(rID);
                        s += Environment.NewLine + "Diagnosis - " + dia.Header;
                    }
                }
                else
                {
                    QuestionGroup qg = QuestionController.getGroupByID(behaviour.DestGrpID);
                    s += Environment.NewLine + "Destination - " + qg.Header;
                }

                cboBehaviourList.Items.Add(s);
            }
        }

        private void newForm()
        {
            navi = new Navigation();
            naviChildQuestion = new List<NaviChildCriteriaQuestion>();
            naviChildAttribute = new List<NaviChildCritAttribute>();
            cboBehaviourList.SelectedIndex = -1;
            cboGroupList.SelectedIndex = -1;
            cboDestination.SelectedIndex = -1;
            chkConclusive.IsChecked = false;
            stkpnlDiagnosis.Visibility = Visibility.Collapsed;
            stkpnlSectionDestination.Visibility = Visibility.Visible;
            lstDiagnosisList.ItemsSource = new List<Diagnosis>();
            //txtDescription.Text = "";
            reloadAttribute(naviChildAttribute);
            reloadCriteria();
            loadAllBehaviour();
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

        private void reloadAttribute(List<NaviChildCritAttribute> otherAttr)
        {
            lstAttributeList.Items.Clear();
            foreach (NaviChildCritAttribute attr in otherAttr)
            {
                string s = "=";
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


        private void loadQuestionGroup()
        {
            cboGroupList.ItemsSource = QuestionController.getAllQuestionGroup();
            cboDestination.ItemsSource = QuestionController.getAllQuestionGroup();
            cboGroupList.SelectedIndex = 0;
            cboDestination.SelectedIndex = 0;
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

            if (!checkDuplicateCriteria(newCriteria))
            {
                naviChildQuestion.Add(newCriteria);
                reloadCriteria();
            }
        }

        private bool checkDuplicateCriteria(NaviChildCriteriaQuestion crit)
        {
            foreach (NaviChildCriteriaQuestion critQn in this.naviChildQuestion)
            {
                if (crit.CriteriaGrpID==critQn.CriteriaGrpID)
                {
                    return true;
                }
            }

            return false;
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

            int criteriasIdx = this.lstCriteriaList.SelectedIndex;
            if (criteriasIdx == -1)
            {
                return;
            }

            int sIdx = cboGroupList.SelectedIndex;
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

            NaviChildCriteriaQuestion oldCriteria = naviChildQuestion[criteriasIdx];
            oldCriteria.CriteriaGrpID = qg.GroupID;
            oldCriteria.Ans = criteriaResult;
            reloadCriteria();
        }

        private void btnAddDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            List<Diagnosis> dList = (List<Diagnosis>)lstDiagnosisList.ItemsSource;
            frmDiagnosisAddingToPath cDiagnosis = new frmDiagnosisAddingToPath(dList);
            if (cDiagnosis.ShowDialog() == true)
            {
                lstDiagnosisList.ItemsSource = null;
                lstDiagnosisList.ItemsSource = cDiagnosis.getAddedDiagnosis();
            }
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

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            newForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (saveBehaviour())
            {
                MessageBox.Show("Saved!");
                newForm();
            }
        }

        private bool saveBehaviour()
        {
            List<int> RIDList = new List<int>();
            if (navi.DiagnosesID.Count > 0)
            {
                foreach (int diaID in navi.DiagnosesID)
                {
                    RIDList.Add(diaID);
                }
            }

            for (int i = 0; i < RIDList.Count; i++)
            {
                navi.removeDiagnosisID(RIDList[i]);
            }

            navi.DestGrpID = -1;

            if (chkConclusive.IsChecked == true)
            {

                for (int i = 0; i < lstDiagnosisList.Items.Count; i++)
                {
                    Diagnosis dia = (Diagnosis)lstDiagnosisList.Items[i];
                    navi.addDiagnosisID(dia.RID);
                }
            }
            else
            {
                int sIdx = cboDestination.SelectedIndex;
                if (sIdx==-1)
                {
                    MessageBox.Show("Please Select ur destination section!");
                    return false;
                }

                QuestionGroup qg = (QuestionGroup)cboDestination.Items[sIdx];
                navi.DestGrpID = qg.GroupID;
            }


            navi.clearCriteriaQuestions();
            navi.clearCriteriaAttributes();

            foreach (NaviChildCriteriaQuestion naviCriteria in naviChildQuestion)
            {
                navi.addNavCriteriaQuestion(naviCriteria);
            }

            foreach (NaviChildCritAttribute naviAttr in naviChildAttribute)
            {
                navi.addNavCriteriaAttribute(naviAttr);
            }

            try
            {
                DefaultBehaviorController.updateRules(navi);
            }
            catch (Exception ex)
            {
                return false;
//                throw;
            }
            return true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int sIdx = cboBehaviourList.SelectedIndex;
            if (sIdx == -1)
            {
                return;
            }
            //Navigation sNavi = (Navigation)cboDiagnosisList.Items[sIdx];
            DefaultBehaviorController.deleteDefaultBehavior(int.Parse(navi.NavID));
            MessageBox.Show("Behaviour Deleted!");
            newForm();
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cboDiagnosisList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sIdx = cboBehaviourList.SelectedIndex;
            if (sIdx == -1)
            {
                return;
            }
            navi = AllBehaviour[sIdx];
            LoadExistingBehaviour();
        }

        private void LoadExistingBehaviour()
        {
            bool isConclusive = navi.isConclusive();
            chkConclusive.IsChecked = isConclusive;
            if (isConclusive)
            {
                stkpnlDiagnosisRule.Visibility = Visibility.Visible;
                stkpnlSectionDestination.Visibility = Visibility.Collapsed;
                lstDiagnosisList.ItemsSource = null;
                List<Diagnosis> diaListExist = new List<Diagnosis>();
                List<Diagnosis> diaList = DiagnosisController.getAllDiagnosis();
                foreach (int rID in navi.DiagnosesID)
                {
                    for (int i = 0; i < diaList.Count; i++)
                    {
                        Diagnosis dia = diaList[i];
                        if (dia.RID == rID)
                        {
                            diaListExist.Add(dia);
                            break;
                        }
                    }
                }
                lstDiagnosisList.ItemsSource = diaListExist;
            }
            else
            {
                stkpnlDiagnosisRule.Visibility = Visibility.Collapsed;
                stkpnlSectionDestination.Visibility = Visibility.Visible;
                int sIdx = -1;
                for (int i = 0; i < cboDestination.Items.Count; i++)
                {
                    QuestionGroup qgItem = (QuestionGroup)cboDestination.Items[i];
                    if (qgItem.GroupID==navi.DestGrpID)
                    {
                        sIdx = i;
                        break;
                    }
                }
                cboDestination.SelectedIndex = sIdx;
            }

            this.naviChildQuestion = new List<NaviChildCriteriaQuestion>();
            this.naviChildAttribute = new List<NaviChildCritAttribute>();

            foreach (NaviChildCriteriaQuestion naviCriteria in navi.ChildCriteriaQuestion)
            {
                naviChildQuestion.Add(naviCriteria);
            }

            foreach (NaviChildCritAttribute naviAttr in navi.ChildCriteriaAttributes)
            {
                naviChildAttribute.Add(naviAttr);
            }

            reloadCriteria();
            reloadAttribute(naviChildAttribute);
        }

        private void chkConclusive_Checked(object sender, RoutedEventArgs e)
        {
            stkpnlSectionDestination.Visibility = Visibility.Collapsed;
            stkpnlDiagnosis.Visibility = Visibility.Visible;
            stkpnlDiagnosisRule.Visibility = Visibility.Visible;
            lstDiagnosisList.Visibility = Visibility.Visible;
        }

        private void chkConclusive_Unchecked(object sender, RoutedEventArgs e)
        {
            stkpnlSectionDestination.Visibility = Visibility.Visible;
            stkpnlDiagnosisRule.Visibility = Visibility.Collapsed;
            stkpnlDiagnosis.Visibility = Visibility.Collapsed;
        }

        private void txtAttrNumValue_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = App.NumberValidationTextBox(e.Text);
        }

        private void btnOtherAttr_Click(object sender, RoutedEventArgs e)
        {
            frmAttributeAddingToPath cAttribute = new frmAttributeAddingToPath(naviChildAttribute);

            if (cAttribute.ShowDialog() == true)
            {
                naviChildAttribute = cAttribute.getOtherAttribute();
                reloadAttribute(naviChildAttribute);
            }
            //  cAttribute = new frmAttributeSetting(naviChildOtherAttrGroupList);

            //if (cAttribute.ShowDialog() == true)
            //{
            //    naviChildOtherAttrGroupList.
            //}
        }

    }
}
