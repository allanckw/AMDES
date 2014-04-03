﻿using System;
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

        public frmRecommendationViewOnly(Frame amdesFrame, History h)
        {
            InitializeComponent();
            //lblSection.Content = sectionID;
            //lastSection = lastSec;
            amdesPageFrame = amdesFrame;
            //prevPage = null;
            //btnPrev.Visibility = Visibility.Collapsed;
            //heightLimit = 430;
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
        //public frmRecommendationViewOnly(Frame amdesFrame, List<Diagnosis> diaList, AMDESPage lastSec)
        //{
        //    InitializeComponent();
        //    //lblSection.Content = sectionID;
        //    lastSection = lastSec;
        //    amdesPageFrame = amdesFrame;
        //    //prevPage = null;
        //    //btnPrev.Visibility = Visibility.Collapsed;
        //    //heightLimit = 430;
        //    Patient currPatient = CLIPSController.CurrentPatient;
        //    lblPatientID.Content = currPatient.NRIC;
        //    lblPatientName.Content = currPatient.Last_Name + " " + currPatient.First_Name;
        //    lblPatientAge.Content = "Age : " + currPatient.getAge();
        //    if (CLIPSController.savePatient == false)
        //    {
        //        lblPatientID.Visibility = Visibility.Collapsed;
        //        lblPatientName.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        lblPatientID.Visibility = Visibility.Visible;
        //        lblPatientName.Visibility = Visibility.Visible;
        //    }
        //    AllDiagnose = diaList;
        //    PageContent = new List<List<ucDiagnosis>>();
        //    loadRecommendation();

        //}

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
            //int sectionID = CLIPSController.getCurrentQnGroupID();
            //frmSection lastSection = new frmSection(amdesPageFrame, sectionID);

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

        //public void addSymptons()
        //{
        //    StackPanel stkpnlSymptons = new StackPanel();

        //    foreach (Symptom sym in CLIPSController.CurrentPatient.getLatestHistory().SymptomsList)
        //    {
        //        Label lblSymptons = new Label();
        //        lblSymptons.Content = "Symptoms - " + sym.SymptomName;
        //        stkpnlSymptons.Children.Add(lblSymptons);
        //    }
        //    //if (stkpnlSymptons.Children.Count == 0)
        //    //{
        //    //    lblSymptonsText.Content = "The patient has no symptoms.";
        //    //}
        //    //updateHeight();
        //}
        //private void sortPage()
        //{
        //    currHeight = 0;
        //    List<ucDiagnosis> QuestionPerPage = new List<ucDiagnosis>();
        //    foreach (ucDiagnosis item in PageFrame.Children)
        //    {
        //        string temp = item.Name;
        //        currHeight += Math.Ceiling(item.getHeight());

        //        if (currHeight >= heightLimit)
        //        {
        //            currHeight = 0;
        //            currHeight += Math.Ceiling(item.getHeight());
        //            PageContent.Add(QuestionPerPage);
        //            QuestionPerPage = new List<ucDiagnosis>();
        //            if (collapseRest == false)
        //            {
        //                collapseRest = true;
        //            }
        //        }

        //        if (collapseRest)
        //        {
        //            item.setVisibility(Visibility.Collapsed);
        //        }

        //        QuestionPerPage.Add(item);
        //    }

        //    if (QuestionPerPage.Count > 0)
        //    {
        //        PageContent.Add(QuestionPerPage);
        //    }

        //    TotalPageNo = PageContent.Count;
        //    lblTotalPage.Content = PageContent.Count.ToString();
        //    btnPrev.Visibility = Visibility.Hidden;
        //    if (TotalPageNo == 1)
        //    {
        //        btnNext.Visibility = Visibility.Hidden;
        //    }
        //}

        //private void btnNext_Click(object sender, RoutedEventArgs e)
        //{
        //    btnPrev.Visibility = Visibility.Visible;
        //    if (CurrPageNo != TotalPageNo)
        //    {
        //        foreach (ucDiagnosis item in PageContent.ElementAt(CurrPageNo - 1))
        //        {
        //            item.setVisibility(Visibility.Collapsed);
        //        }

        //        CurrPageNo++;
        //        foreach (ucDiagnosis item in PageContent.ElementAt(CurrPageNo - 1))
        //        {
        //            item.setVisibility(Visibility.Visible);
        //        }
               
        //        if (CurrPageNo == TotalPageNo)
        //            btnNext.Visibility = Visibility.Hidden;
        //    }
        //}

        //private void btnPrev_Click(object sender, RoutedEventArgs e)
        //{
        //    btnNext.Visibility = Visibility.Visible;
        //    if (CurrPageNo != 1)
        //    {
        //        foreach (ucDiagnosis item in PageContent.ElementAt(CurrPageNo - 1))
        //        {
        //            item.setVisibility(Visibility.Collapsed);
        //        }

        //        CurrPageNo--;
        //        foreach (ucDiagnosis item in PageContent.ElementAt(CurrPageNo - 1))
        //        {
        //            item.setVisibility(Visibility.Visible);
        //        }
        //        lblCurrPage.Content = CurrPageNo.ToString();
        //        if (CurrPageNo == 1)
        //            btnPrev.Visibility = Visibility.Hidden;

        //    }
        //    else
        //    {
        //        btnPrev.Visibility = Visibility.Hidden;
        //    }
        //}
    }
}