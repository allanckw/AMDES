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

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucPatientTest.xaml
    /// </summary>
    public partial class ucPatientTest : UserControl
    {
        private Test t;
        public ucPatientTest(Test t)
        {
            InitializeComponent();
            this.t = t;

            this.txtAssessor.Text = t.Doctor.Name;
            this.txtPatientTest.Text = t.TestName;
            this.txtStatus.Text = t.Status.ToString();
            if (t.OrderedDate != null)
            {
                this.txtTestTime.Text = t.OrderedDate.Value.ToString("dd MMM yyyy");
            }
            if (t.ReportDate != null)
            {
                //this.reportTime.Text = t.ReporDate.Value.ToString("dd MMM yyyy");
            }
        }

        public void setVisbility(Visibility v)
        {
            stkpnlPatientTestDetail.Visibility = v;
        }
    }
}
