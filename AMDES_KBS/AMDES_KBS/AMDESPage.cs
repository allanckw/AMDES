using System.Windows;
using System.Windows.Controls;


namespace AMDES_KBS
{
    public class AMDESPage : Page
    {
        protected bool changed = false;
        public bool isChanged() { return changed; }
        public virtual bool saveChanges() { return true; }
        protected void onChanged(object sender, RoutedEventArgs e) { changed = true; }
    }
}
