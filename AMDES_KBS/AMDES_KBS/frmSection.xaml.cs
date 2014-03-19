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

        frmSection prevPage;

        List<List<ucQuestion>> PageContent = new List<List<ucQuestion>>();
        List<ucQuestion> WholeContent = new List<ucQuestion>();

        bool collapseRest = false;
        public frmSection(Frame amdesFrame, int sectionID)
        {
            InitializeComponent();
            lblSection.Content = sectionID;
            amdesPageFrame = amdesFrame;
            heightLimit = 430;
            Patient currPatient = CLIPSController.CurrentPatient;
            lblPatientID.Content =  currPatient.NRIC;
            lblPatientName.Content = currPatient.Last_Name + " " + currPatient.First_Name;
            loadSection(sectionID);
        }

        public frmSection(Frame amdesFrame, int sectionID, frmSection prevSection)
        {
            InitializeComponent();
            amdesPageFrame = amdesFrame;
            heightLimit = 430;
            Patient currPatient = CLIPSController.CurrentPatient;
            lblPatientID.Content = currPatient.NRIC;
            lblPatientName.Content = currPatient.Last_Name + " " + currPatient.First_Name;
            loadSection(sectionID);
        }

        public void loadSection(int sectionID)
        {
            WholeContent = new List<ucQuestion>();
            QuestionGroup qlist = QuestionController.getGroupByID(sectionID);
            lblSection.Content = qlist.Header;
            txtDesc.Text = qlist.Description.Replace("~~", Environment.NewLine);
            foreach (Question q in qlist.Questions)
            {
                addQuestion(q);
            }
            sortPage();
        }

        public void addQuestion(Question q)
        {
            ucQuestion ucQ = new ucQuestion();
            //ucQ.Name="Question" + q.ID;
            ucQ.loadQuestion(q);
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

            if (QuestionPerPage.Count>0)
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
            }
        }

        private void NavigationNext()
        {

        }
    }
}
