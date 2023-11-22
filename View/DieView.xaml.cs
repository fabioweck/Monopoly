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
        public DieViewModel Dice {  get; set; } = new DieViewModel();
        public int Roll { get; set; }

        public DieView()
        {
            
            InitializeComponent();
            GetNumbers();

        }

        private async void GetNumbers()
        {
            int countDouble = 0;
            while(true)
            {
                int[] face = Dice.RollDice();

                if (face[0] == face[1])
                {
                    countDouble++;
                    if (countDouble == 3)
                    {
                        lblDiceResult.Content = $"You got 3 doubles. Go to the prison...";
                        MovePlayerAndClose();
                        Roll = 0;
                        break;
                    }

                    lblDiceResult.Content = $"Die 1 face: {face[0]} || Die 2 face: {face[1]}. Double! Roll dice again...";
                    await Task.Delay(2000);
                    continue;
                }
                else
                {
                    Roll = face[0] + face[1];
                    lblDiceResult.Content = $"Die 1 face: {face[0]} || Die 2 face: {face[1]}. Move {Roll} places!";
                    MovePlayerAndClose();
                    break;
                } 
            }
            
        }

        private async void MovePlayerAndClose()
        {
            await Task.Delay(2000);
            this.Close();
        }
    }
}
