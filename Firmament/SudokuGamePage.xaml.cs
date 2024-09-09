using Firmament.Module;
using Firmament.Utils.QuardTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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

namespace Firmament
{
    /// <summary>
    /// QuardTreeTestPage.xaml 的交互逻辑
    /// </summary>
    public partial class SudokuGamePage : Page
    {
        int gridSize = 4;
        public SudokuGamePage()
        {
            InitializeComponent();
            this.Loaded += SudokuGamePage_Loaded;
        }

        private void SudokuGamePage_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < gridSize; i++) {
                grid_root.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(1,GridUnitType.Star) });
                grid_root.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }
            int[,] array = new int[gridSize, gridSize];
            Random random = new Random();
            List<int> numbers = new List<int>();
            int len = gridSize * gridSize - 1;
            // 初始化数字列表
            for (int i = 1; i <= len; i++)
            {
                numbers.Add(i);
            }

            // 使用Fisher-Yates算法洗牌 ,   倒序，生成一个随机数，进行互换
            for (int i = len; i > 1; i--)
            {
                int j = random.Next(i);
                int temp = numbers[i - 1];
                numbers[i - 1] = numbers[j];
                numbers[j] = temp;
            }

            int index = 0;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (i == gridSize-1 && j == gridSize-1) {
                        Label label2 = new Label()
                        {
                            Content = "",
                            FontSize = 38,
                            Background = Brushes.Gray,
                            Foreground = Brushes.Red
                        };
                        Grid.SetColumn(label2, j);
                        Grid.SetRow(label2, i);
                        grid_root.Children.Add(label2);
                        break;
                    }
                    array[i, j] = numbers[index++];
                    Label label = new Label()
                    {
                        Content = array[i, j].ToString(),
                        FontSize = 38,
                        Background = Brushes.Gray,
                        Foreground = Brushes.Red
                    };
                    Grid.SetColumn(label, j);
                    Grid.SetRow(label, i);
                    grid_root.Children.Add(label);
                }
            }
            Console.WriteLine("填充完毕");
        }

        bool isClick = false;
        int column;
        int row;
        int distance_x, distance_y;
        Point origin_point;
        private void grid_root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isClick = true;
            var fe2 = (FrameworkElement)e.Source;
            column =  Grid.GetColumn(fe2);
            row = Grid.GetRow(fe2);
            origin_point = e.GetPosition(this);
        }

        private void grid_root_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isClick)
            {
                Point point = e.GetPosition(this);
                distance_x = (int)(point.X - origin_point.X);
                distance_y = (int)(point.Y - origin_point.Y);
                isClick = false;

                if (Math.Abs(distance_y) < 50 && Math.Abs(distance_x) < 50)
                {
                    //无效滑动不处理
                }
                else {
                    if (Math.Abs(distance_x) > Math.Abs(distance_y))
                    {
                        if (distance_x > 0)
                        {
                            Console.WriteLine("向右滑动");

                            UIElement orgin = GetControlFromGrid(grid_root,row,column);
                            UIElement targat = GetControlFromGrid(grid_root, row, column +1);
                            if ((targat as Label).Content.ToString() == "") {
                                Grid.SetColumn(orgin, column + 1);
                                Grid.SetColumn(targat, column);
                            }
                        }
                        else
                        {
                            Console.WriteLine("向左滑动");
                            UIElement orgin = GetControlFromGrid(grid_root, row, column);
                            UIElement targat = GetControlFromGrid(grid_root, row, column - 1);
                            if ((targat as Label).Content.ToString() == "")
                            {
                                Grid.SetColumn(orgin, column - 1);
                                Grid.SetColumn(targat, column);
                            }
                        }
                    }
                    else
                    {
                        if (distance_y > 0)
                        {
                            Console.WriteLine("向下滑动");
                            UIElement orgin = GetControlFromGrid(grid_root, row, column);
                            UIElement targat = GetControlFromGrid(grid_root, row+1, column);
                            if ((targat as Label).Content.ToString() == "")
                            {
                                Grid.SetRow(orgin, row + 1);
                                Grid.SetRow(targat, row);
                            }
                        }
                        else
                        {
                            Console.WriteLine("向上滑动");
                            UIElement orgin = GetControlFromGrid(grid_root, row, column);
                            UIElement targat = GetControlFromGrid(grid_root, row - 1, column);
                            if ((targat as Label).Content.ToString() == "")
                            {
                                Grid.SetRow(orgin, row - 1);
                                Grid.SetRow(targat, row);
                            }
                        }
                    }
                }
                //判断是否胜利
                AdjustResult(grid_root);
            }
        }

        private UIElement GetControlFromGrid(Grid grid, int row, int column)
        {
            foreach (UIElement child in grid.Children)
            {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column)
                {
                    return child;
                }
            }
            return null;
        }
        private void AdjustResult(Grid grid) {
            bool isSucess = true;
            foreach (UIElement child in grid.Children)
            {
                if ((child as Label).Content.ToString() == "") {
                    continue;
                }
                int row = Grid.GetColumn(child);
                int column = Grid.GetRow(child);
                if ((row * gridSize + column + 1) + "" != (child as Label).Content.ToString()) {
                    isSucess = false;
                    break;
                }
            }
            if (isSucess) {
                MessageBox.Show("you are sucess");
            }

        }
    }
}
