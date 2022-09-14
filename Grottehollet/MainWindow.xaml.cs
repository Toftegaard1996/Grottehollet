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

namespace Grottehollet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void NameLoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NameLoginBox.Text = null;
        }

        private void CodeLoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            CodeLoginBox.Text = null;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 1;
        }

        private void ReturnToLogin_Click(object sender, RoutedEventArgs e)
        {
            Grottehollet.SelectedIndex = 0;
        }
    }
}
