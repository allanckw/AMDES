using System.Windows;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Media;
using AMDES_KBS.Controllers;
using System;
using AMDES_KBS.Entity;

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

        public static bool isAttrCompareNumerical(string attrName)
        {
            if (attrName == "AGE")
            {
                return true;
            }

            try
            {
                if (PatAttributeController.searchPatientAttribute(attrName).getAttributeTypeNUM() == PatAttribute.AttributeType.NUMERIC)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string processEnumStringForDataBind(string value)
        {
            string str2display = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]) && i != 0)
                {
                    str2display += " " + value[i];
                }
                else
                {
                    str2display += value[i];
                }
            }

            return str2display;
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
