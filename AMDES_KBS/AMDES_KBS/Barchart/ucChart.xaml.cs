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
using System.Windows.Controls.DataVisualization.Charting;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for ucChart.xaml
    /// </summary>
    public partial class ucChart : UserControl
    {
        public ucChart(List<Tuple<string, string, string>> lst)
        {
            InitializeComponent();
            lblTitle.Content = "Tests Conducted during initial visits";
            LoadChart(ProcessTuple(lst));
        }

        private System.Collections.ObjectModel.Collection<InitialVisitTest> ProcessTuple(List<Tuple<string, string, string>> lst)
        {
            System.Collections.ObjectModel.Collection<InitialVisitTest> TestCollection = new System.Collections.ObjectModel.Collection<InitialVisitTest>();
            foreach (Tuple<string, string, string> item in lst)
            {
                InitialVisitTest t = new InitialVisitTest();
                t.TestName = item.Item1;
                double x = 0;
                bool result = double.TryParse(item.Item2.Remove(item.Item2.Length-1,1), out x);
                if (!result)
                {
                    t.Yes = 0;
                }
                else
                {
                    t.Yes = x;
                }
                TestCollection.Add(t);
            }
            return TestCollection;
        }

        private void LoadChart(System.Collections.ObjectModel.Collection<InitialVisitTest> TestCollection)
        {
            var yAxis = this.Chart1.ActualAxes.OfType<LinearAxis>().FirstOrDefault(ax => ax.Orientation == AxisOrientation.Y);
            if (yAxis != null)
                yAxis.Maximum = 100;
            Chart1.Axes.Add(new LinearAxis()
            {
                Minimum = 0,
                Maximum = 100,
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Top,
                Interval=5,
                ShowGridLines = true,
            });
            
            ((BarSeries)Chart1.Series[0]).ItemsSource = TestCollection;
            Chart1.Height = TestCollection.Count * 30;

        }

    }
}
