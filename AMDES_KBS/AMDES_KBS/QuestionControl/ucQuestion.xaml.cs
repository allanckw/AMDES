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
    /// Interaction logic for ucQuestion.xaml
    /// </summary>
    public partial class ucQuestion : UserControl
    {
        bool questionAnswer = false;
        Question question;
        public ucQuestion()
        {
            InitializeComponent();
        }

        public void loadQuestion(Question q)
        {
            question = q;
            string questionText = q.Name.Replace("~~", Environment.NewLine);
            lblQuestion.Content = q.ID;
            txtQuestion.Text = questionText;
            var desiredSizeOld = txtQuestion.DesiredSize;
            txtQuestion.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var desiredSizeNew = txtQuestion.DesiredSize;
            txtQuestion.Height = desiredSizeNew.Height+10;
            
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            btnYes.IsEnabled = false;
            btnNo.IsEnabled = true;
            CLIPSController.assertQuestion(question.ID, true);
            //clip to assert here or modify
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            btnYes.IsEnabled = true;
            btnNo.IsEnabled = false;
            CLIPSController.assertQuestion(question.ID, false);
            //clip to assert here or modify
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
