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
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmQuestionSetting.xaml
    /// </summary>
    public partial class frmQuestionSetting : AMDESPage
    {
        public frmQuestionSetting()
        {
            InitializeComponent();
            loadAllSection();
            //loadSectionA();
            //loadSectionB();
            //loadSectionC();
            //loadSectionC2();
            //loadSectionC3();
            //loadSectionD();
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            Control btn = ((Control)sender);
            StackPanel stkpnl = (StackPanel)btn.Parent;
            StackPanel stkpnlQuestion = (StackPanel)stkpnl.Parent;
            stkpnlQuestion.Visibility = Visibility.Collapsed;
            //throw new NotImplementedException();
        }

        private void loadAllSection()
        {
            tcQuestionSetting.Items.Clear();
            List<QuestionGroup> qgList = QuestionController.getAllQuestionGroup();
            foreach (QuestionGroup group in qgList)
            {
                loadSection(group);
            }
            tcQuestionSetting.SelectedIndex = 0;
        }

        private void loadSection(QuestionGroup qg)
        {
            TabItem section = new TabItem();
            section.Header = qg.Header;
            section.HorizontalAlignment = HorizontalAlignment.Left;
            section.Tag = qg.GroupID;
            ScrollViewer sv = new ScrollViewer();
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv.HorizontalAlignment = HorizontalAlignment.Left;
            sv.Width = tcQuestionSetting.Width-10;

            StackPanel stkpnlSection = new StackPanel();

            foreach (Question q in qg.Questions)
            {
                ucQuestionSetting question = new ucQuestionSetting(q);
                question.Margin = new Thickness(0, 0, 0, 5);
                stkpnlSection.Children.Add(question);
            }
            sv.Content = stkpnlSection;
            section.Content = sv;
            tcQuestionSetting.Items.Add(section);
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int sectionSelected = tcQuestionSetting.SelectedIndex;
                TabItem sectionTab = (TabItem)tcQuestionSetting.Items.GetItemAt(tcQuestionSetting.SelectedIndex);
                QuestionGroup qg = QuestionController.getGroupByID(int.Parse(sectionTab.Tag.ToString()));

                if (qg.getQuestionTypeENUM() == QuestionType.COUNT)
                {
                    QuestionCountGroup qcg = (QuestionCountGroup)qg;
                    if (NoOfQuestion(sectionTab) >= qcg.MaxQuestions)
                    {
                        MessageBox.Show("You have reached the maximum number of question!");
                        return;
                    }
                }

                ScrollViewer sv = (ScrollViewer)sectionTab.Content;
                StackPanel stkpnl = (StackPanel)sv.Content;
                ucQuestionSetting newQuestion = new ucQuestionSetting();
                newQuestion.Margin = new Thickness(0, 0, 0, 5);
                stkpnl.Children.Add(newQuestion);
                sv.ScrollToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        private void btnSaveAllQuestion_Click(object sender, RoutedEventArgs e)
        {
            foreach (TabItem sectionTab in tcQuestionSetting.Items)
            {
                ScrollViewer sv = (ScrollViewer)sectionTab.Content;
                StackPanel stkpnl = (StackPanel)sv.Content;
                QuestionGroup gp = QuestionController.getGroupByID(int.Parse(sectionTab.Tag.ToString()));
                gp.Questions.Clear();
                foreach (ucQuestionSetting question in stkpnl.Children)
                {
                    if (question.getToSaved())
                    {
                        gp.Questions.Add(question.getQuestion());
                    }
                }
                QuestionController.updateQuestionGroup(gp);
            }

            
            MessageBox.Show("Done");
            loadAllSection();
        }

        private void btnAddNewSection_Click(object sender, RoutedEventArgs e)
        {
            QuestionGroup qg = new QuestionGroup();
            qg.GroupID = QuestionController.getNextGroupID();
            qg.Header = "";
            qg.Description = "";
            qg.Symptom = "";
        }

        private int NoOfQuestion(TabItem sectionTab)
        {
            int count = 0;
            ScrollViewer sv = (ScrollViewer)sectionTab.Content;
            StackPanel stkpnl = (StackPanel)sv.Content;
            QuestionGroup gp = QuestionController.getGroupByID(int.Parse(sectionTab.Tag.ToString()));
            foreach (ucQuestionSetting question in stkpnl.Children)
            {
                if (question.getToSaved())
                {
                    count++;
                }
            }
            return count;
        }
    }
}
