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
        bool newGroup = false;
        public ucQuestionGroup()
        {
            InitializeComponent();
            newGroup = true;
        }

        public ucQuestionGroup(QuestionGroup qg)
        {
            InitializeComponent();
            this.qgData = qg;
            newGroup = false;
            loadQuestionGroupData();
        }

        private void loadQuestionGroupData()
        {
            txtHeader.Text=qgData.Header;
            txtDesc.Text=qgData.Description;
            txtSymptom.Text=qgData.Symptom;
        }

        public QuestionGroup getQuestionGroup()
        {
            QuestionGroup tempQG;
            if (newGroup)
            {
                tempQG = new QuestionGroup();
                tempQG.GroupID = QuestionController.getNextGroupID();
            }
            else
            {
                tempQG = qgData;
            }
            tempQG.Header = txtHeader.Text;
            tempQG.Description = txtDesc.Text.Replace(Environment.NewLine, "~~");
            tempQG.Symptom = txtSymptom.Text;

            return tempQG;
        }
    }
}
