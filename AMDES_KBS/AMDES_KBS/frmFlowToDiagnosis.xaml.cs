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
using CircularDependencyTool;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmFlowToDiagnosis.xaml
    /// </summary>

    public partial class frmFlowToDiagnosis : Window
    {
        int currStep = 1;
        List<ucNavigationFlowSetting> lstStep;
        Rules rule;

        //Graph drawing
        List<Graph> gList = new List<Graph>();
        Graph g;
        //end graph drawing

        private void loadGraph1()
        {
            gList.Clear();
            gList.Add(g);
            zz.DataContext = null;
            zz.DataContext = gList;
        }

        public frmFlowToDiagnosis()
        {
            InitializeComponent();
            //IEnumerable<Graph> g = GraphBuilder.BuildGraphs();

            lstStep = new List<ucNavigationFlowSetting>();

            if (!FirstQuestionController.checkFirstQuestion())
            {
                MessageBox.Show("Please set your first question before continuing!");
                new frmFirstPageSetting().ShowDialog();
            }

            LoadAllRule();

            //newFlowDetail();
        }

        private void LoadAllRule()
        {
            cboDiagnosisList.ItemsSource = NavigationController.getAllRules();
            cboDiagnosisList.SelectedIndex = -1;
        }

        private void LoadExistingNavi()
        {
            cboDiagnosisList.ItemsSource = NavigationController.getAllRules();
        }

        private void loadNaviList(Rules r)
        {
            //cboNaviList.ItemsSource = r.Navigations;
            txtDescription.Text = r.Description;
            lstDiagnosisList.ItemsSource = r.DiagnosisList;
            lstStep = new List<ucNavigationFlowSetting>();
            loadSteps(r.Navigations);
            //cboNaviList.ItemsSource = r.Navigations;
        }

        private void loadSteps(List<Navigation> naviList)
        {
            for (int i = 0; i < naviList.Count; i++)
            {
                Navigation navi = naviList[i];
                addStep(i + 1, navi);
            }
            currStep = 1;
            stkpnlSteps.Children.Clear();
            lstStep[0].loadIsConclusive();
            lstStep[0].loadCheckedAgeMoreOrLess();
            lstStep[0].loadCheckedYN();

            stkpnlSteps.Children.Add(lstStep[0]);
            btnNextStep.Visibility = Visibility.Visible;
            btnPrevStep.Visibility = Visibility.Hidden;
        }

        public void addStep(int stepNo, Navigation step)
        {
            //stkpnlSteps.Children.Clear();
            ucNavigationFlowSetting stepControl = new ucNavigationFlowSetting(stepNo, step);
            stepControl.chkConclusive.Checked += new RoutedEventHandler(chk_Checked);
            stepControl.chkConclusive.Unchecked += new RoutedEventHandler(chk_UnChecked);
            //stepControl
            stepControl.loadIsConclusive();
            stepControl.loadCheckedAgeMoreOrLess();
            stepControl.loadCheckedYN();

            lstStep.Add(stepControl);

            //stkpnlSteps.Children.Add(stepControl);
            //if (currStep == stepNo)
            //{
            //    btnPrevStep.Visibility = Visibility.Hidden;
            //}
            //else
            //{
            //    btnPrevStep.Visibility = Visibility.Visible;
            //}
        }

        private void newFlowDetail()
        {
            currStep = 1;
            rule = new Rules();
            rule.RuleID = NavigationController.getNextRuleRID();

            txtDescription.Text = "";
            lstDiagnosisList.ItemsSource = null;
            cboDiagnosisList.SelectedIndex = -1;
            lstStep = new List<ucNavigationFlowSetting>();
            addNewStep();
            btnNextStep.Visibility = Visibility.Visible;
            btnPrevStep.Visibility = Visibility.Hidden;

            g = new Graph("New Rule - Rule ID: " + rule.RuleID.ToString());
            loadGraph1();
            lblText.Content = "Displaying Decision Points";
        }

        public void addNewStep()
        {
            stkpnlSteps.Children.Clear();
            ucNavigationFlowSetting step = new ucNavigationFlowSetting(currStep);
            step.chkConclusive.Checked += new RoutedEventHandler(chk_Checked);
            step.chkConclusive.Unchecked += new RoutedEventHandler(chk_UnChecked);
            lstStep.Add(step);
            stkpnlSteps.Children.Add(step);
            if (currStep == 1)
            {
                btnPrevStep.Visibility = Visibility.Hidden;
            }
            else
            {
                btnPrevStep.Visibility = Visibility.Visible;
            }
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

        private void lstDiagnosisList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIdx = lstDiagnosisList.SelectedIndex;
            if (selectedIdx == -1)
            {
                return;
            }
        }

        private void btnNextStep_Click(object sender, RoutedEventArgs e)
        {
            currStep++;
            int tempPage = currStep - 1;
            if (tempPage < lstStep.Count)
            {
                loadSteps();
            }
            else
            {
                addNewStep();
            }

            if (cboDiagnosisList.SelectedIndex >= 0)
            {
                Navigation navs = rule.Navigations[currStep - 2];
                g.resetGraph();

                foreach (NaviChildCriteriaQuestion n in navs.ChildCriteriaQuestion)
                {
                    g.addGraphNodes(QuestionController.getGroupByID(n.CriteriaGrpID).Header);
                }

                loadGraph1();
            }
            else
            {
                //@KAI
                //i need the object of the previous navigation, 
                //i see that u did not add it to rules directly, so i am unable to get it back when 
                //no diagnosis id is selected, please facilitate
            }
            lblText.Content = "Displaying Last Decision Point (Previous Step)";
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            newFlowDetail();

        }

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            btnNextStep.Visibility = Visibility.Hidden;
        }

        private void chk_UnChecked(object sender, RoutedEventArgs e)
        {
            btnNextStep.Visibility = Visibility.Visible;
        }

        private void btnPrevStep_Click(object sender, RoutedEventArgs e)
        {
            currStep--;

            loadSteps();

            if (cboDiagnosisList.SelectedIndex >= 0)
            {
                Navigation navs;
                if (currStep == 1)
                    navs = rule.Navigations[currStep - 1];
                else
                    navs = rule.Navigations[currStep - 2];

                g.resetGraph();
                foreach (NaviChildCriteriaQuestion n in navs.ChildCriteriaQuestion)
                {
                    g.addGraphNodes(QuestionController.getGroupByID(n.CriteriaGrpID).Header);
                }
                loadGraph1();
            }
            else
            {
                //@KAI
                //i need the object of the previous navigation, 
                //i see that u did not add it to rules directly, so i am unable to get it back when 
                //no diagnosis id is selected, please facilitate
            }

            lblText.Content = "Displaying Current Decision Point";
        }

        private void loadSteps()
        {
            if (currStep == 1)
            {
                btnPrevStep.Visibility = Visibility.Hidden;
            }
            else
            {
                btnPrevStep.Visibility = Visibility.Visible;
            }

            stkpnlSteps.Children.Clear();
            lstStep[currStep - 1].loadCheckedYN();
            lstStep[currStep - 1].loadCheckedAgeMoreOrLess();
            disableNextButton(lstStep[currStep - 1]);
            stkpnlSteps.Children.Add(lstStep[currStep - 1]);

        }

        private void cboDiagnosisList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIdx = cboDiagnosisList.SelectedIndex;
            if (selectedIdx == -1)
            {
                return;
            }

            rule = (Rules)cboDiagnosisList.Items[selectedIdx];

            //Graph
            g = new Graph("RuleID: " + rule.RuleID + ", " + rule.Description);
            //g.addGraphNodes(QuestionController.getGroupByID(rule.Navigations[0].ChildCriteriaQuestion[0].CriteriaGrpID).Header);
            loadGraph1();
            //end graph

            loadNaviList(rule);
        }

        private bool saveNavigation()
        {
            rule.Description = txtDescription.Text;
            List<int> RIDList = new List<int>();
            if (rule.DiagnosisList.Count > 0)
            {
                foreach (Diagnosis dia in rule.DiagnosisList)
                {
                    RIDList.Add(dia.RID);
                }
            }

            for (int i = 0; i < RIDList.Count; i++)
            {
                rule.removeDiagnosisID(RIDList[i]);
            }

            for (int i = 0; i < lstDiagnosisList.Items.Count; i++)
            {
                Diagnosis dia = (Diagnosis)lstDiagnosisList.Items[i];
                rule.addDiagnosisID(dia.RID);
            }

            List<Navigation> NaviDeleteList = new List<Navigation>();
            if (rule.Navigations.Count > 0)
            {
                foreach (Navigation navi in rule.Navigations)
                {
                    NaviDeleteList.Add(navi);
                    //rule.removeNavigation(navi);
                }
            }

            for (int i = 0; i < NaviDeleteList.Count; i++)
            {
                rule.removeNavigation(NaviDeleteList[i]);
            }

            Navigation prevNavi = null;

            for (int i = 0; i < lstStep.Count; i++)
            {
                Navigation newNavi = new Navigation();
                if (prevNavi != null)
                {
                    //newNavi.addNavCriteriaQuestion(prevgetCriteria());
                    foreach (NaviChildCriteriaQuestion qn in prevNavi.ChildCriteriaQuestion)
                    {
                        newNavi.addNavCriteriaQuestion(qn);
                    }

                    foreach (NaviChildCritAttribute attr in prevNavi.ChildCriteriaAttributes)
                    {
                        newNavi.addNavCriteriaAttribute(attr);
                    }
                }

                ucNavigationFlowSetting currStep = lstStep[i];
                int destID = i + 1;
                if (i + 1 >= lstStep.Count)
                {
                    destID = -1;
                }
                else
                {
                    ucNavigationFlowSetting nextStep = lstStep[i + 1];

                    if (currStep.chkConclusive.IsChecked == true)
                    {
                        destID = -1;
                    }
                    else
                    {
                        destID = nextStep.getGroupID();
                    }
                    //if (destID == -1)
                    //{
                    //    break;
                    //}
                }

                currStep.getAnswer();
                newNavi.addNavCriteriaQuestion(currStep.getCriteria());

                foreach (NaviChildCritAttribute attr in currStep.getAttrList())
                {
                    newNavi.addNavCriteriaAttribute(attr);
                }

                newNavi.DestGrpID = destID;
                prevNavi = newNavi;
                rule.insertNavigation(newNavi);
                if (destID == -1)
                {
                    break;
                }
            }
            prevNavi.DiagnosesID = rule.diagnosis;
            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (saveNavigation())
            {
                NavigationController.updateRules(rule);
                g = new Graph("RuleID: " + rule.RuleID + ", " + rule.Description);
                Navigation nav = rule.Navigations[rule.Navigations.Count - 1];
                foreach (NaviChildCriteriaQuestion n in nav.ChildCriteriaQuestion)
                {
                    g.addGraphNodes(QuestionController.getGroupByID(n.CriteriaGrpID).Header);
                }
                loadGraph1();
                lblText.Content = "Displaying Saved Rule";
                LoadAllRule();
                MessageBox.Show("Saved!");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            NavigationController.deleteRules(rule.RuleID);
            refreshPage();
            MessageBox.Show("Deletion Complete");
        }

        private void refreshPage()
        {
            LoadAllRule();
            newFlowDetail();
        }

        private void disableNextButton(ucNavigationFlowSetting ucControl)
        {
            if (ucControl.chkConclusive.IsChecked == true)
            {
                btnNextStep.Visibility = Visibility.Hidden;
            }
            else
            {
                btnNextStep.Visibility = Visibility.Visible;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
