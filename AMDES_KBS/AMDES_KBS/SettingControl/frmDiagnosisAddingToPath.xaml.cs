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
    /// Interaction logic for frmDiagnosisAddingToPath.xaml
    /// </summary>
    public partial class frmDiagnosisAddingToPath : Window
    {
        private List<Diagnosis> AddedList;
        private List<Diagnosis> NotAddedList;

        public frmDiagnosisAddingToPath(List<Diagnosis> dList)
        {
            InitializeComponent();
            AddedList = dList;
            reloadList();
        }

        private void reloadList()
        {
            loadNotAddedDiagnosis();
            loadAddedDiagnosis();
        }

        private void loadNotAddedDiagnosis()
        {
            List<Diagnosis> dList = DiagnosisController.getAllDiagnosis();

            for (int i = 0; i < dList.Count; i++)
            {
                Diagnosis d = dList[i];
                if (existsInAddedList(d))
                {
                    dList.Remove(d);
                    i--;
                }
            }

            NotAddedList = dList;

            lstDiagnosisList.ItemsSource = NotAddedList;
        }

        private void loadAddedDiagnosis()
        {
            //List<Diagnosis> dList = DiagnosisController.getAllDiagnosis();
            //for (int i = 0; i < dList.Count; i++)
            //{
            //    Diagnosis d = dList[i];
            //    if (!existsInAddedList(d))
            //    {
            //        dList.Remove(d);
            //        i--;
            //    }
            //}

            //AddedList = dList;
            lstDiagnosisListAdded.ItemsSource = null;
            lstDiagnosisListAdded.ItemsSource = AddedList;
        }

        private bool existsInAddedList(Diagnosis d)
        {
            if (AddedList == null)
            {
                return false;
            }

            foreach (Diagnosis dAdded in AddedList)
            {
                if (d.RID == dAdded.RID)
                {
                    return true;
                }
            }
            return false;
        }

        private void loadDiagnosisPreview(Diagnosis d)
        {
            lblheader.Content = d.Header;
            txtComment.Text = d.Comment.Replace("~~", Environment.NewLine);
            txtLink.Text = d.Link;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void lstDiagnosisList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sidx = lstDiagnosisList.SelectedIndex;
            if (sidx == -1)
            {
                return;
            }

            Diagnosis d = (Diagnosis)lstDiagnosisList.Items.GetItemAt(sidx);

            loadDiagnosisPreview(d);
        }

        private void lstDiagnosisListAdded_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sidx = lstDiagnosisListAdded.SelectedIndex;
            if (sidx == -1)
            {
                return;
            }

            Diagnosis d = (Diagnosis)lstDiagnosisListAdded.Items.GetItemAt(sidx);

            loadDiagnosisPreview(d);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lstDiagnosisListAdded.SelectedIndex;
            if (sidx == -1)
            {
                return;
            }

            AddedList.RemoveAt(sidx);
            reloadList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lstDiagnosisList.SelectedIndex;
            if (sidx == -1)
            {
                return;
            }

            Diagnosis d = (Diagnosis)lstDiagnosisList.Items.GetItemAt(sidx);
            AddedList.Add(d);
            reloadList();
        }

        public List<Diagnosis> getAddedDiagnosis()
        {
            return AddedList;
        }
    }
}
