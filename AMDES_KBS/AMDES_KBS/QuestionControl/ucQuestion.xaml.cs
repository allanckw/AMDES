using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucQuestion.xaml
    /// </summary>
    public partial class ucQuestion : UserControl
    {
        bool questionAnswer = false;
        int gid;
        Question question;
        Label scoringData;

        public ucQuestion()
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
            CLIPSController.assertQuestion(gid, question.ID, answer);
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
            if ((sender as ToggleButton).IsChecked == true)
            {
                CLIPSController.assertQuestion(gid, question.ID, true);
                if (scoringData != null)
                {
                    int score = int.Parse(scoringData.Content.ToString());
                    score++;
                    scoringData.Content = score;
                }
                questionAnswer = true;
                Thread.Sleep(100);
            }
            else
            {
                CLIPSController.assertQuestion(gid, question.ID, false);
                if (scoringData != null)
                {
                    int score = int.Parse(scoringData.Content.ToString());
                    if (questionAnswer)
                    {
                        score--;
                        scoringData.Content = score;
                    }
                    questionAnswer = false;
                    Thread.Sleep(100);
                }
            }
           
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
