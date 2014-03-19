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
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmDiagnosisSetting.xaml
    /// </summary>
    public partial class frmDiagnosisSetting : Page
    {
        public frmDiagnosisSetting()
        {
            InitializeComponent();
            Diagnosis dia = new Diagnosis();
            //dia.Comment;
        }

        private void loadALLDiagnosis()
        {
            
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            Diagnosis newDiagnosis = new Diagnosis();
            newDiagnosis.Comment = txtComment.Text.Replace(Environment.NewLine, "~~");
        }

        private void lstDiagnosisList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sidx = lstDiagnosisList.SelectedIndex;
            if (sidx == -1)
            {
                return;
	        }

            Diagnosis sDiagnosis = (Diagnosis)lstDiagnosisList.Items.GetItemAt(sidx);
            txtComment.Text = sDiagnosis.Comment.Replace("~~", Environment.NewLine);

        }
    }
}
