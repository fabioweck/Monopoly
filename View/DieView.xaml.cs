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
using System.Windows.Shapes;

namespace Monopoly.View
{
    /// <summary>
    /// Interaction logic for DieView.xaml
    /// </summary>
    public partial class DieView : Window
    {
        public DieViewModel Dice {  get; set; }
        public int Moves { get; set; }
        public PlayerViewModel Player { get; set; }

        public DieView(PlayerViewModel player)
        {
            
            InitializeComponent();

            Player = player;

            Dice = new DieViewModel();
            int[] face = Dice.RollDice();
            Moves = face[0] + face[1];
            lblDiceResult.Content = $"Die 1 face: {face[0]} || Die 2 face: {face[1]}. Move {Moves} places!";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Player.MovePlayer(Moves);
            this.Close();
        }
    }
}
