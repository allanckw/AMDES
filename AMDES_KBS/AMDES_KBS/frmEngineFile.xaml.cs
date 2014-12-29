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
using System.Windows.Shapes;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmEngineFile.xaml
    /// </summary>
    public partial class frmEngineFile : Window
    {
        public frmEngineFile()
        {
            InitializeComponent();
            EngineFile ef = EnginePathController.readEngineFileName();
            if (ef == null)
            {
                txtFileName.Text = "";
                MessageBox.Show("Please enter file name of engine!", "Missing Engine File", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                txtFileName.Text = ef.FileName;
               
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            EngineFile ef = new EngineFile(txtFileName.Text.Trim());
            EnginePathController.writeEngineFileName(ef);
            CLIPSController.setCLPPath(ef);
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
