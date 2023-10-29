using Monopoly.View;
using Monopoly.ViewModel;
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

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public PlayerViewModel Player { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Player = new PlayerViewModel();
            lblPlayer1.DataContext = Player;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.DieView dieView = new View.DieView(Player);
            dieView.Show();
            //Label dice = new Label();
            //dice.Content = "Die 1: 3, Die 2: 4. Move 7 places.";
            //dice.FontSize = 20;
            //dice.Background = Brushes.White;
            //Grid.SetRow(dice, 5);
            //Grid.SetColumn(dice, 5);
            //Grid.SetColumnSpan(dice, 5);
            //BoardGrid.Children.Add(dice);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Player.MovePlayer(3, 4);
        }
    }
}
