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

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucPatientTest.xaml
    /// </summary>
    public partial class ucPatientTest : UserControl
    {
        public ucPatientTest()
        {
            InitializeComponent();
        }

        public void setVisbility(Visibility v)
        {
            stkpnlPatientTestDetail.Visibility = v;
        }
    }
}
