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
    public partial class frmRecommendation : AMDESPage
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

        public frmRecommendation()
        {
            InitializeComponent();
            //amdesPageFrame = amdesFrame;
            //heightLimit = 430;
            loadRecommendation();
        }

        public frmRecommendation(Frame amdesFrame, List<Diagnosis> diaList,AMDESPage lastSec)
        {
            InitializeComponent();
            //lblSection.Content = sectionID;
            lastSection = lastSec;
            amdesPageFrame = amdesFrame;
            //prevPage = null;
            //btnPrev.Visibility = Visibility.Collapsed;
            //heightLimit = 430;
            Patient currPatient = CLIPSController.CurrentPatient;
            lblPatientID.Content = currPatient.NRIC;
            lblPatientName.Content = currPatient.Last_Name + " " + currPatient.First_Name;
            lblPatientAge.Content = "Age : " + currPatient.getAge();
            
            AllDiagnose = diaList;
            PageContent = new List<List<ucDiagnosis>>();
            loadRecommendation();

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

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            //int sectionID = CLIPSController.getCurrentQnGroupID();
            //frmSection lastSection = new frmSection(amdesPageFrame, sectionID);

            CLIPSController.retractDiagnosis();
            int sectionID = CLIPSController.getCurrentQnGroupID();
            amdesPageFrame.Navigate(lastSection);
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

        private void AddSymptons()
        {
            Label header = new Label();
            header.Content = "The patient has the following symptoms stated in the questionnaire :";
            header.Height = 30;
            header.FontWeight = FontWeights.Bold;

            StackPanel stkpnlSymptons = new StackPanel();
            stkpnlSymptons.Margin = new Thickness(10, 0, 0, 0);

            foreach (Symptom sym in CLIPSController.CurrentPatient.getLatestHistory().SymptomsList)
            {
                Label lblSymptons = new Label();
                lblSymptons.Content = sym.SymptomName;
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            frmRecommendationViewOnly frmConclusion = new frmRecommendationViewOnly(amdesPageFrame, CLIPSController.CurrentPatient.getLatestHistory(),true);
           
        }

        private frmSection createSection(int gid, List<QnHistory> qnhistory)
        {
            frmSection section = new frmSection(amdesPageFrame, gid, qnhistory);
            return section;
        }

        
    }
}
