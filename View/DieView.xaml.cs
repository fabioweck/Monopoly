﻿using Monopoly.ViewModel;
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
        public DieViewModel Dice { get; set; } = new DieViewModel();
        public static int Roll { get; set; }
        public static int Double { get; set; } = 0;
        public string PlayerName;

        public DieView(string name)
        {

            InitializeComponent();
            PlayerName = name;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        private async void GetNumbers(int isDouble = 0)
        {

            while (true)
            {
                if (isDouble == 0)
                {
                    int[] face = Dice.RollDice();

                    if (face[0] == face[1])
                    {

                        Double++;

                        if (Double == 3)
                        {
                            lblDiceResult.Content = $"You got 3 doubles. Go to the prison...";
                            MovePlayerAndClose();
                            Roll = 0;
                            break;
                        }

                        Roll = face[0] + face[1];
                        lblPlayer.Content = $"Player {PlayerName} rolled dice.";
                        lblDiceResult.Content = $"Die 1 face: {face[0]} || Die 2 face: {face[1]}. Double!";
                        MovePlayerAndClose();
                        break;

                    }
                    else
                    {
                        Double = 0;
                        Roll = face[0] + face[1];
                        lblPlayer.Content = $"Player {PlayerName} rolled dice.";
                        lblDiceResult.Content = $"Die 1 face: {face[0]} || Die 2 face: {face[1]}. Move {Roll} places!";
                        MovePlayerAndClose();
                        break;
                    }
                }

                else
                {
                    int[] face = { 2, 2 }; //Alter the number from 1 to 6

                    Double++;

                    if (Double == 3)
                    {
                        lblDiceResult.Content = $"You got 3 doubles. Go to the prison...";
                        MovePlayerAndClose();
                        Roll = 0;
                        break;
                    }

                    Roll = face[0] + face[1];
                    lblPlayer.Content = $"Player {PlayerName} rolled dice.";
                    lblDiceResult.Content = $"Die 1 face: {face[0]} || Die 2 face: {face[1]}. Double!";
                    MovePlayerAndClose();
                    break;
                }
            }
        }
        private async void MovePlayerAndClose()
        {
            for (int i = 0; i < 3; i++)
            {
                lblTimer.Content = $"Counter: {3 - i}s";
                await Task.Delay(1000);
            }
            this.Close();
        }
        private void btnAuto_Click(object sender, RoutedEventArgs e)
        {
            btnAuto.Visibility = Visibility.Hidden;
            btnManual.Visibility = Visibility.Hidden;
            btnMove.Visibility = Visibility.Hidden;
            btnDouble.Visibility = Visibility.Hidden;
            GetNumbers();
        }

        private void btnManual_Click(object sender, RoutedEventArgs e)
        {
            Double = 0;
            lblPlayer.Content = $"Type in the number of places {PlayerName} will move.";
            btnAuto.Visibility = Visibility.Hidden;
            btnManual.Visibility = Visibility.Hidden;
            btnDouble.Visibility = Visibility.Hidden;
            btnMove.Visibility = Visibility.Visible;
            btnMove.Content = $"Move {PlayerName}";
            txtNumberOfPlaces.Visibility = Visibility.Visible;
            txtNumberOfPlaces.Focus();
        }
        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            Roll = Convert.ToInt32(txtNumberOfPlaces.Text);
            this.Close();
        }

        private void btnDouble_Click(object sender, RoutedEventArgs e)
        {
            btnAuto.Visibility = Visibility.Hidden;
            btnManual.Visibility = Visibility.Hidden;
            btnMove.Visibility = Visibility.Hidden;
            GetNumbers(1); //Add any number to the method to get double
        }

        private void btnDouble_Click(object sender, RoutedEventArgs e)
        {
            btnAuto.Visibility = Visibility.Hidden;
            btnManual.Visibility = Visibility.Hidden;
            btnMove.Visibility = Visibility.Hidden;
            GetNumbers(1); //Add any number to the method to get double
        }
    }
}
