using System;
using System.Windows;
using System.Windows.Controls;
using AMDES_KBS.Entity;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.IO;
using AMDES_KBS.Controllers;
using System.Linq;

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
                string filePath = dlg.FileName;
                string fileName = Path.GetFileName(filePath);

                txtImgURL.Text = QuestionImageController.processImage(filePath, fileName); 
                

            }
        }


        public Question getQuestion()
        {
            question.Name = txtQuestion.Text.Replace(Environment.NewLine, "~~");
            question.Symptom = txtSymptom.Text;
            if (txtScore.Text.Trim().Length == 0)
                question.Score = 1;
            else
                question.Score = float.Parse(txtScore.Text.Trim());

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

        private void txtScore_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtScore.Text.Trim().Length > 0)
            {
                bool isValid = App.NumericValidationTextBox(txtScore.Text.Trim());
                if (!isValid)
                {
                    MessageBox.Show("Please enter a valid numeric value!", "Invalid value", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtScore.Clear();
                    txtScore.Focus();
                }
            }
        }

        private void txtScore_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
             e.Handled = App.NumericValidationTextBox(e.Text);
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBx = (TextBox)sender;
            float x = 0;
            if (!float.TryParse(txtBx.Text.Trim(), out x))
            {
                int removeCount = e.Changes.First().AddedLength;
                txtBx.Text = txtBx.Text.Substring(0, txtBx.Text.Length - removeCount);
                txtBx.CaretIndex = txtBx.Text.Length;
            }
        }
    }
}
