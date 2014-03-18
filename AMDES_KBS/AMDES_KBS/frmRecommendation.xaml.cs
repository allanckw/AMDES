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

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmRecommendation.xaml
    /// </summary>
    public partial class frmRecommendation : AMDESPage
    {
        double heightLimit = 0;
        double currHeight = 0;
        int CurrPageNo = 1;
        int TotalPageNo = 1;

        //Frame amdesPageFrame;
        List<List<ucDiagnosis>> PageContent = new List<List<ucDiagnosis>>();
        bool collapseRest = false;

        public frmRecommendation()
        {
            InitializeComponent();
            //amdesPageFrame = amdesFrame;
            heightLimit = 430;
            loadRecommendation();
        }

        public void loadRecommendation()
        {
            for (int i = 0; i < 7; i++)
            {
                ucDiagnosis diagnosis = new ucDiagnosis("R" + (i+1));
                //diagnosis.addSymptoms(i+1);
                PageFrame.Children.Add(diagnosis);
            }
            sortPage();
        }

        private void sortPage()
        {
            currHeight = 0;
            List<ucDiagnosis> QuestionPerPage = new List<ucDiagnosis>();
            foreach (ucDiagnosis item in PageFrame.Children)
            {
                string temp = item.Name;
                currHeight += Math.Ceiling(item.getHeight());

                if (currHeight >= heightLimit)
                {
                    currHeight = 0;
                    currHeight += Math.Ceiling(item.getHeight());
                    PageContent.Add(QuestionPerPage);
                    QuestionPerPage = new List<ucDiagnosis>();
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
            lblTotalPage.Content = PageContent.Count.ToString();
            btnPrev.Visibility = Visibility.Hidden;
            if (TotalPageNo == 1)
            {
                btnNext.Visibility = Visibility.Hidden;
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            btnPrev.Visibility = Visibility.Visible;
            if (CurrPageNo != TotalPageNo)
            {
                foreach (ucDiagnosis item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Collapsed);
                }

                CurrPageNo++;
                foreach (ucDiagnosis item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Visible);
                }
                lblCurrPage.Content = CurrPageNo.ToString();
                if (CurrPageNo == TotalPageNo)
                    btnNext.Visibility = Visibility.Hidden;
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            btnNext.Visibility = Visibility.Visible;
            if (CurrPageNo != 1)
            {
                foreach (ucDiagnosis item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Collapsed);
                }

                CurrPageNo--;
                foreach (ucDiagnosis item in PageContent.ElementAt(CurrPageNo - 1))
                {
                    item.setVisibility(Visibility.Visible);
                }
                lblCurrPage.Content = CurrPageNo.ToString();
                if (CurrPageNo == 1)
                    btnPrev.Visibility = Visibility.Hidden;

            }
            else
            {
                btnPrev.Visibility = Visibility.Hidden;
            }
        }
    }
}
