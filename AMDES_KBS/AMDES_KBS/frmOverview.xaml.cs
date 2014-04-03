using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;
using System.Windows.Media;

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
            loadAllPatients();
        }

        public frmOverview(Frame frame, List<Patient> plist)
        {
            InitializeComponent();
            //loadQuestion();
            amdesPageFrame = frame;
            loadPatients(plist);

        }

        private void loadPatient(Patient p, Color c)
        {
            ucPatientDisplay patient = new ucPatientDisplay(p, amdesPageFrame, c);
            stkpnlPatientList.Children.Add(patient);
        }

        private void loadPatients(List<Patient> pList)
        {
            for (int i = 0; i < pList.Count; i++)
            {
                Patient p = pList.ElementAt(i);
                if (i % 2 != 0)
                {
                    loadPatient(p, Color.FromRgb(255, 254, 254));
                }
                else
                {
                    loadPatient(p, Color.FromRgb(242, 254, 254));
                }
            }
        }

        private void loadAllPatients()
        {
            List<Patient> pList = PatientController.getAllPatients();

            loadPatients(pList);
        }


    }
}
