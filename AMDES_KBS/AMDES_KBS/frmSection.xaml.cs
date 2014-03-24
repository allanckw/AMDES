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
    /// Interaction logic for frmSection.xaml
    /// </summary>
    public partial class frmSection : AMDESPage
    {
        double heightLimit = 0;
        double currHeight = 0;
        int CurrPageNo = 1;
        int TotalPageNo = 1;

        Frame amdesPageFrame;
        QuestionType sectionType;
        QuestionGroup QG;
        frmSection prevPage;

        List<List<ucQuestion>> PageContent = new List<List<ucQuestion>>();
        List<ucQuestion> WholeContent = new List<ucQuestion>();

        bool collapseRest = false;
        public frmSection(Frame amdesFrame, int sectionID)
        {
            InitializeComponent();
            lblSection.Content = sectionID;
            amdesPageFrame = amdesFrame;
            prevPage = null;
            btnPrev.Visibility = Visibility.Collapsed;
            heightLimit = 430;
            Patient currPatient = CLIPSController.CurrentPatient;
            lblPatientID.Content = currPatient.NRIC;
            lblPatientName.Content = currPatient.Last_Name + " " + currPatient.First_Name;



            loadSection(sectionID);
        }

        public frmSection(Frame amdesFrame, int sectionID, frmSection prevSection)
        {
            InitializeComponent();
            amdesPageFrame = amdesFrame;
            prevPage = prevSection;
            btnPrev.Visibility = Visibility.Visible;
            heightLimit = 430;
            Patient currPatient = CLIPSController.CurrentPatient;
            lblPatientID.Content = currPatient.NRIC;
            lblPatientName.Content = currPatient.Last_Name + " " + currPatient.First_Name;
            loadSection(sectionID);
        }

        //public frmSection(frmSection prevSection)
        //{
        //    InitializeComponent();
        //    lblSection.Content = prevSection.lblSection.Content;
        //    amdesPageFrame = prevSection.amdesPageFrame;
        //    prevPage = prevSection.prevPage;
        //    heightLimit = 430;
        //    Patient currPatient = CLIPSController.CurrentPatient;
        //    lblPatientID.Content = currPatient.NRIC;
        //    lblPatientName.Content = currPatient.Last_Name + " " + currPatient.First_Name;
        //    PageContent = prevSection.PageContent;
        //    WholeContent = prevSection.WholeContent;
        //    heightLimit = prevSection.heightLimit;
        //    CurrPageNo = prevSection.CurrPageNo;
        //    TotalPageNo = prevSection.TotalPageNo;
        //    collapseRest = prevSection.collapseRest;
        //    sectionType = prevSection.sectionType;
        //    //loadSection(sectionID);

        //    //double heightLimit = 0;
        //    //double currHeight = 0;
        //    //int CurrPageNo = 1;
        //    //int TotalPageNo = 1;

        //    //Frame amdesPageFrame;
        //    //QuestionType sectionType;
        //    //frmSection prevPage;

        //    //List<List<ucQuestion>> PageContent = new List<List<ucQuestion>>();
        //    //List<ucQuestion> WholeContent = new List<ucQuestion>();

        //    //bool collapseRest = false;
        //}

        //public void loadPrevSection(frmSection prevSection)
        //{
        //    this.MainFrame = prevSection.MainFrame;
        //}


        public void loadSection(int sectionID)
        {
            WholeContent = new List<ucQuestion>();
            QG = QuestionController.getGroupByID(sectionID);
            sectionType = QG.getQuestionTypeENUM();
            stkpnlScore.Visibility = Visibility.Hidden;
            if (this.sectionType == QuestionType.COUNT)
            {
                stkpnlScore.Visibility = Visibility.Visible;
                QuestionCountGroup QCG = (QuestionCountGroup)QG;
                lblCurrScore.Content = 0;
                lblCurrScore.Tag = QCG.Threshold;
                lblTotalScore.Content = QCG.MaxQuestions;
            }

            lblSection.Content = QG.Header;
            txtDesc.Text = QG.Description.Replace("~~", Environment.NewLine);
            foreach (Question q in QG.Questions)
            {
                addQuestion(q);
            }
            sortPage();
        }

        public void addQuestion(Question q)
        {
            ucQuestion ucQ = new ucQuestion();
            if (sectionType == QuestionType.COUNT)
            {
                ucQ.loadQuestion(q, lblCurrScore);
            }
            else
            {
                //ucQ.Name="Question" + q.ID;
                ucQ.loadQuestion(q, null);
            }
            //currHeight += Math.Ceiling(ucQ.getHeight());
            QuestionFrame.Children.Add(ucQ);
            WholeContent.Add(ucQ);
        }

        private void sortPage()
        {
            currHeight = 0;
            List<ucQuestion> QuestionPerPage = new List<ucQuestion>();
            foreach (ucQuestion item in QuestionFrame.Children)
            {
                string temp = item.Name;
                currHeight += Math.Ceiling(item.getHeight());

                if (currHeight >= heightLimit)
                {
                    currHeight = 0;
                    currHeight += Math.Ceiling(item.getHeight());
                    PageContent.Add(QuestionPerPage);
                    QuestionPerPage = new List<ucQuestion>();
                    if (collapseRest == false)
                    {
                        collapseRest = true;
                    }
                }

                if (collapseRest)
                {
                    item.setVisibility(Visibility.Collapsed);
                }

                QuestionPerPage.Add(item);
            }

            if (QuestionPerPage.Count > 0)
            {
                PageContent.Add(QuestionPerPage);
            }

            TotalPageNo = PageContent.Count;
            lblTotalPage.Content = PageContent.Count.ToString("D2");
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (CurrPageNo != TotalPageNo)
            {
                foreach (ucQuestion item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Collapsed);
                }

                CurrPageNo++;
                foreach (ucQuestion item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Visible);
                }
                lblCurrPage.Content = CurrPageNo.ToString("D2");
            }
            else
            {
                NavigationNext();
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (CurrPageNo != 1)
            {
                foreach (ucQuestion item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Collapsed);
                }

                CurrPageNo--;
                foreach (ucQuestion item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Visible);
                }
                lblCurrPage.Content = CurrPageNo.ToString("D2");
            }
            else
            {
                //Navigation();
                if (prevPage != null)
                {
                    NavigationPrev();
                }
            }

            if (CurrPageNo == 1)
            {
                if (prevPage == null)
                {
                    btnPrev.Visibility = Visibility.Collapsed;
                }
                else
                {
                    btnPrev.Visibility = Visibility.Visible;
                }
            }
        }

        private void NavigationNext()
        {
            CLIPSController.assertNextSection();

            int sectionID = CLIPSController.getCurrentQnGroupID();
            
            frmSection nextSection = new frmSection(amdesPageFrame, sectionID, this);
            
            amdesPageFrame.Navigate(nextSection);
        }

        private void NavigationPrev()
        {
            if (prevPage != null)
            {
                amdesPageFrame.Navigate(prevPage);
            }
        }
    }
}
