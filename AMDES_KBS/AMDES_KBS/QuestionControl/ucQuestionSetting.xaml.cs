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

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucQuestion.xaml
    /// </summary>
    public partial class ucQuestionSetting : UserControl
    {
        Question question;

        public ucQuestionSetting()
        {
            InitializeComponent();
            lblQuestion.Content = "New";
            question = new Question();
        }

        public ucQuestionSetting(Question q)
        {
            InitializeComponent();
            loadQuestion(q);
        }

        public void loadQuestion(Question q)
        {
            question = q;
            string questionText = q.Name.Replace("~~", Environment.NewLine);
            lblQuestion.Content = q.ID;
            txtSympton.Text = q.Symptom;
            txtQuestion.Text = questionText;            
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            this.stkpnlQuestion.Visibility = Visibility.Collapsed;
        }

        public Question getQuestion()
        {
            question.Name = txtQuestion.Text.Replace(Environment.NewLine, "~~");
            question.Symptom = txtSympton.Text;
            return question;
        }

        public bool getToSaved()
        {
            if (txtQuestion.Text.Trim() == "")
                return false;
            if (stkpnlQuestion.Visibility == Visibility.Collapsed)
                return false;

            return true;
        }
    }
}
