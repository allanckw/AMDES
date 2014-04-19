using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Controllers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmP2.xaml
    /// </summary>
    public partial class frmP2 : Window
    {
        List<P2Controller> Test_Sets;

        public frmP2()
        {
            InitializeComponent();
            this.Cursor = Cursors.Wait;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbName.SelectedIndex == -1)
            {
                MessageBox.Show("Invalid Attribute Name, Please select something");
                return;
            }
            string value = get_value();

            if (value == null || value.Trim() == "")
            {
                MessageBox.Show("Invalid value");
                return;
            }

            if (txtValue.Visibility == System.Windows.Visibility.Visible)
            {
                double result;
                if (!Double.TryParse(value.Trim(), out result))
                {
                    MessageBox.Show("continous value, please enter a number");
                    return;
                }
            }

            foreach (Tuple<string, string> exist in LVVariables.Items)
            {
                if (exist.Item1 == cbName.Text)
                {
                    MessageBox.Show("this attribute already been asserted before");
                    return;
                }
            }

            Tuple<string, string> newVar = new Tuple<string, string>(cbName.Text, value);

            LVVariables.Items.Add(newVar);

            //P2Controller.load(LVVariables);
            load();
        }

        public void load()
        {
            try
            {
                LVResults.ItemsSource = null;
                List<Tuple<string, double, double>> lst = new List<Tuple<string, double, double>>();
                lst.Capacity = Test_Sets.Count;

                //foreach (P2Controller inc in Test_Sets)
                Parallel.ForEach(Test_Sets, inc =>
                {
                    Tuple<string, double, double> temp_store = load(inc);
                    if (temp_store == null)
                        return;

                    lst.Add(temp_store);

                    //1st 1 is test name 2nd 1 is yes 3rd 1 is no %
                });
                lst = lst.OrderByDescending(x => x.Item2).ToList();
                LVResults.ItemsSource = lst;

                loadBarChart(lst);

            }
            catch (Exception c)
            {
                MessageBox.Show(c.Message);
                this.Close();
            }
        }

        private void loadBarChart(List<Tuple<string, double, double>> lst)
        {
            stkpnlGraph.Children.Clear();
            ucChart barChart = new ucChart(lst);
            stkpnlGraph.Children.Add(barChart);

        }
        private Tuple<string, double, double> load(P2Controller instance)
        {
            List<Tuple<string, string, string>> result = instance.Click(LVVariables);
            double positive = 0; string pos = "yes";//"positive";
            double negative = 0; string neg = "no";

            for (int i = 0; i < result.Count(); i++)
            {
                string classifcation = result[i].Item1.ToLower();
                Double aggree = 0;
                Double disaggree = 0;

                if ((!Double.TryParse(result[i].Item2, out aggree)) || (!Double.TryParse(result[i].Item3, out disaggree)))
                {
                    throw new InvalidOperationException("invalid percentage Number detected");

                }

                if (classifcation.Equals(pos))
                {
                    positive += aggree;
                    negative += disaggree;
                }
                else if (classifcation.Equals(neg))
                {
                    negative += aggree;
                    positive += disaggree;
                }
                else
                {

                    throw new InvalidOperationException("invalid classifaction detected, this may be due to invalid rule files");
                }
            }

            Double total = positive + negative;

            if (total != 0)
            {
                double percent1 = positive / total; //string.Format("{0:0.0%}",);
                double percent2 = negative / total; // string.Format("{0:0.0%}", negative / total);


                Tuple<string, double, double> Popresult = new Tuple<string, double, double>(
                    instance.File_Name,
                    percent1, percent2);

                return Popresult;
            }
            else
            {
                Tuple<string, double, double> Popresult = new Tuple<string, double, double>(
                    instance.File_Name,
                   0, 0);

                return Popresult;
            }



        }




        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (LVVariables.SelectedIndex == -1)
            {
                MessageBox.Show("Nothing is Selected");
                return;
            }

            LVVariables.Items.RemoveAt(LVVariables.SelectedIndex);

            load();
        }
        private List<Tuple<string, bool, List<string>>> choices;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            P2Controller.attributes = new List<string>();

            string folder = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Initial Test\Rules"; ;
            string filter = "*.txt";
            string[] files = Directory.GetFiles(folder, filter);
            Test_Sets = new List<P2Controller>();
            Test_Sets.Capacity = files.Length;

            cboValue.Visibility = System.Windows.Visibility.Collapsed;
            try
            {
                //foreach (string _file in files)
                Parallel.ForEach(files, _file =>
                {
                    string _filename = System.IO.Path.GetFileName(_file);
                    Test_Sets.Add(new P2Controller(_filename.Substring(0, _filename.Length - 4), _file));//"Engine\\dementia.CLP"));

                });
                //Test_Sets.Add(new P2Controller("Rules\\25-HYDROXY VITAMIN D.txt", "C:\\Users\\Nick\\Documents\\nus sem 8\\cs4244\\dementia.CLP"));


            }
            catch (Exception c)
            {
                MessageBox.Show(c.Message);
                this.Close();
            }
            btnDelete.IsEnabled = false;

            List<Tuple<string, bool, List<string>>> AttChoices = new List<Tuple<string, bool, List<string>>>();//the bool indicate if its nominal anot
            AttChoices.Capacity = Test_Sets.Count;

            foreach (P2Controller t  in Test_Sets)
            //Parallel.ForEach(Test_Sets, t =>
            {
                AttChoices = t.init(AttChoices);
            }//);

            choices = AttChoices;

            foreach (Tuple<string, bool, List<string>> c in choices)
            {
                if (!cbName.Items.Contains(c))
                    cbName.Items.Add(c);
            }

            txtValue.IsEnabled = false;
            load();
            this.Cursor = Cursors.Arrow;
        }

        private void LVVariables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVVariables.SelectedIndex == -1)
            {
                btnDelete.IsEnabled = false;
            }
            btnDelete.IsEnabled = true;
        }

        private void cbName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Set_Mode((Tuple<string, bool, List<string>>)cbName.SelectedValue);
        }

        private string get_value()
        {
            if (txtValue.Visibility == System.Windows.Visibility.Visible && txtValue.IsEnabled)
            {
                return txtValue.Text;
            }
            else if (cboValue.Visibility == System.Windows.Visibility.Visible && cboValue.IsEnabled)
            {
                if (cboValue.SelectedIndex == -1)
                    return null;
                return cboValue.SelectedValue.ToString();
            }

            return null;
        }

        private void Set_Mode(Tuple<string, bool, List<string>> raw_data)
        {
            txtValue.Clear();
            cboValue.SelectedIndex = -1;
            if (raw_data == null)
            {
                txtValue.IsEnabled = false;
                cboValue.IsEnabled = false;
                return;
            }

            txtValue.IsEnabled = true;
            cboValue.IsEnabled = true;
            if (raw_data.Item2)
            {
                cboValue.Visibility = System.Windows.Visibility.Visible;
                txtValue.Visibility = System.Windows.Visibility.Collapsed;

                //populate cb

                cboValue.Items.Clear();
                foreach (string choice in raw_data.Item3)
                {
                    cboValue.Items.Add(choice);
                }
                //cboValue.Items.Add("others");
            }
            else
            {
                cboValue.Visibility = System.Windows.Visibility.Collapsed;
                txtValue.Visibility = System.Windows.Visibility.Visible;
            }
        }


    }
}
