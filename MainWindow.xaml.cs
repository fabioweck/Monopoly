using Monopoly.Model;
using Monopoly.View;
using Monopoly.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
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

        public static int NumberOfPlayers = 0;
        public static List<TextBox> txtBoxPanelPlayers;
        public static bool isBankrupt = false;
        public static string jailLogic;
        public List<Label> LblPlayers = new List<Label>();
        public CardViewModel Cards;
        public static List<PlayerPropertiesPanel> PlayerPropertiesPanels = new List<PlayerPropertiesPanel>();

        LodgingViewModel lodgingViewModel = new LodgingViewModel();

        public MainWindow()
        {
            InitializeComponent();

            Cards = new CardViewModel();
            SpaceViewModel.PopulateBoard();

            txtBoxPanelPlayers = new List<TextBox>() { P1, P2, P3, P4 };

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            btnRollDice.Focus();

        }

        //Method to roll dice
        private void btnRollDice_Click(object sender, RoutedEventArgs e)
        {

            jailLogic = "false";

            if (PlayerViewModel.CurrentPlayer.IsInJail)
                jailLogic = SpaceViewModel.ResolveJail(PlayerViewModel.CurrentPlayer, BoardGrid, this);

            if (jailLogic == "return") return;

            //If the dice are not being rolled to resolve prison logic, then proceed
            if (jailLogic == "false")
            {
                //If the user closes the window and does not choose any option to roll dice,
                //then open the window again
                while (DieView.Click != true)
                {
                    DieView dieView = new DieView(PlayerViewModel.CurrentPlayer.Name);

                    //Open dice window to roll dice
                    dieView.txtPlayer.Text = $"Player {PlayerViewModel.CurrentPlayer.Name}, how would you like to move?";
                    dieView.ShowDialog();
                }
            }

            DieView.Click = false;

            //Get the result
            int move = DieView.Roll;


            //If move is zero, it means that the player rolled dice and got 3 doubles
            //Send the player to the jail
            if (move != 0)
                PlayerViewModel.CurrentPlayer.MovePlayer(move);
            else
            {
                PlayerViewModel.GoToJail();
                ChangePlayer();
                return;
            }

            ResolveLogic();

        }

        //Method to ask number of players
        private void HowManyPlayers(object sender, EventArgs e)
        {
            while (NumberOfPlayers == 0)
            {
                PlayerCountQuestion playerCount = new PlayerCountQuestion();
                playerCount.ShowDialog();
            }

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                new PlayerViewModel();

                //-----------------------------------------------------
                Label myLabel = new Label();

                // Create a source object
                PlayerViewModel myData = PlayerViewModel.Players[i];

                // Create a Binding object
                Binding bindingRow = new Binding("Row");
                Binding bindingColumn = new Binding("Column");
                Binding bindingName = new Binding("Name");

                // Set the source of the binding to the data object
                bindingRow.Source = myData;
                bindingColumn.Source = myData;
                bindingName.Source = myData;

                // Apply the binding to the Label's Content property
                myLabel.SetBinding(ContentProperty, bindingName);
                myLabel.SetBinding(Grid.RowProperty, bindingRow);
                myLabel.SetBinding(Grid.ColumnProperty, bindingColumn);

                SolidColorBrush playerColorBrush = myData.Color;
                myLabel.Background = new SolidColorBrush(playerColorBrush.Color); 

                myLabel.Name = $"Player{i}";
                //PlayerViewModel.Players[i].playerLabel = myLabel;

                Panel.SetZIndex(myLabel, 2);

                //-----------------------------------------------------

                // Iterate through our dictionary and create data binding to properly display on View:

                //Label label = new Label();

                //label.Content = Players[i].Name;
                //Grid.SetRow(label, Players[i].Row);
                //Grid.SetColumn(label, Players[i].Column);


                LblPlayers.Add(myLabel);
                BoardGrid.Children.Add(myLabel);

                PlayerPropertiesPanel pp = new PlayerPropertiesPanel(myData, SpaceViewModel.propertyOwners, BoardGrid);
                BoardGrid.Children.Add(pp);
                Grid.SetColumnSpan(pp, 5);
                if (i == 0 || i == 2)
                    Grid.SetColumn(pp, 27);
                else
                {
                    Grid.SetColumn(pp, 32);
                    Grid.SetColumnSpan(pp, 6);
                }
                if (i == 0 || i == 1)
                    Grid.SetRow(pp, 5);
                else
                    Grid.SetRow(pp, 16);

                Grid.SetZIndex(pp, 5);
                Grid.SetRowSpan(pp, 7);
                PlayerPropertiesPanels.Add(pp);


                // TODO - Set Panel.Zindex on boardGrid (each player should have the ZIndex set to 1 or higher)

                txtBoxPanelPlayers[i].Visibility = Visibility.Visible;
                //System.Diagnostics.Debug.WriteLine(txtBoxPanelPlayers[i]);
            }

            foreach (var space in SpaceViewModel.spaceModels)
            {
                // Create a new control:
                Image image = new Image();

                // Create the data source:
                SpaceModel sm = space.Value;

                // Create the Binding objects to set the path to the properties:
                Binding bindRow = new Binding("Row");
                Binding bindCol = new Binding("Column");
                Binding bindRowSpan = new Binding("RowSpan");
                Binding bindColSpan = new Binding("ColumnSpan");
                Binding bindImg = new Binding("ImgSrc");

                // Set the source to the data source previously defined:
                bindRow.Source = sm;
                bindCol.Source = sm;
                bindRowSpan.Source = sm;
                bindColSpan.Source = sm;
                bindImg.Source = sm;

                // Apply the binding to the controls' properties:
                image.SetBinding(Grid.RowProperty, bindRow);
                image.SetBinding(Grid.ColumnProperty, bindCol);
                image.SetBinding(Grid.RowSpanProperty, bindRowSpan);
                image.SetBinding(Grid.ColumnSpanProperty, bindColSpan);
                image.SetBinding(Image.SourceProperty, bindImg);
                BoardGrid.Children.Add(image);
            }

            // UpdatePlayerPanel for each player
            foreach (TextBox textBox in txtBoxPanelPlayers)
            {
                foreach (PlayerViewModel player in PlayerViewModel.Players)
                {
                    if (textBox.Name == player.Name)
                    {
                        SpaceViewModel.UpdatePlayerPanel(textBox, player);
                    }
                }
            }
        }

        // After rolling the dice, resolve game's logic:
        public async void ResolveLogic()
        {

            await Task.Delay(400);

            SpaceViewModel.Resolve(BoardGrid, txtBoxPanelPlayers, lodgingViewModel.AddLodgingToBoard, this);

            UpdateAllPlayersPanel();

            //If the last turn was a player getting out of prison due to a double,
            //then the double doesn't count to play again and call next player
            if(jailLogic == "true")
            {
                ChangePlayer();
                return;
            }

            //If the player got double and did not go bankrupt, then roll dice again
            if (DieView.Double != 0 && PlayerViewModel.Players.Contains(PlayerViewModel.CurrentPlayer)) return;

            ChangePlayer();

        }

        // Call the next player to roll the dice
        public void ChangePlayer()
        {
            int _ind = PlayerViewModel.CurrentPlayer.instanceNumber;
             // If the player is the last in the list, call the first one
            if (PlayerViewModel.Players.IndexOf(PlayerViewModel.CurrentPlayer) >= PlayerViewModel.Players.Count - 1)
                PlayerViewModel.CurrentPlayer = PlayerViewModel.Players[0];

            // If not the last player
            else if (!isBankrupt)
            {
                // Call the next player
                PlayerViewModel.CurrentPlayer = PlayerViewModel.Players[_ind+1];
            }
            else if (isBankrupt)
            {
                // Preserve current index to give the turn to the next player, unless there is only 1 player left.
                if (PlayerViewModel.Players.Count > 1)
                    if (_ind >= PlayerViewModel.Players.Count) 
                        _ind = 0;
                    PlayerViewModel.CurrentPlayer = PlayerViewModel.Players[_ind];
            }
            DieView.Double = 0;
            isBankrupt = false;
        }

        // Updates the players panel with current information
        public void UpdateAllPlayersPanel()
        {
            foreach (TextBox textBox in txtBoxPanelPlayers)
            {
                foreach (PlayerViewModel player in PlayerViewModel.Players)
                {
                    if (textBox.Name == player.Name)
                    {
                        SpaceViewModel.UpdatePlayerPanel(textBox, player);
                    }
                }
            }        
            foreach (PlayerPropertiesPanel pp in PlayerPropertiesPanels)
            {
                pp.Update();
            }
        }

        //Method to allow users to press enter when on the main screen and roll dice
        private void DiceKeyDown(object sender, KeyEventArgs e)
        {
            //If key down was enter, then roll dice
            if(e.Key == Key.Enter)
            {
                btnRollDice_Click(this, new RoutedEventArgs());
            }
        }
    }
}

