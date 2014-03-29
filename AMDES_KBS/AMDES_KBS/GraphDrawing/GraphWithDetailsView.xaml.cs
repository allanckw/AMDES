using System.Windows.Controls;
using System.Windows.Media.Media3D;


namespace CircularDependencyTool
{
    public partial class GraphWithDetailsView : UserControl
    {
        public GraphWithDetailsView()
        {
            InitializeComponent();
        }

        private void btnDown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Point3D x = cntCtrl3D.CameraPrototype.Position;
            if (x.Y > -0.5)
            {
                x.Y -= 0.1;
                cntCtrl3D.CameraPrototype.Position = x;
            }
        }

        private void btnUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            Point3D x = cntCtrl3D.CameraPrototype.Position;
            if (x.Y < 0.5)
            {
                x.Y += 0.1;
                cntCtrl3D.CameraPrototype.Position = x;
            }
        }

    }
}