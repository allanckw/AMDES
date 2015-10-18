using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using AMDES_KBS.Entity;
using System.Windows.Media.Imaging;
using AMDES_KBS.Controllers;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucQuestion.xaml
    /// </summary>
    public partial class ucQuestionViewOnly : UserControl
    {
        bool answer;
        int gid;
        Question question;
        Label scoringData;
        bool havImage = false;

        public ucQuestionViewOnly()
        {
            InitializeComponent();
        }

        private void loadImage(String url)
        {
            havImage = true;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(url);
            image.EndInit();
            imgPicture.Source = image;
            imgPicture.Stretch = System.Windows.Media.Stretch.None;
            imgPicture.Visibility = Visibility.Visible;
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
            txtQuestion.Height = desiredSizeNew.Height + 10;

            if (lblscore != null)
                scoringData = lblscore;

            if (question.isNegation)
                setAnswer(true);
            else
                setAnswer(false);

            if (q.hasImage)
                this.loadImage(QuestionImageController.getImage(q.ImagePath));
        }

        public void setAnswer(bool answer)
        {
            this.answer = answer;
            int score = 0;

            if (question.isNegation)
                btnYes.IsChecked = !this.answer;
            else
                btnYes.IsChecked = this.answer;

            if (scoringData != null)
            {
                score = int.Parse(scoringData.Content.ToString());

                if (question.isNegation && this.answer) //negation and answer = yes :-Score
                    score -= question.Score;
                else if (question.isNegation && !this.answer) //negation and answer = no :+Score
                    score += question.Score;
                else if (!question.isNegation && this.answer) //no negation and answer = yes :+Score
                    score += question.Score;
                else if (!question.isNegation && !this.answer) //no negation and answer = no :-Score
                    score -= question.Score;

                scoringData.Content = score;
            }


        }

        //Allan Note: ClipsController Point 1 Integration Done
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            if (question.isNegation)
                (sender as ToggleButton).IsChecked = !answer;
            else
                (sender as ToggleButton).IsChecked = answer;
        }

        public bool getAnswer()
        {
            return answer;
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

        public bool getHaveImage()
        {
            return havImage;
        }
    }
}
