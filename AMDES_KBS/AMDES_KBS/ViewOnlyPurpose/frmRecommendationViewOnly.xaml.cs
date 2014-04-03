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
            lblPatientID.Content = currPatient.NRIC;
            lblPatientName.Content = currPatient.Last_Name + " " + currPatient.First_Name;
            lblPatientAge.Content = "Age : " + currPatient.getAge();
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
            AllDiagnose = h.Diagnoses;
            PageContent = new List<List<ucDiagnosis>>();
            loadRecommendation();
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

            foreach (Diagnosis diaRule in AllDiagnose)
            {
                ucDiagnosis diagnosisControl = new ucDiagnosis(diaRule);
                //diagnosis.addSymptoms(i + 1);
                PageFrame.Children.Add(diagnosisControl);
            }
            //sortPage();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            amdesPageFrame.Navigate(lastSection);
        }

        private void AddSymptons()
        {
            Label header = new Label();
            header.Content = "The patient has the following symptoms stated in the questionnaire :";
            header.Height = 30;
            StackPanel stkpnlSymptons = new StackPanel();
            stkpnlSymptons.Margin = new Thickness(20, 0, 0, 0);

            foreach (Symptom sym in CLIPSController.CurrentPatient.getLatestHistory().SymptomsList)
            {
                Label lblSymptons = new Label();
                lblSymptons.Content = "Symptoms - " + sym.SymptomName;
                stkpnlSymptons.Children.Add(lblSymptons);
            }

            if (stkpnlSymptons.Children.Count == 0)
            {
                header.Content = "The patient has no symptoms.";
            }
            //<ScrollViewer x:Name="svS" VerticalScrollBarVisibility="auto" Height="160" Width="960" HorizontalAlignment="Left">
            ScrollViewer sv = new ScrollViewer();
            sv.Height=160-30;
            sv.Width=960;
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv.Content = stkpnlSymptons;

            StackPanel stkpnlSymptonsPnl = new StackPanel();
            stkpnlSymptonsPnl.Children.Add(header);
            stkpnlSymptonsPnl.Children.Add(sv);
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
