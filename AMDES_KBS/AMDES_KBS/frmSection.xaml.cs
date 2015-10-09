using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;
using System.Windows.Input;

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
            //heightLimit = 470;
            Patient currPatient = CLIPSController.CurrentPatient;
            loadSection(sectionID);
            offCommands(240);

        }

        public frmSection(Frame amdesFrame, int sectionID, List<QnHistory> hisList)
        {
            InitializeComponent();
            lblSection.Content = sectionID;
            amdesPageFrame = amdesFrame;
            prevPage = null;
            //heightLimit = 470;
            loadSection(sectionID);
            setAns(hisList);
            setLastPage();
            offCommands(240);
            if (CurrPageNo == 1)
                btnPrev.Visibility = Visibility.Hidden;
            else
                btnPrev.Visibility = Visibility.Visible;
        }


        public frmSection(Frame amdesFrame, int sectionID, frmSection prevSection)
        {
            InitializeComponent();
            amdesPageFrame = amdesFrame;
            prevPage = prevSection;
            btnPrev.Visibility = Visibility.Visible;
            //heightLimit = 470;
            loadSection(sectionID);
            offCommands(240);
        }

        void offCommands(double offset)
        {
            if (CLIPSController.savePatient == false) //turn off saving when anon mode
            {
                btnSave.Visibility = Visibility.Collapsed;
                if (btnPrev.Visibility == Visibility.Visible)
                    offset -= btnPrev.Width + 10;

                //zz.Margin = new Thickness(0, 0, offset, 0);
                lblPatientName.Content = lblPatientID.Content = "";
            }
            else
            {
                offset -= 110;
                if (btnPrev.Visibility == Visibility.Visible)
                    offset -= btnPrev.Width + 10;

                //zz.Margin = new Thickness(0, 0, offset, 0);
                lblPatientID.Content = CLIPSController.CurrentPatient.NRIC;
                lblPatientName.Content = CLIPSController.CurrentPatient.Last_Name + " " + CLIPSController.CurrentPatient.First_Name;


            }

            this.Cursor = Cursors.Pen;
        }

        public void loadSection(int sectionID)
        {
            heightLimit = this.QuestionFrame.Height;
            WholeContent = new List<ucQuestion>();
            QG = QuestionController.getGroupByID(sectionID);
            sectionType = QG.getQuestionTypeENUM();
            stkpnlScore.Visibility = Visibility.Collapsed;
            if (this.sectionType == QuestionType.COUNT)
            {
                stkpnlScore.Visibility = Visibility.Visible;
                QuestionCountGroup QCG = (QuestionCountGroup)QG;

                lblCurrScore.Content = 0;
                lblCurrScore.Tag = QCG.Threshold;
                lblTotalScore.Content = QCG.MaximumScore;
                heightLimit -= 11;
            }

            lblSection.Content = QG.Header;
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
            sortPage();
        }

        public void addQuestion(Question q)
        {
            ucQuestion ucQ = new ucQuestion();
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

        private void sortPage()
        {
            int test = 0; //for forcing qn 3 to have image
            currHeight = 0;
            bool singlePage = false;
            List<ucQuestion> QuestionPerPage = new List<ucQuestion>();
            foreach (ucQuestion item in QuestionFrame.Children)
            {
                test++;
                //Force Set Question 3
                if (test==3)
                {
                    //loading image
                    item.loadImage("D:\\Users\\sisajk\\Desktop\\2015-09-23_233400.jpg");
                }

                string temp = item.Name;
                currHeight += Math.Ceiling(item.getHeight());

                //to make it single page
                if (singlePage)
                {
                    currHeight = 0;
                    PageContent.Add(QuestionPerPage);
                    //QuestionPerPage.Add(QuestionPerPage);
                    QuestionPerPage = new List<ucQuestion>();
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
                    QuestionPerPage = new List<ucQuestion>();
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
                foreach (ucQuestion item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Collapsed);
                }

                CurrPageNo++;
                foreach (ucQuestion item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Visible);
                }
                btnPrev.Visibility = Visibility.Visible;
                lblCurrPage.Content = CurrPageNo.ToString("D2");
            }
            else
            {
                NavigationNext();
            }
        }

        private void setLastPage()
        {
            while (CurrPageNo < TotalPageNo)
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
                else
                {
                    btnPrev.Visibility = Visibility.Hidden;
                }
            }

            if (CurrPageNo == 1)
            {
                if (prevPage == null)
                {
                    btnPrev.Visibility = Visibility.Hidden;
                }
                else
                {
                    btnPrev.Visibility = Visibility.Visible;

                }
            }
        }

        private void NavigationNext()
        {
            foreach (ucQuestion qn in this.WholeContent)
            {
                CLIPSController.assertQuestion(QG.GroupID, qn.getQuestion().ID, qn.getAnswer(), qn.getQuestion().isNegation);
                Thread.Sleep(50);
            }

            CLIPSController.assertNextSection();

            int sectionID = CLIPSController.getCurrentQnGroupID();
            if (sectionID == -1)
            {
                CLIPSController.getResultingDiagnosis();
                List<Diagnosis> result = CLIPSController.CurrentPatient.getLatestHistory().Diagnoses;
                frmRecommendation finalConclusionPage = new frmRecommendation(amdesPageFrame, result, this);
                amdesPageFrame.Navigate(finalConclusionPage);
            }
            else
            {
                //MessageBox.Show(sectionID.ToString());

                if (lblSection.Content.ToString().CompareTo(sectionID.ToString()) == 0)
                {
                    ////Warning Stuck Condition, this freaking msgbox should never come out in our testing phase if all the rules are defined correctly.
                    //MessageBox.Show("The Rules did not cover the sequence of diagnosis defined in the system" + Environment.NewLine +
                    //    "please verify your rules in the configuration, " + Environment.NewLine + " if the problem persists, please contact the system administrator of this system with the data folder in the root folder of this program and we will assist you " +
                    //    Environment.NewLine + " at amdes_nus_soc@googlegroups.com ", "Error In Navigation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    frmSection nextSection = new frmSection(amdesPageFrame, sectionID, this);
                    amdesPageFrame.Navigate(nextSection);
                }


            }
        }

        private void NavigationPrev()
        {
            if (prevPage != null)
            {
                foreach (ucQuestion qn in this.WholeContent)
                {
                    CLIPSController.assertQuestion(QG.GroupID, qn.getQuestion().ID, false, qn.getQuestion().isNegation);
                    Thread.Sleep(50);
                }
                CLIPSController.assertPrevSection();
                int sectionID = CLIPSController.getCurrentQnGroupID();
                //u can use prevPage to navigate back, there will be no effect on clips, 
                //the effect on clips will be invisible to the user
                amdesPageFrame.Navigate(prevPage);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Warning: only last session saved!!!
            CLIPSController.saveAssertLog();
            CLIPSController.saveCurrentNavex();
            MessageBox.Show("History have been saved", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void setPrevPage(frmSection prevSection)
        {
            this.prevPage = prevSection;
            this.btnPrev.Visibility = Visibility.Visible;
        }
    }
}
