using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmRecommendation.xaml
    /// </summary>
    public partial class frmRecommendationViewOnly : AMDESPage
    {
        //double heightLimit = 0;
        //double currHeight = 0;
        //int CurrPageNo = 1;
        //int TotalPageNo = 1;

        Frame amdesPageFrame;
        List<Diagnosis> AllDiagnose;
        List<List<ucDiagnosis>> PageContent;
        AMDESPage lastSection;
        //bool collapseRest = false;

        public frmRecommendationViewOnly()
        {
            InitializeComponent();
            //amdesPageFrame = amdesFrame;
            //heightLimit = 430;
            loadRecommendation();
        }

        public frmRecommendationViewOnly(Frame amdesFrame, History h, bool fromRecommendations = false)
        {
            InitializeComponent();
            amdesPageFrame = amdesFrame;
            Patient currPatient = CLIPSController.CurrentPatient;
            lblPatientID.Content = "Patient's ID: " + currPatient.NRIC;
            lblPatientName.Content = "Patient's Name: " + currPatient.Last_Name + " " + currPatient.First_Name;
            lblPatientAge.Content = "Patient's Age: " + currPatient.getAge();
            if (CLIPSController.savePatient == false)
            {
                lblPatientID.Visibility = Visibility.Collapsed;
                lblPatientName.Visibility = Visibility.Collapsed;
                lblPatientAge.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                lblPatientID.Visibility = Visibility.Visible;
                lblPatientName.Visibility = Visibility.Visible;
                lblPatientAge.Margin = new Thickness(10, 0, 0, 0);
            }
            AllDiagnose = h.Diagnoses;
            PageContent = new List<List<ucDiagnosis>>();
            loadRecommendation();
            LoadResources();
            loadHistory(h);

            if (fromRecommendations)
            {
                amdesPageFrame.Navigate(lastSection);
            }
        }

        private void loadHistory(History h)
        {
            frmSectionViewOnly prevSection = null;

            Dictionary<int, List<QnHistory>> history = h.getHistory();

            for (int i = 0; i < history.Keys.Count; i++)
            {
                int k = history.Keys.ElementAt(i);
                List<QnHistory> qnHis = h.retrieveHistoryList(k);

                frmSectionViewOnly QnSect = createSection(k, qnHis);

                if (prevSection != null)
                {
                    prevSection.setNextPage(QnSect);
                    QnSect.setPrevPage(prevSection);
                }

                prevSection = QnSect;
            }
            lastSection = prevSection;
            prevSection.setNextPage(this);
        }

        private frmSectionViewOnly createSection(int gid, List<QnHistory> qnhistory)
        {
            frmSectionViewOnly section = new frmSectionViewOnly(amdesPageFrame, gid, qnhistory);
            return section;
        }

        public void loadRecommendation()
        {
            AddSymptons();

            //Label header = new Label();
            //header.Content = ""; //"The following course of action(s) are recommended for the patient: ";
            //header.Height = 10;
            //header.FontWeight = FontWeights.Bold;
            //header.FontSize = 15;
            //header.Margin = new Thickness(0, 10, 0, 0);

            foreach (Diagnosis diaRule in AllDiagnose)
            {
                ucDiagnosis diagnosisControl = new ucDiagnosis(diaRule);
                //diagnosis.addSymptoms(i + 1);
                PageFrame.Children.Add(diagnosisControl);
            }

            if (CLIPSController.savePatient == false)
            {
                lblPatientID.Visibility = Visibility.Collapsed;
                lblPatientName.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblPatientID.Visibility = Visibility.Visible;
                lblPatientName.Visibility = Visibility.Visible;
            }
        }

        private void LoadResources()
        {
            List<Diagnosis> resources = DiagnosisController.getResourceRules();
            //Label header = new Label();
            //header.Content = "Resources that you may find useful: ";
            //header.Height = 30;
            //header.FontSize = 15;
            //header.FontWeight = FontWeights.Bold;
            //header.Margin = new Thickness(0, 10, 0, 0);

            StackPanel stkpnlRes = new StackPanel();
            //stkpnlRes.Margin = new Thickness(10, 0, 0, 0);

            foreach (Diagnosis d in resources)
            {

                ucDiagnosisResource newDiagRes = new ucDiagnosisResource(d);
                stkpnlRes.Children.Add(newDiagRes);
            }

            //if (stkpnlRes.Children.Count == 0)
            //{
            //    header.Content = "There is no resource available.";
            //}

            StackPanel stkpnlResPnl = new StackPanel();
            //stkpnlResPnl.Children.Add(header);
            stkpnlResPnl.Children.Add(stkpnlRes);
            PageRFrame.Children.Add(stkpnlResPnl);
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            amdesPageFrame.Navigate(lastSection);
        }

        private void AddSymptons()
        {
            Label header = new Label();
            header.Content = "The patient has the following issues uncovered from the questionnaire: ";
            header.Height = 30;
            header.FontWeight = FontWeights.Bold;
            header.Margin = new Thickness(15, 0, 0, 0);
            StackPanel stkpnlSymptons = new StackPanel();
            stkpnlSymptons.Margin = new Thickness(20, 0, 0, 0);

            foreach (Symptom sym in CLIPSController.CurrentPatient.getLatestHistory().SymptomsList)
            {
                Label lblSymptons = new Label();
                lblSymptons.Content = App.bulletForm() + sym.SymptomName;
                stkpnlSymptons.Children.Add(lblSymptons);
            }

            if (stkpnlSymptons.Children.Count == 0)
            {
                header.Content = "The patient has no issues";
            }
            //<ScrollViewer x:Name="svS" VerticalScrollBarVisibility="auto" Height="160" Width="960" HorizontalAlignment="Left">
            //ScrollViewer sv = new ScrollViewer();
            //sv.Width = tcResults.Width-30;
            //sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            //sv.Content = stkpnlSymptons;

            StackPanel stkpnlSymptonsPnl = new StackPanel();
            stkpnlSymptonsPnl.Children.Add(header);
            stkpnlSymptonsPnl.Children.Add(stkpnlSymptons);
            PageSFrame.Children.Add(stkpnlSymptonsPnl);
        }

        private void btnTestAgin_Click(object sender, RoutedEventArgs e)
        {
            AssertQuestions();
            int sectionID = CLIPSController.getCurrentQnGroupID();

            //MessageBox.Show(sectionID.ToString());

            frmSection TestSection = new frmSection(amdesPageFrame, sectionID);
            amdesPageFrame.Navigate(TestSection);
        }

        private void AssertQuestions()
        {
            CLIPSController.clearAndLoadNew();
        }

    }
}
