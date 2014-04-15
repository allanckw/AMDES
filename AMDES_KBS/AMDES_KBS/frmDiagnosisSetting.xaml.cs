using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;


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
            loadALLQuestionGroup();
            lstGroupList.ItemsSource = null;
            lstGroupList.ItemsSource = new List<QuestionGroup>();
        }

        private void loadALLQuestionGroup()
        {
            cboGroupList.ItemsSource = null;
            cboGroupList.ItemsSource = QuestionController.getAllQuestionGroup();
        }

        private void loadALLDiagnosis()
        {
            lstGroupList.ItemsSource = null;
            lstGroupList.ItemsSource = new List<QuestionGroup>();
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
            newDiagnosis.RetrieveSym = chkSym.IsChecked.Value;
            newDiagnosis.RetrievalIDList = getSymptonsForDiagnosis();

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

            cboGroupList.SelectedIndex = -1;
            lstGroupList.ItemsSource = null;
            lstSymptomsList.ItemsSource = null;

            Diagnosis sDiagnosis = (Diagnosis)lstDiagnosisList.Items.GetItemAt(sidx);
            loadData(sDiagnosis);
            currIdx = sidx;
        }

        private void loadData(Diagnosis d)
        {
            txtHeader.Text = d.Header;
            txtComment.Text = d.Comment.Replace("~~", Environment.NewLine);
            txtLink.Text = d.Link;
            chkSym.IsChecked = d.RetrieveSym;
            chkSym_Checked(this, new RoutedEventArgs());

            lstGroupList.ItemsSource = null;
            List<QuestionGroup> qgGrpList = new List<QuestionGroup>();
            for (int i = 0; i < d.RetrievalIDList.Count; i++)
            {
                qgGrpList.Add(QuestionController.getGroupByID(d.RetrievalIDList[i]));
            }
            lstGroupList.ItemsSource = qgGrpList;
        }

        private void loadSymtpomQuestionGroup(Diagnosis d)
        {
            lstGroupList.Items.Clear();
            List<QuestionGroup> currQGLst = new List<QuestionGroup>();
            foreach (QuestionGroup qgItem in currQGLst)
            {
                lstGroupList.Items.Add(qgItem);
            }
            
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

            sDiagnosis.RetrieveSym = chkSym.IsChecked.Value;
            sDiagnosis.RetrievalIDList = getSymptonsForDiagnosis();

            if (!SaveDiagnosis(sDiagnosis))
            {
                return;
            }

            currIdx = sidx;
            loadALLDiagnosis();

        }

        private List<int> getSymptonsForDiagnosis()
        {
            List<QuestionGroup> currQGlist = (List<QuestionGroup>)lstGroupList.ItemsSource;

            List<int> z = new List<int>();

            foreach (QuestionGroup g in currQGlist)
            {
                z.Add(g.GroupID);
            }

            return z;
        }

        private bool SaveDiagnosis(Diagnosis d)
        {
            if (d.Header.Trim().Length == 0)
            {
                MessageBox.Show("Please Key In Header!");
                txtHeader.Focus();
                return false;
            }

            if (d.RetrieveSym == false && d.Comment.Trim().Length == 0)
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

        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            int idx = cboGroupList.SelectedIndex;
            if (idx==-1)
            {
                return;
            }

            QuestionGroup qg = (QuestionGroup)cboGroupList.Items[idx];
            if (addSymptonsFromGroup(qg))
                AddSymptomtoDiagnosis(qg);
        }

        private bool addSymptonsFromGroup(QuestionGroup qg)
        {
            List<QuestionGroup> lstQG = (List<QuestionGroup>)lstGroupList.ItemsSource;

            if (lstQG == null)
            {
                return true;
            }

            foreach (QuestionGroup qgItem in lstQG)
            {
                if (qgItem.GroupID == qg.GroupID)
                {
                    return false;
                }
            }
            return true;
        }

        private void AddSymptomtoDiagnosis(QuestionGroup qg)
        {
            List<QuestionGroup> lstQG = (List<QuestionGroup>)lstGroupList.ItemsSource;
            if (lstQG == null)
            {
                lstQG = new List<QuestionGroup>();
            }

            lstQG.Add(qg);
            lstGroupList.ItemsSource = null;
            lstGroupList.ItemsSource = lstQG;
        }

        private void btnDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            List<QuestionGroup> lstQG = (List<QuestionGroup>)lstGroupList.ItemsSource;
            int idx = lstGroupList.SelectedIndex;
            if (idx == -1)
            {
                return;
            }
            lstQG.RemoveAt(idx);
            lstGroupList.ItemsSource = null;
            lstGroupList.ItemsSource = lstQG;
        }

        private void cboGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<String> SymptomsList = new List<String>();
            int idx = cboGroupList.SelectedIndex;
            if (idx == -1)
                return;
            QuestionGroup qg = (QuestionGroup)cboGroupList.Items[idx];
            if (qg.Symptom.Trim()!="")
            {
                SymptomsList.Add(qg.Symptom.Trim());
            }

            foreach (Question q in qg.Questions)
            {
                String symptom = q.Symptom.Trim();
                if (symptom.Length>0)
                {
                    if (!SymptomsList.Contains(symptom))
                    {
                        SymptomsList.Add(symptom);
                    }
                }
            }

            lstSymptomsList.ItemsSource = null;
            lstSymptomsList.ItemsSource = SymptomsList;
        }

        private void lstGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = lstGroupList.SelectedIndex;
            if (idx==-1)
            {
                return;
            }

            QuestionGroup selectedQG = (QuestionGroup)lstGroupList.Items[idx];
            for (int i = 0; i < cboGroupList.Items.Count; i++)
            {
                QuestionGroup qg = (QuestionGroup)cboGroupList.Items[i];
                if (qg.GroupID==selectedQG.GroupID)
                {
                    cboGroupList.SelectedIndex = i;
                    break;
                }
            }
            
        }

        private void chkSym_Checked(object sender, RoutedEventArgs e)
        {
            //@Allan - I think he still want to have comment and link if i'm not wrong
            //bool sym = chkSym.IsChecked.Value;

            //txtComment.IsEnabled = !sym;
            //txtLink.IsEnabled = !sym;

            stkpnlSymtomsSection.Visibility = Visibility.Visible;
        }

        private void chkSym_Unchecked(object sender, RoutedEventArgs e)
        {
            stkpnlSymtomsSection.Visibility = Visibility.Hidden;
        }
    }
}
