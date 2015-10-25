﻿using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucQuestionGroup.xaml
    /// </summary>
    public partial class ucQuestionGroup : UserControl
    {
        QuestionGroup qgData;
        QuestionCountGroup qcgData;
        QuestionType qType;
        bool newGroup = false;

        public int MaxQuestions
        {
            get { return int.Parse(txtMaxQn.Text.Trim()); }
        }
  

        public ucQuestionGroup(int questionTypeENUM)
        {
            InitializeComponent();
            newGroup = true;
            qgData = new QuestionGroup();
            qgData.setQuestionType(questionTypeENUM);
            qType = qgData.getQuestionTypeENUM();

            if (qgData.getQuestionTypeENUM()==QuestionType.COUNT)
            {
                qcgData = new QuestionCountGroup();
                qcgData.setQuestionType(questionTypeENUM);
                stkpnlCOUNT.Visibility = Visibility.Visible;
            }
        }

        public ucQuestionGroup(QuestionGroup qg)
        {
            InitializeComponent();
            this.qgData = qg;
            newGroup = false;
            stkpnlCOUNT.Visibility = Visibility.Collapsed;
            qType = qg.getQuestionTypeENUM();
            loadQuestionGroupData();
        }

        public ucQuestionGroup(QuestionCountGroup qcg)
        {
            InitializeComponent();
            this.qcgData = qcg;
            newGroup = false;
            stkpnlCOUNT.Visibility = Visibility.Visible;
            qType = qcg.getQuestionTypeENUM();
            loadQuestionGroupData();
        }

        private void loadQuestionGroupData()
        {

            if (this.getQuestionType() == QuestionType.COUNT)
            {
                txtHeader.Text = qcgData.Header;
                txtDesc.Text = qcgData.Description.Replace("~~",Environment.NewLine);
                txtSymptom.Text = qcgData.Symptom;
                txtThreshold.Text = qcgData.Threshold.ToString();
                txtMaxQn.Text = qcgData.MaxQuestions.ToString();
                chkNegate.IsChecked = qcgData.isNegation;
            }
            else
            {
                txtHeader.Text = qgData.Header;
                txtDesc.Text = qgData.Description.Replace("~~", Environment.NewLine);
                txtSymptom.Text = qgData.Symptom;
                chkNegate.IsChecked = qgData.isNegation;
            }
        }

        public QuestionGroup getQuestionGroup()
        {
            if (newGroup)
            {
                qgData.GroupID = QuestionController.getNextGroupID();
            }

            qgData.Header = txtHeader.Text;
            qgData.Description = txtDesc.Text.Replace(Environment.NewLine, "~~");
            qgData.Symptom = txtSymptom.Text;
            qgData.isNegation = chkNegate.IsChecked.Value;
            return qgData;
        }

        public QuestionCountGroup getQuestionCountGroup()
        {
            if (newGroup)
            {
                qcgData.GroupID = QuestionController.getNextGroupID();
            }

            qcgData.Header = txtHeader.Text;
            qcgData.Description = txtDesc.Text.Replace(Environment.NewLine, "~~");
            qcgData.Symptom = txtSymptom.Text;
            qcgData.Threshold = int.Parse(txtThreshold.Text.ToString());
            qcgData.MaxQuestions = int.Parse(this.txtMaxQn.Text.Trim());

            qcgData.isNegation = chkNegate.IsChecked.Value;

            return qcgData;
        }

        public QuestionType getQuestionType()
        {
            return qType;
        }

        private void txtMaxQn_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = App.NumberValidationTextBox(e.Text);
        }

        private void txtThreshold_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = App.NumberValidationTextBox(e.Text);
        }


    }
}
