using Firmament.Utils;
using System.Windows;

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
            //frame.Navigate(new SudokuGamePage());
            GamePage game = new GamePage();
            Common.InitCommon(game);
            frame.Navigate(game);

        }
    }
}
