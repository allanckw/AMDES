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
            loadAllPatients();
        }

        public frmOverview(Frame frame, Patient p)
        {
            InitializeComponent();
            //loadQuestion();
            amdesPageFrame = frame;
            loadPatient(p);

        }


        public frmOverview(Frame frame, List<Patient> plist)
        {
            InitializeComponent();
            //loadQuestion();
            amdesPageFrame = frame;
            loadPatients(plist);

        }

        private void loadPatient(Patient p)
        {
            ucPatientDisplay patient = new ucPatientDisplay(p, amdesPageFrame);
            stkpnlPatientList.Children.Add(patient);
        }

        private void loadPatients(List<Patient> pList)
        {
            for (int i = 0; i < pList.Count; i++)
            {
                Patient p = pList.ElementAt(i);
                loadPatient(p);
            }
        }

        private void loadAllPatients()
        {
            List<Patient> pList = PatientController.getAllPatients();

            loadPatients(pList);
            //for (int i = 0; i < pList.Count; i++)
            //{
            //    Patient p = pList.ElementAt(i);
            //    loadPatient(p);
            //}
        }


    }
}
