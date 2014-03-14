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
using System.Windows.Shapes;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmSetting.xaml
    /// </summary>
    public partial class frmSetting : Window
    {
        public frmSetting()
        {
            InitializeComponent();
            loadSectionA();
            loadSectionB();
            loadSectionC();
            loadSectionC2();
            loadSectionC3();
            loadSectionD();
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            Control btn = ((Control)sender);
            StackPanel stkpnl = (StackPanel)btn.Parent;
            StackPanel stkpnlQuestion = (StackPanel)stkpnl.Parent;
            stkpnlQuestion.Visibility = Visibility.Collapsed;
            //throw new NotImplementedException();
        }

        public void loadSectionA()
        {
            stkpnlSectionA.Children.Clear();
            for (int c = 0; c < 5; c++)
            {
                StackPanel stkpnlQuestion = new StackPanel();
                StackPanel stkpnlHeader = new StackPanel();
                stkpnlHeader.Margin = new Thickness(0, 0, 0, 5);
                stkpnlHeader.Orientation = Orientation.Horizontal;

                Label lb = new Label();
                lb.Margin = new Thickness(0, 0, 5, 5);
                lb.Content = "Question A" + (c + 1);

                Button btn = new Button();
                btn.Content = "Delete this question";
                btn.Click += new RoutedEventHandler(btn_Click);

                stkpnlHeader.Children.Add(lb);
                stkpnlHeader.Children.Add(btn);

                TextBox tb = new TextBox();
                tb.AcceptsReturn = true;
                tb.Width = 500;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Margin = new Thickness(0, 0, 0, 10);
                string temp = "";
                for (int i = 0; i < 5; i++)
                {
                    temp = temp + i + Environment.NewLine;
                }
                tb.Text = temp;

                stkpnlQuestion.Children.Add(stkpnlHeader);
                stkpnlQuestion.Children.Add(tb);
                stkpnlSectionA.Children.Add(stkpnlQuestion);
            }


        }

        public void loadSectionB()
        {
            stkpnlSectionB.Children.Clear();
            for (int c = 0; c < 5; c++)
            {
                StackPanel stkpnlQuestion = new StackPanel();
                StackPanel stkpnlHeader = new StackPanel();
                stkpnlHeader.Margin = new Thickness(0, 0, 0, 5);
                stkpnlHeader.Orientation = Orientation.Horizontal;

                Label lb = new Label();
                lb.Margin = new Thickness(0, 0, 5, 5);
                lb.Content = "Question B" + (c + 1);

                Button btn = new Button();
                btn.Content = "Delete this question";
                btn.Click += new RoutedEventHandler(btn_Click);

                stkpnlHeader.Children.Add(lb);
                stkpnlHeader.Children.Add(btn);

                TextBox tb = new TextBox();
                tb.AcceptsReturn = true;
                tb.Width = 500;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Margin = new Thickness(0, 0, 0, 10);
                string temp = "";
                for (int i = 0; i < 5; i++)
                {
                    temp = temp + i + Environment.NewLine;
                }
                tb.Text = temp;

                stkpnlQuestion.Children.Add(stkpnlHeader);
                stkpnlQuestion.Children.Add(tb);
                stkpnlSectionB.Children.Add(stkpnlQuestion);
            }


        }

        public void loadSectionC()
        {
            stkpnlSectionC.Children.Clear();
            for (int c = 0; c < 5; c++)
            {
                StackPanel stkpnlQuestion = new StackPanel();
                StackPanel stkpnlHeader = new StackPanel();
                stkpnlHeader.Margin = new Thickness(0, 0, 0, 5);
                stkpnlHeader.Orientation = Orientation.Horizontal;

                Label lb = new Label();
                lb.Margin = new Thickness(0, 0, 5, 5);
                lb.Content = "Question C" + (c + 1);

                Button btn = new Button();
                btn.Content = "Delete this question";
                btn.Click += new RoutedEventHandler(btn_Click);

                stkpnlHeader.Children.Add(lb);
                stkpnlHeader.Children.Add(btn);

                TextBox tb = new TextBox();
                tb.AcceptsReturn = true;
                tb.Width = 500;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Margin = new Thickness(0, 0, 0, 10);
                string temp = "";
                for (int i = 0; i < 5; i++)
                {
                    temp = temp + i + Environment.NewLine;
                }
                tb.Text = temp;

                stkpnlQuestion.Children.Add(stkpnlHeader);
                stkpnlQuestion.Children.Add(tb);
                stkpnlSectionC.Children.Add(stkpnlQuestion);
            }


        }

        public void loadSectionC2()
        {
            stkpnlSectionC2.Children.Clear();
            for (int c = 0; c < 5; c++)
            {
                StackPanel stkpnlQuestion = new StackPanel();
                StackPanel stkpnlHeader = new StackPanel();
                stkpnlHeader.Margin = new Thickness(0, 0, 0, 5);
                stkpnlHeader.Orientation = Orientation.Horizontal;

                Label lb = new Label();
                lb.Margin = new Thickness(0, 0, 5, 5);
                lb.Content = "Question C2-" + (c + 1);

                Button btn = new Button();
                btn.Content = "Delete this question";
                btn.Click += new RoutedEventHandler(btn_Click);

                stkpnlHeader.Children.Add(lb);
                stkpnlHeader.Children.Add(btn);

                TextBox tb = new TextBox();
                tb.AcceptsReturn = true;
                tb.Width = 500;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Margin = new Thickness(0, 0, 0, 10);
                string temp = "";
                for (int i = 0; i < 5; i++)
                {
                    temp = temp + i + Environment.NewLine;
                }
                tb.Text = temp;

                stkpnlQuestion.Children.Add(stkpnlHeader);
                stkpnlQuestion.Children.Add(tb);
                stkpnlSectionC2.Children.Add(stkpnlQuestion);
            }


        }

        public void loadSectionC3()
        {
            stkpnlSectionC3.Children.Clear();
            for (int c = 0; c < 5; c++)
            {
                StackPanel stkpnlQuestion = new StackPanel();
                StackPanel stkpnlHeader = new StackPanel();
                stkpnlHeader.Margin = new Thickness(0, 0, 0, 5);
                stkpnlHeader.Orientation = Orientation.Horizontal;

                Label lb = new Label();
                lb.Margin = new Thickness(0, 0, 5, 5);
                lb.Content = "Question C3-" + (c + 1);

                Button btn = new Button();
                btn.Content = "Delete this question";
                btn.Click += new RoutedEventHandler(btn_Click);

                stkpnlHeader.Children.Add(lb);
                stkpnlHeader.Children.Add(btn);

                TextBox tb = new TextBox();
                tb.AcceptsReturn = true;
                tb.Width = 500;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Margin = new Thickness(0, 0, 0, 10);
                string temp = "";
                for (int i = 0; i < 5; i++)
                {
                    temp = temp + i + Environment.NewLine;
                }
                tb.Text = temp;

                stkpnlQuestion.Children.Add(stkpnlHeader);
                stkpnlQuestion.Children.Add(tb);
                stkpnlSectionC3.Children.Add(stkpnlQuestion);
            }


        }

        public void loadSectionD()
        {
            stkpnlSectionD.Children.Clear();
            for (int c = 0; c < 5; c++)
            {
                StackPanel stkpnlQuestion = new StackPanel();
                StackPanel stkpnlHeader = new StackPanel();
                stkpnlHeader.Margin = new Thickness(0, 0, 0, 5);
                stkpnlHeader.Orientation = Orientation.Horizontal;

                Label lb = new Label();
                lb.Margin = new Thickness(0, 0, 5, 5);
                lb.Content = "Question D" + (c + 1);

                Button btn = new Button();
                btn.Content = "Delete this question";
                btn.Click += new RoutedEventHandler(btn_Click);

                stkpnlHeader.Children.Add(lb);
                stkpnlHeader.Children.Add(btn);

                TextBox tb = new TextBox();
                tb.AcceptsReturn = true;
                tb.Width = 500;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Margin = new Thickness(0, 0, 0, 10);
                string temp = "";
                for (int i = 0; i < 5; i++)
                {
                    temp = temp + i + Environment.NewLine;
                }
                tb.Text = temp;

                stkpnlQuestion.Children.Add(stkpnlHeader);
                stkpnlQuestion.Children.Add(tb);
                stkpnlSectionD.Children.Add(stkpnlQuestion);
            }

        }

        private void AddNewQuestion(StackPanel stkpnl)
        {
            StackPanel stkpnlQuestion = new StackPanel();
            StackPanel stkpnlHeader = new StackPanel();
            stkpnlHeader.Margin = new Thickness(0, 0, 0, 5);
            stkpnlHeader.Orientation = Orientation.Horizontal;

            Label lb = new Label();
            lb.Margin = new Thickness(0, 0, 5, 5);
            lb.Content = "New Question";

            Button btn = new Button();
            btn.Content = "Delete this question";
            btn.Click += new RoutedEventHandler(btn_Click);

            stkpnlHeader.Children.Add(lb);
            stkpnlHeader.Children.Add(btn);

            TextBox tb = new TextBox();
            tb.AcceptsReturn = true;
            tb.Width = 500;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 0, 0, 10);
            string temp = "";

            tb.Text = temp;

            stkpnlQuestion.Children.Add(stkpnlHeader);
            stkpnlQuestion.Children.Add(tb);
            stkpnl.Children.Add(stkpnlQuestion);
            tb.Focus();
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int sectionSelected = tcQuestionSetting.SelectedIndex;
                switch (sectionSelected)
                {
                    case 0://Section A
                        AddNewQuestion(stkpnlSectionA);
                        break;
                    case 1://Section B
                        AddNewQuestion(stkpnlSectionB);
                        break;
                    case 2://Secton C
                        AddNewQuestion(stkpnlSectionC);
                        break;
                    case 3://Section C2
                        AddNewQuestion(stkpnlSectionC2);
                        break;
                    case 4://section C3
                        AddNewQuestion(stkpnlSectionC3);
                        break;
                    case 5://section D
                        AddNewQuestion(stkpnlSectionD);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
