using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmOverview.xaml
    /// </summary>
    public partial class frmOverview : AMDESPage
    {
        public frmOverview()
        {
            InitializeComponent();
            //loadQuestion();
            loadPatient();
        }

        private void loadPatient()
        {
            for (int i = 0; i < 10; i++)
            {
                ucPatientDisplay patient = new ucPatientDisplay();
                stkpnlPatientList.Children.Add(patient);
            }
        }


    }
}
