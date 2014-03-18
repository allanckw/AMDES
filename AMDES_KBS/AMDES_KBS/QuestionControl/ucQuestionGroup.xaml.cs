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
    /// Interaction logic for ucQuestionGroup.xaml
    /// </summary>
    public partial class ucQuestionGroup : UserControl
    {
        QuestionGroup qgData;
        QuestionCountGroup qcgData;
        QuestionType qType;
        bool newGroup = false;

        public ucQuestionGroup(int questionTypeENUM)
        {
            InitializeComponent();
            newGroup = true;
            qgData = new QuestionGroup();
            qgData.setQuestionType(questionTypeENUM);

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
            }
            else
            {
                txtHeader.Text = qgData.Header;
                txtDesc.Text = qgData.Description.Replace("~~", Environment.NewLine);
                txtSymptom.Text = qgData.Symptom;
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
            qcgData.Threshold = int.Parse(txtThreshold.Text.Trim());
            qcgData.MaxQuestions = int.Parse(this.txtMaxQn.Text.Trim());
            return qcgData;
        }

        public QuestionType getQuestionType()
        {
            return qType;
        }
    }
}
