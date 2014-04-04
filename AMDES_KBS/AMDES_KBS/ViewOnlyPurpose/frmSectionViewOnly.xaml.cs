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

            if (CLIPSController.savePatient == false) //turn off saving when anon mode
            {
                lblPatientName.Content = lblPatientID.Content = "";
            }
            else
            {
                lblPatientID.Content = CLIPSController.CurrentPatient.NRIC;
                lblPatientName.Content = CLIPSController.CurrentPatient.Last_Name + " " + CLIPSController.CurrentPatient.First_Name;
            }
        }

        public void loadSection(int sectionID)
        {
            WholeContent = new List<ucQuestionViewOnly>();
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
            sortPageAndShowLast();
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
            List<ucQuestionViewOnly> QuestionPerPage = new List<ucQuestionViewOnly>();
            foreach (ucQuestionViewOnly item in QuestionFrame.Children)
            {
                string temp = item.Name;
                currHeight += Math.Ceiling(item.getHeight());

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

                }
                else
                {
                    btnPrev.Visibility = Visibility.Visible;
                }
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
        }

        public void setNextPage(AMDESPage nextSection)
        {
            this.nextPage = nextSection;
            //this.btnPrev.Visibility = Visibility.Visible;
        }
    }
}
