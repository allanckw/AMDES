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

namespace AMDES_KBS.QuestionControl
{
    /// <summary>
    /// Interaction logic for ucQuestion.xaml
    /// </summary>
    public partial class ucQuestion : UserControl
    {
        public ucQuestion()
        {
            InitializeComponent();
        }

        public void loadQuestion(string question)
        {
            question = question.Replace("\n", Environment.NewLine);
            txtQuestion.Text = question;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            btnYes.IsEnabled = false;
            btnNo.IsEnabled = true;
            //clip to assert here or modify
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            btnYes.IsEnabled = true;
            btnNo.IsEnabled = false;
            //clip to assert here or modify
        }


    }
}
