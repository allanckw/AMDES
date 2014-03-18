using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmOverview.xaml
    /// </summary>
    public partial class frmOverview : AMDESPage
    {
        Frame amdesPageFrame;
        public frmOverview(Frame frame)
        {
            InitializeComponent();
            //loadQuestion();
            amdesPageFrame = frame;
            loadPatient();
            
        }

        private void loadPatient()
        {
            List<Patient> pList = PatientController.getAllPatients();

            for (int i = 0; i < pList.Count; i++)
            {
                ucPatientDisplay patient = new ucPatientDisplay(pList.ElementAt(i), amdesPageFrame);
                stkpnlPatientList.Children.Add(patient);
            }
        }


    }
}
