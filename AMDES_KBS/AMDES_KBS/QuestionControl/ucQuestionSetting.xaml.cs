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

        public ucQuestionSetting(Question q, bool isScoreType)
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
            chkNegate.IsChecked = q.isNegation;
            txtImgURL.Text = q.ImagePath;
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            this.stkpnlQuestion.Visibility = Visibility.Collapsed;
        }
        
        private void btnBrowseImg_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Image Files (BMP,JPG,PNG,GIF)|*.bmp;*.jpg;*.png;*.gif";
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 

            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                txtImgURL.Text = filename;
            }
        }


        public Question getQuestion()
        {
            question.Name = txtQuestion.Text.Replace(Environment.NewLine, "~~");
            question.Symptom = txtSymptom.Text;
            if (txtScore.Text.Trim().Length == 0)
                question.Score = 1;
            else
                question.Score = int.Parse(txtScore.Text.Trim());

            question.isNegation = chkNegate.IsChecked.Value;
            question.ImagePath = txtImgURL.Text.Trim();
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

        private void txtScore_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = App.NumberValidationTextBox(e.Text);
        }

    }
}
