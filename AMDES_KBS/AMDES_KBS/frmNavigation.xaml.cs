using System.Windows.Controls;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmNavigation.xaml
    /// </summary>
    public partial class frmNavigation : AMDESPage
    {
        public frmNavigation()
        {
            InitializeComponent();
            loadAllGroup();
        }

        private void loadAllGroup()
        {
            lstGroupList.ItemsSource = QuestionController.getAllQuestionGroup();
        }

        private void lstGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx =lstGroupList.SelectedIndex;
            if (idx == -1)
            {
                return;
            }

            QuestionGroup qg = (QuestionGroup)lstGroupList.Items.GetItemAt(idx);

            ucNaviTrue.loadData(qg, true);
            ucNaviFalse.loadData(qg, false);

        }
    }
}
