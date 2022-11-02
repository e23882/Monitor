using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using WebSocketSharp;
using static System.Net.Mime.MediaTypeNames;

namespace Harmos
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel main;
        
        public MainWindow()
        {
            InitializeComponent();
            main = new MainViewModel();
            this.DataContext = main;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            main.ws.Send("123");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
