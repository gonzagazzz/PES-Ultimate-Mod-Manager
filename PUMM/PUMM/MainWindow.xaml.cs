using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PUMM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            /* Properties */
            //WindowStyle = WindowStyle.None; // hides windows default title bar
            ResizeMode = ResizeMode.CanResize; // window default resize mode
            //ShowsNavigationUI = false; // hides navigation bar
            
            InitializeComponent();
            //System.Diagnostics.Debugger.Break();
            this.DataContext = new ViewModel.MainWindowViewModel();
            
        }

        private void TabHome_Click(object sender, RoutedEventArgs e)
        {
            tabHome.Style = (Style)Application.Current.Resources["EnabledTab"];
            tabLibrary.Style = (Style)Application.Current.Resources["DisabledTab"];
        }

        private void TabLibrary_Click(object sender, RoutedEventArgs e)
        {
            tabHome.Style = (Style)Application.Current.Resources["DisabledTab"];
            tabLibrary.Style = (Style)Application.Current.Resources["EnabledTab"];
        }
    }
}
