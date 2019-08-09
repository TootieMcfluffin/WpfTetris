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
using WpfTetris.Extensions;
using WpfTetris.Models;
using WpfTetris.ViewModels;
using This = WpfTetris.Views.CrazyTetris;

namespace WpfTetris.Views
{
    /// <summary>
    /// Interaction logic for CrazyTetris.xaml
    /// </summary>
    public partial class CrazyTetris : Page
    {
        private GameViewModel Game
        {
            get { return this.DataContext as GameViewModel; }
            set { this.DataContext = value; }
        }

        public CrazyTetris()
        {
            this.Game = new GameViewModel();
            this.Game.SetToCrazy(true);
            this.InitializeComponent();
            This.SetupField(this.field, this.Game.Field.Cells, 30);
            This.SetupField(this.nextField, this.Game.NextField.Cells, 18);
            this.Game.Play();
        }

        private static void SetupField(Grid field, CellViewModel[,] cells, byte blockSize)
        {
            //--- 行/列の定義
            for (int r = 0; r < cells.GetLength(0); r++)
                field.RowDefinitions.Add(new RowDefinition { Height = new GridLength(blockSize, GridUnitType.Pixel) });

            for (int c = 0; c < cells.GetLength(1); c++)
                field.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(blockSize, GridUnitType.Pixel) });

            //--- セル設定
            foreach (var item in cells.WithIndex())
            {
                //--- 背景色をバインド
                var brush = new SolidColorBrush();
                var control = new TextBlock
                {
                    DataContext = item.Element,
                    Background = brush,
                    Margin = new Thickness(1),
                };
                BindingOperations.SetBinding(brush, SolidColorBrush.ColorProperty, new Binding("Color.Value"));

                //--- 位置を決めて子要素として追加
                Grid.SetRow(control, item.X);
                Grid.SetColumn(control, item.Y);
                field.Children.Add(control);
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs ev)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += (s, e) =>
            {
                switch (e.Key)
                {
                    case Key.Z: this.Game.Field.RotationTetrimino(RotationDirection.Left); break;
                    case Key.X: this.Game.Field.RotationTetrimino(RotationDirection.Right); break;
                    case Key.Up: this.Game.Field.RotationTetrimino(RotationDirection.Right); break;
                    case Key.Right: this.Game.Field.MoveTetrimino(MoveDirection.Right); break;
                    case Key.Down: this.Game.Field.MoveTetrimino(MoveDirection.Down); break;
                    case Key.Left: this.Game.Field.MoveTetrimino(MoveDirection.Left); break;
                    case Key.Escape: this.NavigationService.Navigate(new Uri("Views/MainMenu.xaml", UriKind.Relative)); break;
                    case Key.Space: this.Game.Field.ForceFixTetrimino(); break;
                }
            };
        }
    }
}
