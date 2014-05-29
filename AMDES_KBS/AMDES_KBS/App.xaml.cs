using System.Windows;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Media;

namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string bulletForm()
        {
            return "\u2713";
        }

        public static bool NumberValidationTextBox(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(text);
        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


        // \u2023 (TRIANGULAR BULLET)
        // \u25E6 (WHITE BULLET)
        // \u25C9 (FISHEYE)
        // \u25A0 (BLACK SQUARE)
        // \u25A1 (WHITE SQUARE)
        // \u274F (LOWER RIGHT DROP-SHADOWED WHITE SQUARE)
        // \u2713 (Tick)
    }
}
