using System.Windows;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string bulletForm()
        {
            return "\u25A0";
        }
        // \u2023 (TRIANGULAR BULLET)
        // \u25E6 (WHITE BULLET)
        // \u25C9 (FISHEYE)
        // \u25A0 (BLACK SQUARE)
        // \u25A1 (WHITE SQUARE)
        // \u274F (LOWER RIGHT DROP-SHADOWED WHITE SQUARE)
    }
}
