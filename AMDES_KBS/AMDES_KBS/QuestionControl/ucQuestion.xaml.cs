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

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucQuestion.xaml
    /// </summary>
    public partial class ucQuestion : UserControl
    {
        bool questionAnswer = false;

        public ucQuestion()
        {
            InitializeComponent();
        }

        public void loadQuestion(string questionID, string question)
        {
            question = question.Replace("\n", Environment.NewLine);
            lblQuestion.Content = questionID;
            txtQuestion.Text = question;
            var desiredSizeOld = txtQuestion.DesiredSize;
            txtQuestion.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var desiredSizeNew = txtQuestion.DesiredSize;
            txtQuestion.Height = desiredSizeNew.Height-5;
            
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            btnYes.IsEnabled = false;
            btnNo.IsEnabled = true;
            questionAnswer = true;
            //clip to assert here or modify
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            btnYes.IsEnabled = true;
            btnNo.IsEnabled = false;
            questionAnswer = false;
            //clip to assert here or modify
        }

        public bool getAnswer()
        {
            return questionAnswer;
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
