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
        public List<Label> LblPlayers = new List<Label>();
        public static List<TextBox> txtBoxPanelPlayers;
        public CardViewModel Cards;
        public static bool isBankrupt = false;

        LodgingViewModel lodgingViewModel = new LodgingViewModel();

        public MainWindow()
        {
            InitializeComponent();

            Cards = new CardViewModel();
            SpaceViewModel.PopulateBoard();

            txtBoxPanelPlayers = new List<TextBox>() { P1, P2, P3, P4 };

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            btnRollDice.Focus();

            //CardView = new CardViewModel();
            //Card1.DataContext = CardView;
        }

        private void RollDice_Click(object sender, RoutedEventArgs e)
        {
            //If the user closes the window and does not choose any number of players
            //opens the window again

            while(DieView.Click != true)
            {
                DieView dieView = new DieView(PlayerViewModel.CurrentPlayer.Name);

                //Open dice window to roll dice
                dieView.lblPlayer.Content = $"Player {PlayerViewModel.CurrentPlayer.Name}, select how to roll dice";
                dieView.ShowDialog();
            }

            DieView.Click = false;

            //Get the result
            int move = DieView.Roll;


            //If move is zero, it means that the player rolled dice and got 3 doubles
            //Send the player to the jail
            if (move != 0) PlayerViewModel.CurrentPlayer.MovePlayer(move);
            else
            {
                PlayerViewModel.GoToJail();
                ChangePlayer();
                return;
            }

            ResolveLogic();
        }

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
        public void ResolveLogic()
        {

            SpaceViewModel.Resolve(BoardGrid, txtBoxPanelPlayers, lodgingViewModel.AddLodgingToBoard, this);

            UpdateAllPlayersPanel();

            if (DieView.Double != 0) return;

            ChangePlayer();

        }

        // Call the next player to roll the dice
        private void ChangePlayer()
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
        }
    }
}

