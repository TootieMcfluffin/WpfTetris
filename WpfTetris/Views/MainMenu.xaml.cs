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

namespace WpfTetris.Views
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void RegularTetris_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/Tetris.xaml", UriKind.Relative)); 
        }

        private void CrazyTetris_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/CrazyTetris.xaml", UriKind.Relative));
        }
    }
}
