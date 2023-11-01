using Monopoly.ViewModel;
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
using System.Windows.Shapes;

namespace Monopoly.View
{
    /// <summary>
    /// Interaction logic for DieView.xaml
    /// </summary>
    public partial class DieView : Window
    {
        public DieViewModel Dice {  get; set; }
        public int Roll { get; set; }

        public DieView()
        {
            
            InitializeComponent();

            Dice = new DieViewModel();
            int[] face = Dice.RollDice();
            Roll = face[0] + face[1];
            lblDiceResult.Content = $"Die 1 face: {face[0]} || Die 2 face: {face[1]}. Move {Roll} places!";
            MovePlayerAndClose();

        }

        private async void MovePlayerAndClose()
        {
            await Task.Delay(2000);
            this.Close();
        }
    }
}
