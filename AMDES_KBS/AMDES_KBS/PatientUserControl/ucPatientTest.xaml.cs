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
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucPatientTest.xaml
    /// </summary>
    public partial class ucPatientTest : UserControl
    {
        private History t;
        Patient pat;
        Frame amdesPageFrame;

        public ucPatientTest(History t, Patient p, Frame ParentFrame, SolidColorBrush brush)
        {
            InitializeComponent();
            this.t = t;
            this.Background = brush;
            foreach (Control c in stkpnlPatientTestDetail.Children)
            {
                if (c.GetType() != typeof(Button))
                    c.Background = brush;

            }
            //this.stkpnlPatientTestDetail.Background = brush;
            //this.txtStatus.Text = t.Status.ToString();
            this.amdesPageFrame = ParentFrame;
            this.pat = p;
            this.txtStatus.Text = t.getStatusEnum().ToString();
            if (t.AssessmentDate != null)
            {
                this.txtTestTime.Text = t.AssessmentDate.ToString("dd MMM yyyy");
            }
            if (t.getStatusEnum() == PatientStatus.COMPLETED)
            {
                btnResult.Visibility = Visibility.Visible;
                btnCont.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnResult.Visibility = Visibility.Collapsed;
                btnCont.Visibility = Visibility.Visible;
            }
        }

        public void setVisbility(Visibility v)
        {
            stkpnlPatientTestDetail.Visibility = v;
        }

        private void btnResult_Click(object sender, RoutedEventArgs e)
        {
            CLIPSController.CurrentPatient = pat;
            frmRecommendationViewOnly frmConclusion = new frmRecommendationViewOnly(amdesPageFrame, t);
            amdesPageFrame.Navigate(frmConclusion);
        }

        private void btnCont_Click(object sender, RoutedEventArgs e)
        {
            resume();
        }

        private void resume()
        {
            frmSection prevSection = null;
            CLIPSController.CurrentPatient = pat;

            CLIPSController.clearAndLoadNew();

            Dictionary<int, List<QnHistory>> history = t.getHistory();
            for (int i = 0; i < history.Keys.Count; i++)
            {
                int k = history.Keys.ElementAt(i);
                List<QnHistory> qnHis = t.retrieveHistoryList(k);

                frmSection QnSect = createSection(k, qnHis);

                if (prevSection != null)
                {
                    QnSect.setPrevPage(prevSection);
                }

                if (i != history.Keys.Count - 1)
                {
                    CLIPSController.assertNextSection();
                }

                prevSection = QnSect;
            }
            //foreach (int k in history.Keys)
            //{
            //    //MessageBox.Show(k.ToString());

            //}
            int sectionID = CLIPSController.getCurrentQnGroupID();
            amdesPageFrame.Navigate(prevSection);
        }

        private frmSection createSection(int gid, List<QnHistory> qnhistory)
        {
            frmSection section = new frmSection(amdesPageFrame, gid, qnhistory);
            return section;
        }
    }
}
