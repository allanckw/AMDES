using System.Windows;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmQuestionGroupSetting.xaml
    /// </summary>
    public partial class frmQuestionGroupSetting : Window
    {
        private int answer=-1;
        public frmQuestionGroupSetting()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (radOR.IsChecked == true)
            {
                answer = int.Parse(radOR.Tag.ToString());
            }
            else if (radAND.IsChecked == true)
            {
                answer = int.Parse(radAND.Tag.ToString());
            }
            else if (radCOUNT.IsChecked == true)
            {
                answer = int.Parse(radCOUNT.Tag.ToString());
            }
            else
            {
                answer = -1;
                this.DialogResult = false;
                this.Close();
            }
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            answer = -1;
            this.DialogResult = false;
            this.Close();
        }

        public int getAnswer()
        {
            return answer;
        }
    }
}
