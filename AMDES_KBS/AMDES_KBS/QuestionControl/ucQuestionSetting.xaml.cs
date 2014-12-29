using System;
using System.Windows;
using System.Windows.Controls;
using AMDES_KBS.Entity;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucQuestion.xaml
    /// </summary>
    public partial class ucQuestionSetting : UserControl
    {
        Question question;

        public ucQuestionSetting(bool isScoreType)
        {
            InitializeComponent();
            lblQuestion.Content = "New";
            question = new Question();

            if (!isScoreType)
                lblScore.Visibility = Visibility.Collapsed;

            txtScore.Visibility = lblScore.Visibility;
        }

        public ucQuestionSetting(Question q, bool isScoreType )
        {
            InitializeComponent();
            
            if (!isScoreType)
                lblScore.Visibility = Visibility.Collapsed;

            txtScore.Visibility = lblScore.Visibility;
            loadQuestion(q);
        }

        public void loadQuestion(Question q)
        {
            question = q;
            string questionText = q.Name.Replace("~~", Environment.NewLine);
            lblQuestion.Content = q.ID;
            txtSymptom.Text = q.Symptom;
            txtQuestion.Text = questionText;
            txtScore.Text = q.Score.ToString();
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            this.stkpnlQuestion.Visibility = Visibility.Collapsed;
        }

        public Question getQuestion()
        {
            question.Name = txtQuestion.Text.Replace(Environment.NewLine, "~~");
            question.Symptom = txtSymptom.Text;
            question.Score = int.Parse(txtScore.Text.Trim());
            return question;
        }

        public bool getToSaved(bool ignoreEmptyText)
        {
            if (!ignoreEmptyText)
            {
                if (txtQuestion.Text.Trim() == "")
                    return false;
            }

            if (stkpnlQuestion.Visibility == Visibility.Collapsed)
                return false;

            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
