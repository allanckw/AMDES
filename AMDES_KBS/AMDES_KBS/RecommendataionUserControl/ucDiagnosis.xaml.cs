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
    /// Interaction logic for ucDiagnosis.xaml
    /// </summary>
    public partial class ucDiagnosis : UserControl
    {
        public ucDiagnosis(string ruleID)
        {
            InitializeComponent();
            lblRuleID.Content = ruleID;
            //updateHeight();
        }

        public double getHeight()
        {
            updateHeight();
            return gridDiagnosis.Height;
        }

        public void addSymptons(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Label lblSymptons = new Label();
                lblSymptons.Content = "Symptons " + (i + 1);
                stkpnlSymptons.Children.Add(lblSymptons);
            }
            //updateHeight();
        }
        
        public void loadComment()
        {
            string comment = "Likelihood of dementia is low. If the cognitive deficit(s) has been present  for a long time, do consider congenital conditions like mental retardation, cerebral palsy. " +
                Environment.NewLine +
                "You may consider a referral to a specialist should there be a sudden change or decline in the condition.";
            this.txtDiagnosisMessage.Text = comment;
        }

        private void updateHeight()
        {
            var desiredSizeOld = gridDiagnosis.DesiredSize;
            gridDiagnosis.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var desiredSizeNew = gridDiagnosis.DesiredSize;
            gridDiagnosis.Height = desiredSizeNew.Height - 5;
        }

        public void setVisibility(Visibility v)
        {
            this.gridDiagnosis.Visibility = v;
        }

    }
}
