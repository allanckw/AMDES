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
using System.Threading;
using System.Windows.Controls.Primitives;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucQuestion.xaml
    /// </summary>
    public partial class ucQuestionViewOnly : UserControl
    {
        bool questionAnswer = false;
        int gid;
        Question question;
        Label scoringData;

        public ucQuestionViewOnly()
        {
            InitializeComponent();
        }

        public void loadQuestion(Question q, int gid, Label lblscore)
        {
            question = q;
            this.gid = gid;
            string questionText = q.Name.Replace("~~", Environment.NewLine);

            lblQuestion.Content = q.ID;
            txtQuestion.Text = questionText;
            var desiredSizeOld = txtQuestion.DesiredSize;
            txtQuestion.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var desiredSizeNew = txtQuestion.DesiredSize;
            txtQuestion.Height = desiredSizeNew.Height+10;
            if (lblscore != null)
            {
                scoringData = lblscore;
            }
            
        }

        public void setAnswer(bool answer)
        {
            questionAnswer = answer;
            btnYes.IsChecked = answer;
            if (answer)
            {
                if (scoringData != null)
                {
                    int score = int.Parse(scoringData.Content.ToString());
                    score++;
                    scoringData.Content = score;
                }
            }
        }

        //Allan Note: ClipsController Point 1 Integration Done
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            (sender as ToggleButton).IsChecked = questionAnswer;        
        }

        public bool getAnswer()
        {
            return questionAnswer;
        }

        public Question getQuestion()
        {
            return question;
        }

        public double getHeight()
        {

            return txtQuestion.Height;
        }

        public void setVisibility(Visibility v)
        {
            stkpnlQuestion.Visibility = v;
        }
    }
}
