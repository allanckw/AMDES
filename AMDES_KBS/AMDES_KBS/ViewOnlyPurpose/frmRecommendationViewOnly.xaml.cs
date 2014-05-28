using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;
using System.Windows.Input;
using System.Windows.Documents;
using System;

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
        List<Diagnosis> allDiagnoses;
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
            allDiagnoses = h.Diagnoses;
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

            foreach (Diagnosis diaRule in allDiagnoses)
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

            rtbPrint.Document = Printer.writeFlowDoc(allDiagnoses);
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
                lblSymptons.FontSize = 14;
                lblSymptons.Content = App.bulletForm() + sym.SymptomName;
                stkpnlSymptons.Children.Add(lblSymptons);
            }

            if (stkpnlSymptons.Children.Count == 0)
            {
                header.Content = "The evaluation does not suggest dementia in this patient";
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

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            DoThePrint(this.rtbPrint.Document);
        }

        private void DoThePrint(FlowDocument document)
        {
            // Clone the source document's content into a new FlowDocument.
            // This is because the pagination for the printer needs to be
            // done differently than the pagination for the displayed page.
            // We print the copy, rather that the original FlowDocument.
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            TextRange source = new TextRange(document.ContentStart, document.ContentEnd);
            source.Save(s, DataFormats.Xaml);
            FlowDocument copy = new FlowDocument();
            TextRange dest = new TextRange(copy.ContentStart, copy.ContentEnd);
            dest.Load(s, DataFormats.Xaml);

            // Create a XpsDocumentWriter object, implicitly opening a Windows common print dialog,
            // and allowing the user to select a printer.

            // get information about the dimensions of the seleted printer+media.
            System.Printing.PrintDocumentImageableArea ia = null;
            System.Windows.Xps.XpsDocumentWriter docWriter = System.Printing.PrintQueue.CreateXpsDocumentWriter(ref ia);

            if (docWriter != null && ia != null)
            {
                DocumentPaginator paginator = ((IDocumentPaginatorSource)copy).DocumentPaginator;

                // Change the PageSize and PagePadding for the document to match the CanvasSize for the printer device.
                paginator.PageSize = new Size(ia.MediaSizeWidth, ia.MediaSizeHeight);
                Thickness t = new Thickness(72);  // copy.PagePadding;
                copy.PagePadding = new Thickness(
                                 Math.Max(ia.OriginWidth, t.Left),
                                   Math.Max(ia.OriginHeight, t.Top),
                                   Math.Max(ia.MediaSizeWidth - (ia.OriginWidth + ia.ExtentWidth), t.Right),
                                   Math.Max(ia.MediaSizeHeight - (ia.OriginHeight + ia.ExtentHeight), t.Bottom));

                copy.ColumnWidth = double.PositiveInfinity;
                //copy.PageWidth = 528; // allow the page to be the natural with of the output device

                // Send content to the printer.
                docWriter.Write(paginator);
            }

        }

    }
}
