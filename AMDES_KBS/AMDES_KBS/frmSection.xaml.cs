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

        public void loadSection(int sectionID)
        {
            WholeContent = new List<ucQuestion>();
            QuestionGroup qlist = QuestionController.getGroupByID(sectionID);
            foreach (Question q in qlist.Questions)
            {
                addQuestion(q);
            }
            //for (int i = 0; i < 100; i++)
            //{
            //    addQuestion((i + 1).ToString(), randomQuestion());
            //}
            ////q1.loadQuestion("A1",tempquestion);
            sortPage();
        }

        //public string randomQuestion()
        //{
        //    int no = new Random().Next(3);
        //    string tempquestion = "";
        //    switch (no)
        //    {
        //        case 1:
        //            tempquestion = "Forget previously learned material (Initially with recent material lost), or impaired ability to learn new material or both\n";
        //            tempquestion = tempquestion + "Compared to before, does the patient have problems remembering things/events that happened recently?\n";
        //            tempquestion = tempquestion + "Frequently misplacing his/her personal belongings?\n";
        //            tempquestion = tempquestion + "Forget how to carry out basic tasks? (such as turning off the lights/fan, making a drink, how to get assistance, etc)\n";
        //            tempquestion = tempquestion + "Forget where he/she keeps his/her things?\n";
        //            tempquestion = tempquestion + "Forget what he/she has come to do?\n";
        //            tempquestion = tempquestion + "Have problems finding his/her way around ?\n";
        //            tempquestion = tempquestion + "Ask repeated questions or repeat himself/herself often?\n";
        //            tempquestion = tempquestion + "Other Q:__________________________________\n";
        //            break;
        //        case 2:
        //            tempquestion = "Forget previously learned material (Initially with recent material lost), or impaired ability to learn new material or both\n";
        //            tempquestion = tempquestion + "Other Q:__________________________________\n";
        //            break;
        //        case 3:
        //            tempquestion = "Forget previously learned material (Initially with recent material lost), or impaired ability to learn new material or both\n";
        //            tempquestion = tempquestion + "Compared to before, does the patient have problems remembering things/events that happened recently?\n";
        //            tempquestion = tempquestion + "Frequently misplacing his/her personal belongings?\n";
        //            tempquestion = tempquestion + "Forget how to carry out basic tasks? (such as turning off the lights/fan, making a drink, how to get assistance, etc)\n";
        //            tempquestion = tempquestion + "Other Q:__________________________________\n";
        //            break;
        //        default:
        //            tempquestion = tempquestion + "Other Q:__________________________________\n";
        //            break;
        //    }

        //    return tempquestion;
        //}

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
            lblTotalPage.Content = PageContent.Count.ToString();
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
                lblCurrPage.Content = CurrPageNo.ToString();
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
                lblCurrPage.Content = CurrPageNo.ToString();
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
