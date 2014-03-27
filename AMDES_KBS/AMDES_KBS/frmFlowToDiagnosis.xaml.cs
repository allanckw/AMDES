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
    /// Interaction logic for frmFlowToDiagnosis.xaml
    /// </summary>

    public partial class frmFlowToDiagnosis : Window
    {
        int currStep=1;
        List<ucNavigationFlowSetting> lstStep;
        public frmFlowToDiagnosis()
        {
            InitializeComponent();
            lstStep = new List<ucNavigationFlowSetting>();
        }

        public void newFlowDetail()
        {
            currStep = 1;
            lstStep = new List<ucNavigationFlowSetting>();
            addNewStep();
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

        private void cboGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lstDiagnosisList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstDiagnosisList.SelectedIndex = -1;
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
            if (currStep > 0)
            {
                currStep--;
                loadSteps();
                btnPrevStep.Visibility = Visibility.Visible;
            }
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
            
            stkpnlSteps.Children.Add(lstStep[currStep-1]);

        }
    }
}
