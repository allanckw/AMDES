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
using System.Text.RegularExpressions;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmDiagnosisSetting.xaml
    /// </summary>
    public partial class frmDiagnosisSetting : Page
    {

        int currIdx=-1;

        public frmDiagnosisSetting()
        {
            InitializeComponent();
            loadALLDiagnosis();
        }

        private void loadALLDiagnosis()
        {
            lstDiagnosisList.ItemsSource = DiagnosisController.getAllDiagnosis();
            lstDiagnosisList.SelectedIndex = currIdx;
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            Diagnosis newDiagnosis = new Diagnosis();
            newDiagnosis.RID = DiagnosisController.getNextDiagnosisID();
            newDiagnosis.Header = txtHeader.Text.Trim();
            newDiagnosis.Comment = txtComment.Text.Replace(Environment.NewLine, "~~");
            newDiagnosis.Link = txtLink.Text.Trim();

            if (!SaveDiagnosis(newDiagnosis))
            {
                return;
            }
            currIdx = lstDiagnosisList.Items.Count;
            loadALLDiagnosis();
        }

        private void lstDiagnosisList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sidx = lstDiagnosisList.SelectedIndex;
            if (sidx == -1)
            {
                txtHeader.Text = "";
                txtComment.Text = "";
                txtLink.Text = "";
                return;
	        }

            Diagnosis sDiagnosis = (Diagnosis)lstDiagnosisList.Items.GetItemAt(sidx);
            loadData(sDiagnosis);
            currIdx = sidx;
        }

        private void loadData(Diagnosis d)
        {
            txtHeader.Text = d.Header;
            txtComment.Text = d.Comment.Replace("~~", Environment.NewLine);
            txtLink.Text = d.Link;
        }

        private void btnDeleteComment_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lstDiagnosisList.SelectedIndex;
            if (sidx == -1)
            {
                return;
            }
            Diagnosis sDiagnosis = (Diagnosis)lstDiagnosisList.Items.GetItemAt(sidx);
            DiagnosisController.deleteDiagnosis(sDiagnosis.RID);
            currIdx = -1;
            loadALLDiagnosis();
        }

        private void btnSaveComment_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lstDiagnosisList.SelectedIndex;
            if (sidx == -1)
            {
                return;
            }
            Diagnosis sDiagnosis = (Diagnosis)lstDiagnosisList.Items.GetItemAt(sidx);
            sDiagnosis.Header = txtHeader.Text.Trim();
            sDiagnosis.Comment = txtComment.Text.Replace(Environment.NewLine, "~~");
            sDiagnosis.Link = txtLink.Text.Trim();

            if (!SaveDiagnosis(sDiagnosis))
            {
                return;
            }

            currIdx = sidx;
            loadALLDiagnosis();

        }

        private bool SaveDiagnosis(Diagnosis d)
        {
            if (d.Header.Trim().Length == 0)
            {
                MessageBox.Show("Please Key In Header!");
                txtHeader.Focus();
                return false;
            }

            if (d.Comment.Trim().Length == 0)
            {
                MessageBox.Show("Please Key In Comment!");
                txtComment.Focus();
                return false;
            }

            if (!IsValidUrl(d.Link.Trim()) && d.Link.Trim().Length > 0)
            {
                MessageBox.Show("Please Key In Valid URL!");
                txtLink.Text = "";
                txtLink.Focus();
                return false;
            }

            DiagnosisController.updateDiagnosis(d);
            MessageBox.Show("Diagnosis Saved!");
            return true;
        }

        private bool IsValidUrl(string urlString)
        {
            if (Regex.IsMatch(urlString,@"(http://|https://)?(www\.)?[A-Za-z0-9]+\.[a-z]{2,3}"))
            {
                return true;
            }

            return false;
            //Uri uri;
            //return Uri.TryCreate(urlString, UriKind.Absolute, out uri);
                //&& //(uri.Scheme == Uri.UriSchemeHttp
                 //|| uri.Scheme == Uri.UriSchemeHttps
                 //|| uri.Scheme == Uri.UriSchemeFtp
                 //|| uri.Scheme == Uri.UriSchemeMailto
                ///*...*/);
        }
    }
}
