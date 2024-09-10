using Firmament.Module;
using Firmament.Utils;
using Firmament.Utils.QuardTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Firmament
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //frame.Navigate(new EasyQuardTreeTestPage());
            frame.Navigate(new QuardTreeTestPage());
            //frame.Navigate(new SudokuGamePage());
            //GamePage game = new GamePage();
            //Common.InitCommon(game);
            //frame.Navigate(game);

        }
    }
}
