using System;
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
    /// Interaction logic for frmSection.xaml
    /// </summary>
    public partial class frmSectionViewOnly : AMDESPage
    {
        double heightLimit = 0;
        double currHeight = 0;
        int CurrPageNo = 1;
        int TotalPageNo = 1;

        Frame amdesPageFrame;
        QuestionType sectionType;
        QuestionGroup QG;
        AMDESPage prevPage;
        AMDESPage nextPage;

        List<List<ucQuestionViewOnly>> PageContent = new List<List<ucQuestionViewOnly>>();
        List<ucQuestionViewOnly> WholeContent = new List<ucQuestionViewOnly>();

        bool collapseRest = false;

        public frmSectionViewOnly(Frame amdesFrame, int sectionID, List<QnHistory> hisList)
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
            setAns(hisList);

           
        }

        public void loadSection(int sectionID)
        {
            heightLimit = this.QuestionFrame.Height;
            WholeContent = new List<ucQuestionViewOnly>();
            QG = QuestionController.getGroupByID(sectionID);
            sectionType = QG.getQuestionTypeENUM();
            stkpnlScore.Visibility = Visibility.Collapsed;
            if (this.sectionType == QuestionType.COUNT)
            {
                stkpnlScore.Visibility = Visibility.Visible;
                QuestionCountGroup QCG = (QuestionCountGroup)QG;
                lblCurrScore.Content = 0;
                lblCurrScore.Tag = QCG.Threshold;
                lblTotalScore.Content = QCG.MaxQuestions;
                heightLimit -= 11;
            }

            lblSection.Content = QG.Header.Trim();
            txtDesc.Text = QG.Description.Replace("~~", Environment.NewLine).Trim();

            var desiredSizeOld = txtDesc.DesiredSize;
            txtDesc.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var desiredSizeNew = txtDesc.DesiredSize;
            txtDesc.Height = desiredSizeNew.Height;
            
            //MessageBox.Show(txtDesc.Height.ToString());
            heightLimit += (60 - txtDesc.Height);
            QuestionFrame.Height = heightLimit;

            foreach (Question q in QG.Questions)
            {
                addQuestion(q);
            }
            sortPageAndShowLast();
        }


        void offCommands(double offset)
        {
            
            if (CLIPSController.savePatient == false) //turn off saving when anon mode
            {

                if (btnPrev.Visibility != Visibility.Visible)
                {
                    offset += btnPrev.Width + 10;
                }
                
                lblPatientName.Content = lblPatientID.Content = "";
            }
            else
            {
                if (btnPrev.Visibility != Visibility.Visible)
                {
                    offset += btnPrev.Width + 10;
                }
                
                lblPatientID.Content = CLIPSController.CurrentPatient.NRIC;
                lblPatientName.Content = CLIPSController.CurrentPatient.Last_Name + " " + CLIPSController.CurrentPatient.First_Name;


            }
        }


        public void addQuestion(Question q)
        {
            ucQuestionViewOnly ucQ = new ucQuestionViewOnly();
            if (sectionType == QuestionType.COUNT)
            {
                ucQ.loadQuestion(q, QG.GroupID, lblCurrScore);
            }
            else
            {
                //ucQ.Name="Question" + q.ID;
                ucQ.loadQuestion(q, QG.GroupID, null);
            }
            //currHeight += Math.Ceiling(ucQ.getHeight());
            QuestionFrame.Children.Add(ucQ);
            WholeContent.Add(ucQ);
        }

        private void sortPageAndShowLast()
        {
            currHeight = 0;
            bool singlePage = false;

            List<ucQuestionViewOnly> QuestionPerPage = new List<ucQuestionViewOnly>();
            foreach (ucQuestionViewOnly item in QuestionFrame.Children)
            {
                string temp = item.Name;
                currHeight += Math.Ceiling(item.getHeight());

                //to make it single page
                if (singlePage)
                {
                    currHeight = 0;
                    PageContent.Add(QuestionPerPage);
                    //QuestionPerPage.Add(QuestionPerPage);
                    QuestionPerPage = new List<ucQuestionViewOnly>();
                    if (collapseRest == false)
                    {
                        collapseRest = true;
                    }
                    singlePage = false;
                }

                //check for image to set single page
                if (item.getHaveImage())//Image - single page
                {
                    currHeight = 0;
                    PageContent.Add(QuestionPerPage);
                    QuestionPerPage = new List<ucQuestionViewOnly>();
                    if (collapseRest == false)
                    {
                        collapseRest = true;
                    }
                    singlePage = true;
                }

                if (currHeight >= heightLimit)
                {
                    currHeight = 0;
                    currHeight += Math.Ceiling(item.getHeight());
                    PageContent.Add(QuestionPerPage);
                    QuestionPerPage = new List<ucQuestionViewOnly>();
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
            //CurrPageNo = TotalPageNo;
            lblTotalPage.Content = PageContent.Count.ToString("D2");
            while (CurrPageNo < TotalPageNo)
            {
                foreach (ucQuestionViewOnly item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Collapsed);
                }

                CurrPageNo++;
                foreach (ucQuestionViewOnly item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Visible);
                }
                lblCurrPage.Content = CurrPageNo.ToString("D2");
            }
            if (CurrPageNo > 1)
            {
                btnPrev.Visibility = Visibility.Visible;
            }
        }

        private void setAns(List<QnHistory> hisList)
        {
            for (int i = 0; i < WholeContent.Count; i++)
            {
                
                WholeContent[i].setAnswer(hisList[i].Answer);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (CurrPageNo != TotalPageNo)
            {
                foreach (ucQuestionViewOnly item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Collapsed);
                }

                CurrPageNo++;
                foreach (ucQuestionViewOnly item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Visible);
                }
                //JK changes
                btnPrev.Visibility = Visibility.Visible;
                lblCurrPage.Content = CurrPageNo.ToString("D2");
            }
            else
            {
                NavigationNext();
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            btnPrev.Visibility = Visibility.Visible;

            if (CurrPageNo != 1)
            {
                foreach (ucQuestionViewOnly item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Collapsed);
                }

                CurrPageNo--;
                foreach (ucQuestionViewOnly item in PageContent.ElementAt(CurrPageNo - 1))
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
                    offCommands(390);
                }
                else
                {
                    btnPrev.Visibility = Visibility.Visible;
                    offCommands(390);
                }
            }
            else
            {
                btnPrev.Visibility = Visibility.Visible;
                offCommands(390);
            }
        }

        private void NavigationNext()
        {
            if (nextPage == null)
            {
                amdesPageFrame.Navigate(null);
            }
            else
            {
                amdesPageFrame.Navigate(nextPage);
            }
        }

        private void NavigationPrev()
        {
            if (prevPage != null)
            {
                amdesPageFrame.Navigate(prevPage);
            }
        }

        public void setPrevPage(frmSectionViewOnly prevSection)
        {
            this.prevPage = prevSection;
            this.btnPrev.Visibility = Visibility.Visible;
            offCommands(390);
        }

        public void setNextPage(AMDESPage nextSection)
        {
            this.nextPage = nextSection;
            offCommands(390);
            //this.btnPrev.Visibility = Visibility.Visible;
        }
    }
}
