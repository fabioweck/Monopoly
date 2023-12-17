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
        public DieViewModel Dice { get; set; } = new DieViewModel();
        public static int Roll { get; set; }
        public static int Double { get; set; } = 0;
        public string PlayerName;
        public static bool Click = false;

        public DieView(string name)
        {

            InitializeComponent();
            PlayerName = name;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            btnAuto.Focus();
        }

        //Method to generante dice numbers
        private void GetNumbers(bool isDouble = false)
        {       
            //Repeat rolling dice until getting a result to exit the loop
            while (true)
            {
                //Check if the button was "debugging double" or not
                if (!isDouble)
                {
                    //Auto roll dice
                    int[] face = Dice.RollDice();

                    //If gets a double
                    if (face[0] == face[1])
                    {
                        //Count +1 double
                        Double++;

                        //Define roll total number
                        Roll = face[0] + face[1];

                        //If the player is in jail, they get out of jail
                        if (PlayerViewModel.CurrentPlayer.IsInJail)
                        {
                            txtPlayer.Text = $"Player {PlayerName} rolled the dice.";
                            txtDiceResult.Text = $"Die 1: {face[0]}\nDie 2: {face[1]}\nDouble?! You are free! Move {Roll} spaces!";
                            PlayerViewModel.CurrentPlayer.AttemptsToGetOutOfJail = 0;
                            PlayerViewModel.CurrentPlayer.IsInJail = false;
                            CountTimeAndClose();
                            break;
                        }

                        //If is not in jail and gets 3 double, go to jail
                        if (Double == 3)
                        {
                            txtDiceResult.Text = $"You got 3 doubles. Go to jail...";
                            CountTimeAndClose();
                            Roll = 0;
                            break;
                        }

                        //Else, move
                        txtPlayer.Text = $"Player {PlayerName} rolled the dice.";
                        txtDiceResult.Text = $"Die 1: {face[0]}\nDie 2: {face[1]}\nDouble! Move {Roll} spaces!";
                        CountTimeAndClose();
                        break;

                    }
                    else
                    {
                        //Define roll total number
                        Roll = face[0] + face[1];

                        //If the playeris in jail, can't leave without a double
                        if (PlayerViewModel.CurrentPlayer.IsInJail)
                        {
                            txtPlayer.Text = $"Player {PlayerName} rolled the dice.";
                            txtDiceResult.Text = $"Die 1: {face[0]}\nDie 2: {face[1]}\nIt's not a double - you stay in jail!";
                            PlayerViewModel.CurrentPlayer.AttemptsToGetOutOfJail++;
                            CountTimeAndClose();
                            return;
                        }

                        //Else, move
                        Double = 0;
                        txtPlayer.Text = $"Player {PlayerName} rolled the dice.";
                        txtDiceResult.Text = $"Die 1: {face[0]}\nDie 2: {face[1]}\nMove {Roll} spaces!";
                        CountTimeAndClose();
                        break;
                    }
                }
                //If it is a forced double, then proceed
                else
                {
                    int[] face = { 2, 2 }; //Alter the number from 1 to 6
                    Double++;
                    Roll = face[0] + face[1];

                    //If player is in jail, then leave
                    if (PlayerViewModel.CurrentPlayer.IsInJail)
                    {
                        txtPlayer.Text = $"Player {PlayerName} rolled the dice.";
                        txtDiceResult.Text = $"Die 1: {face[0]}\nDie 2: {face[1]}\nDouble?! You are free! Move {Roll} spaces.";
                        PlayerViewModel.CurrentPlayer.IsInJail = false;
                        PlayerViewModel.CurrentPlayer.AttemptsToGetOutOfJail = 0;
                        CountTimeAndClose();
                        break;
                    }

                    //If the player gets 3 doubles in a row, go to prison
                    if (Double == 3)
                    {
                        txtDiceResult.Text = $"You got 3 doubles. Go to prison...";
                        CountTimeAndClose();
                        Roll = 0;
                        break;
                    }

                    //Else, move
                    txtPlayer.Text = $"Player {PlayerName} rolled the dice.";
                    txtDiceResult.Text = $"Die 1: {face[0]}\nDie 2: {face[1]}\nDouble! Move {Roll} spaces.";
;
                    CountTimeAndClose();
                    break;
                }
            }
        }

        //Method to display a counter to the player
        private async void CountTimeAndClose()
        {
            timerImage.Visibility = Visibility.Visible;
            lblTimer.Visibility = Visibility.Visible;
            for (int i = 0; i < 3; i++)
            {
                lblTimer.Content = $"{3 - i}s";
                await Task.Delay(1000);
            }
            this.Close();
        }

        //Display all items related to auto roll
        private void btnAuto_Click(object sender, RoutedEventArgs e)
        {
            Click = true;
            gpBoxDebug.Visibility = Visibility.Hidden;
            btnAuto.Visibility = Visibility.Hidden;
            btnManual.Visibility = Visibility.Hidden;
            btnMove.Visibility = Visibility.Hidden;
            btnDouble.Visibility = Visibility.Hidden;
            GetNumbers();
        }

        //Display all item related to manual move
        private void btnManual_Click(object sender, RoutedEventArgs e)
        {
            Click = true;
            Double = 0;
            txtPlayer.Text = $"Enter the number of places {PlayerName} will move. (Range: 0 - 40)";
            gpBoxDebug.Visibility = Visibility.Hidden;
            btnAuto.Visibility = Visibility.Hidden;
            btnManual.Visibility = Visibility.Hidden;
            btnDouble.Visibility = Visibility.Hidden;
            btnMove.Visibility = Visibility.Visible;
            btnMove.Content = $"Move {PlayerName}";
            txtNumberOfPlaces.Visibility = Visibility.Visible;
            txtNumberOfPlaces.Focus();
        }

        //Move the player with manual move
        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            Click = true;

            // Check and parse the input to int number and check the valid range
            if(int.TryParse(txtNumberOfPlaces.Text, out int result) && result >= 0 && result <= 40)
            {
                Roll = result;

                //Debug mode - simulate that the player couldn't get a double
                if (PlayerViewModel.CurrentPlayer.IsInJail)
                {
                    txtPlayer.Text = $"Player {PlayerName} rolled the dice.";
                    txtDiceResult.Text = $"It's not a double - you stay in jail!";
                    btnMove.Visibility = Visibility.Hidden;
                    txtNumberOfPlaces.Visibility = Visibility.Hidden;
                    PlayerViewModel.CurrentPlayer.AttemptsToGetOutOfJail++;
                    CountTimeAndClose();
                }
                else
                {
                    this.Close();
                }     
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        //Method to simulate a double and display all itens related to double
        private void btnDouble_Click(object sender, RoutedEventArgs e)
        {
            Click = true;
            gpBoxDebug.Visibility = Visibility.Hidden;
            btnAuto.Visibility = Visibility.Hidden;
            btnManual.Visibility = Visibility.Hidden;
            btnMove.Visibility = Visibility.Hidden;
            btnDouble.Visibility = Visibility.Hidden;
            GetNumbers(true); //Add any number to the method to get double
        }

        //Method to limit wrong input
        private void txtNumberOfPlaces_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numeric input
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        //Method to allow user move pressing "enter" key
        private void txtNumberOfPlaces_KeyDown(object sender, KeyEventArgs e)
        {
            // Invoke method pressing enter key
            if (e.Key == Key.Enter)
            {
                btnMove_Click(sender, e);
            }
        }
        


    }
}
