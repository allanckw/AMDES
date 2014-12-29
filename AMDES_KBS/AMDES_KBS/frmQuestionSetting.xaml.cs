using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

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
        }

        private void loadAllSection()
        {
            tcQuestionSetting.Items.Clear();
            tcQuestionGroupSetting.Items.Clear();
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

            sv.Content = stkpnlSection;
            section.Content = sv;
            tcQuestionSetting.Items.Add(section);
            bool isScoreType = false;

            ucQuestionGroup ucQG;
            if (qg.getQuestionTypeENUM() == QuestionType.COUNT)
            {
                QuestionCountGroup qcg = (QuestionCountGroup)qg;
                ucQG = new ucQuestionGroup(qcg);
                isScoreType = true;
            }
            else
            {
                ucQG = new ucQuestionGroup(qg);
            }

            foreach (Question q in qg.Questions)
            {
                ucQuestionSetting question = new ucQuestionSetting(q, isScoreType);
                stkpnlSection.Children.Add(question);
            }

            TabItem groupSection = new TabItem();
            groupSection.Visibility = Visibility.Collapsed;
            groupSection.Header = qg.Header;
            groupSection.Tag = qg.GroupID;
            groupSection.HorizontalAlignment = HorizontalAlignment.Left;
            groupSection.Content = ucQG;
            tcQuestionGroupSetting.Items.Add(groupSection);
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isScoreType = false;

                int selectedidx = tcQuestionSetting.SelectedIndex;
                TabItem sectionTab = (TabItem)tcQuestionSetting.Items.GetItemAt(selectedidx);

                TabItem tbGroup = (TabItem)tcQuestionGroupSetting.Items.GetItemAt(selectedidx);
                ucQuestionGroup ucControlGroup = (ucQuestionGroup)tbGroup.Content;

                if (sectionTab.Tag.ToString().Trim() != "")
                {
                    QuestionGroup qg = QuestionController.getGroupByID(int.Parse(sectionTab.Tag.ToString()));

                    if (qg.getQuestionTypeENUM() == QuestionType.COUNT)
                    {
                        isScoreType = true;
                        QuestionCountGroup qcg = (QuestionCountGroup)qg;
                        if (NoOfQuestion(sectionTab) >= qcg.MaxQuestions)
                        {
                            MessageBox.Show("You have reached the maximum number of question!");
                            return;
                        }
                    }
                }
                else
                {

                    if (ucControlGroup.stkpnlCOUNT.Visibility==Visibility.Visible)
                    {
                        isScoreType = true;
                        int NoOfQuestionAllowed = int.Parse(ucControlGroup.txtMaxQn.Text.Trim());
                        if (NoOfQuestion(sectionTab) >= NoOfQuestionAllowed)
                        {
                            MessageBox.Show("You have reached the maximum number of question!");
                            return;
                        }
                    }
                }
                
                ScrollViewer sv = (ScrollViewer)sectionTab.Content;
                StackPanel stkpnl = (StackPanel)sv.Content;
                ucQuestionSetting newQuestion = new ucQuestionSetting(isScoreType);
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
            for (int i = 0; i < tcQuestionGroupSetting.Items.Count; i++)
            {
                TabItem sectionTab = (TabItem)tcQuestionSetting.Items.GetItemAt(i);
                TabItem groupsectionTab = (TabItem)tcQuestionGroupSetting.Items.GetItemAt(i);
                ucQuestionGroup groupControl = (ucQuestionGroup)groupsectionTab.Content;
                ScrollViewer sv = (ScrollViewer)sectionTab.Content;
                StackPanel stkpnl = (StackPanel)sv.Content;

                if (groupControl.txtHeader.Text.Trim().Length==0)
                {
                    //tcQuestionGroupSetting.SelectedIndex = i;
                    tcQuestionSetting.SelectedIndex = i;
                    tcQuestionGroupSetting.SelectedIndex = i;
                    MessageBox.Show("Header cannot be empty!");
                    groupControl.txtHeader.Focus();
                    return;
                }

                if (groupControl.getQuestionType() == QuestionType.COUNT)
                {
                    QuestionCountGroup qcg = groupControl.getQuestionCountGroup();

                    qcg.Questions.Clear();

                    foreach (ucQuestionSetting question in stkpnl.Children)
                    {
                        if (question.getToSaved(false))
                        {
                            qcg.Questions.Add(question.getQuestion());
                        }
                    }
                    QuestionController.updateQuestionGroup(qcg);
                }
                else
                {
                    QuestionGroup qg = groupControl.getQuestionGroup();

                    qg.Questions.Clear();

                    foreach (ucQuestionSetting question in stkpnl.Children)
                    {
                        if (question.getToSaved(false))
                        {
                            qg.Questions.Add(question.getQuestion());
                        }
                    }
                    QuestionController.updateQuestionGroup(qg);
                }
            }

            MessageBox.Show("Done");
            loadAllSection();
        }

        private void btnAddNewSection_Click(object sender, RoutedEventArgs e)
        {
            frmQuestionGroupSetting askGroupQuestionSetting = new frmQuestionGroupSetting();
            int questionType = -1;
            if (askGroupQuestionSetting.ShowDialog() == true)
                questionType = askGroupQuestionSetting.getAnswer();
            else
            {
                return;
            }

            
            TabItem newSection = new TabItem();
            newSection.Tag = "";
            newSection.Header = "New";
            newSection.HorizontalAlignment = HorizontalAlignment.Left;

            ScrollViewer sv = new ScrollViewer();
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv.HorizontalAlignment = HorizontalAlignment.Left;
            sv.Width = tcQuestionSetting.Width - 10;

            StackPanel stkpnlSection = new StackPanel();

            sv.Content = stkpnlSection;
            newSection.Content = sv;

            tcQuestionSetting.Items.Add(newSection);

            TabItem newGroupSection = new TabItem();
            newGroupSection.Tag = "";
            newGroupSection.Header = "New";
            newGroupSection.Visibility = Visibility.Collapsed;
            newGroupSection.Content = new ucQuestionGroup(questionType);
            tcQuestionGroupSetting.Items.Add(newGroupSection);
            tcQuestionSetting.SelectedIndex = tcQuestionSetting.Items.Count - 1;
        }

        private int NoOfQuestion(TabItem sectionTab)
        {
            int count = 0;
            ScrollViewer sv = (ScrollViewer)sectionTab.Content;
            StackPanel stkpnl = (StackPanel)sv.Content;
            //QuestionGroup gp = QuestionController.getGroupByID(int.Parse(sectionTab.Tag.ToString()));
            foreach (ucQuestionSetting question in stkpnl.Children)
            {
                if (question.getToSaved(true))
                {
                    count++;
                }
            }
            return count;
        }

        private void tcQuestionSetting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tcQuestionSetting.Items.Count>0)
            {
                tcQuestionGroupSetting.SelectedIndex = tcQuestionSetting.SelectedIndex;
            }
        }

        private void btnDeleteSection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedidx=tcQuestionSetting.SelectedIndex;
                TabItem tbGroup = (TabItem)tcQuestionSetting.Items.GetItemAt(selectedidx);
                if (tbGroup.Tag.ToString().Trim() != "")
                {
                    QuestionController.deleteQuestionGroup(int.Parse(tbGroup.Tag.ToString().Trim()));
                    //QuestionController.getAllQuestionGroup();
                }
                tcQuestionSetting.Items.RemoveAt(selectedidx);
                tcQuestionGroupSetting.Items.RemoveAt(selectedidx);
                tcQuestionSetting.SelectedIndex = selectedidx;
                tcQuestionGroupSetting.SelectedIndex = selectedidx;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            int lastIdx = tcQuestionSetting.SelectedIndex;
            loadAllSection();
            tcQuestionSetting.SelectedIndex = lastIdx;
        }
    }
}
