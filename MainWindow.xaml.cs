using Monopoly.Model;
using Monopoly.View;
using Monopoly.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
        public List<Label> LblPlayersBalance;

        public MainWindow()
        {
            InitializeComponent();
            SpaceViewModel.PopulateBoard();
            //LodgingViewModel.PopulateBoard();

            LblPlayersBalance = new List<Label>() { P1, P2, P3, P4 };

            //CardView = new CardViewModel();
            //Card1.DataContext = CardView;
        }

        private void RollDice_Click(object sender, RoutedEventArgs e)
        {
            //Open dice window to roll dice
            DieView dieView = new DieView();
            dieView.ShowDialog();

            //Get the result
            int move = dieView.Roll;

            //If move is zero, it means that the player rolled dice and got 3 doubles
            //Send the player to the jail
            if (move != 0) PlayerViewModel.CurrentPlayer.MovePlayer(move);
            else
            {
                GoToJail();
                ChangePlayer();
                return;
            }

            ResolveLogic();
        }

        private SolidColorBrush GetPlayerColor(int instanceNumber)
        {
            switch (instanceNumber)
            {
                case 0: // Player 1
                    return new SolidColorBrush(Color.FromRgb(0, 102, 204));
                case 1: // Player 2
                    return new SolidColorBrush(Color.FromRgb(255, 140, 0));
                case 2: // Player 3
                    return new SolidColorBrush(Color.FromRgb(255, 69, 0)); 
                case 3: // Player 4
                    return new SolidColorBrush(Color.FromRgb(34, 139, 34)); 
                default:
                    return new SolidColorBrush(Colors.Gray);
            }
        }


        private void HowManyPlayers(object sender, EventArgs e)
        {
            PlayerCountQuestion playerCount = new PlayerCountQuestion();
            playerCount.ShowDialog();

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                new PlayerViewModel();

                //-----------------------------------------------------
                Label myLabel = new Label();

                // Create a source object (in this case, a simple class with a property)
                PlayerViewModel myData = PlayerViewModel.Players[i];

                // Create a Binding object and set the path to the property you want to bind
                Binding bindingRow = new Binding("Row");
                Binding bindingColumn = new Binding("Column");
                Binding bindingName = new Binding("Name");

                // Set the source of the binding to your data object
                bindingRow.Source = myData;
                bindingColumn.Source = myData;
                bindingName.Source = myData;

                // Apply the binding to the Label's Content property
                myLabel.SetBinding(Label.ContentProperty, bindingName);
                myLabel.SetBinding(Grid.RowProperty, bindingRow);
                myLabel.SetBinding(Grid.ColumnProperty, bindingColumn);

                SolidColorBrush playerColorBrush = GetPlayerColor(myData.instanceNumber);
                myLabel.Background = new SolidColorBrush(playerColorBrush.Color); 


                myLabel.Name = $"Player{i}";

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

                LblPlayersBalance[i].Visibility = Visibility.Visible;
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
        }

        // After rolling the dice, resolve game's logic:
        public void ResolveLogic()
        {
            CheckPlayerOverProperty();

            ChangePlayer();

            CheckBankruptcy();
        }

        //Check the player over properties and do the logic
        public async void CheckPlayerOverProperty()
        {
            
            //Get both current player and current property
            PlayerViewModel currentPlayer = PlayerViewModel.CurrentPlayer;
            var currentSpace = SpaceViewModel.spaceModels[PlayerViewModel.CurrentPlayer.Position];
            SolidColorBrush playerColorBrush = new SolidColorBrush();

            if (currentPlayer.Position == 30)
            {
                MessageBox.Show("Go to the jail...", ":(", MessageBoxButton.OK);
                GoToJail();
                return;
            }
            //Check if the space is a property
            if (currentSpace.GetType() == typeof(PropertyModel))
            {
                //if yes, cast to Property type
                PropertyModel property = (PropertyModel)currentSpace;

                //Once the property has no owner, offer to buy it to the current player
                if (property.Owner == null)
                {
                    
                    MessageBoxResult result = MessageBox.Show($"Would you like to buy this property for ${property.Price}?", "Landed on a private property.", MessageBoxButton.YesNo);

                    //If the player wants to buy the property, pass the function to balance to perform the calculation
                    if (result == MessageBoxResult.Yes)
                    {
                        currentPlayer.ChangeBalance(value => currentPlayer.Balance -= value, property.Price);
                        property.Owner = PlayerViewModel.CurrentPlayer;

                        // Create a label for the property
                        Label propertyLabel = new Label();
                        propertyLabel.Content = property.Owner.Name;
                        propertyLabel.FontSize = 12;
                        propertyLabel.FontWeight = FontWeights.Bold;

                        // Set the Grid row and column
                        //Top
                        if(property.Row >= 0 && property.Row <= 3 && property.Column >= 0 && property.Column <= 21)
                        {
                            propertyLabel.SetValue(Grid.RowProperty, property.Row + 3);
                            propertyLabel.SetValue(Grid.ColumnProperty, property.Column);
                            propertyLabel.SetValue(Grid.ColumnSpanProperty, property.ColumnSpan);
                            propertyLabel.SetValue(Grid.RowSpanProperty, property.RowSpan);
                        }
                        //Right
                        if (property.Row >= 4 && property.Row <= 21 && property.Column >= 22 && property.Column <= 24)
                        {
                            propertyLabel.SetValue(Grid.RowProperty, property.Row);
                            propertyLabel.SetValue(Grid.ColumnProperty, property.Column - 1);
                            propertyLabel.SetValue(Grid.ColumnSpanProperty, property.ColumnSpan);
                            propertyLabel.SetValue(Grid.RowSpanProperty, property.RowSpan);
                        }
                        //Bottom
                        if (property.Row >= 22 && property.Row <= 24 && property.Column >= 0 && property.Column <= 21)
                        {
                            propertyLabel.SetValue(Grid.RowProperty, property.Row - 1);
                            propertyLabel.SetValue(Grid.ColumnProperty, property.Column);
                            propertyLabel.SetValue(Grid.ColumnSpanProperty, property.ColumnSpan);
                            propertyLabel.SetValue(Grid.RowSpanProperty, property.RowSpan);
                        }
                        //Left
                        if (property.Row >= 0 && property.Row <= 21 && property.Column >= 0 && property.Column <= 3)
                        {
                            propertyLabel.SetValue(Grid.RowProperty, property.Row);
                            propertyLabel.SetValue(Grid.ColumnProperty, property.Column + 3);
                            propertyLabel.SetValue(Grid.ColumnSpanProperty, property.ColumnSpan);
                            propertyLabel.SetValue(Grid.RowSpanProperty, property.RowSpan);
                        }
                        //Left-top corner
                        if (property.Row >= 4 && property.Row <= 5 && property.Column >= 0 && property.Column <= 3)
                        {
                            propertyLabel.SetValue(Grid.RowProperty, 5);
                            propertyLabel.SetValue(Grid.ColumnProperty, property.Column + 3);
                            propertyLabel.SetValue(Grid.ColumnSpanProperty, property.ColumnSpan);
                            propertyLabel.SetValue(Grid.RowSpanProperty, property.RowSpan);
                        }
                        //Left-bottom corner
                        if (property.Row >= 20 && property.Row <= 21 && property.Column >= 0 && property.Column <= 3)
                        {
                            propertyLabel.SetValue(Grid.RowProperty, 20);
                            propertyLabel.SetValue(Grid.ColumnProperty, property.Column + 3);
                            propertyLabel.SetValue(Grid.ColumnSpanProperty, property.ColumnSpan);
                            propertyLabel.SetValue(Grid.RowSpanProperty, property.RowSpan);
                        }
                        //Right-top corner
                        if (property.Row >= 4 && property.Row <= 5 && property.Column >= 22 && property.Column <= 24)
                        {
                            propertyLabel.SetValue(Grid.RowProperty, 5);
                            propertyLabel.SetValue(Grid.ColumnProperty, property.Column - 1);
                            propertyLabel.SetValue(Grid.ColumnSpanProperty, property.ColumnSpan);
                            propertyLabel.SetValue(Grid.RowSpanProperty, property.RowSpan);
                        }

                        // Label with the same color of the player
                        propertyLabel.Foreground = GetPlayerColor(PlayerViewModel.CurrentPlayer.instanceNumber);
                        Console.WriteLine(playerColorBrush.Color.ToString());

                        // Add the label to the Grid
                        BoardGrid.Children.Add(propertyLabel);

                        // Add a house to the board (Check if the property is a Railroad or Utility, and if so, do not add a house)
                        if (property.Group != "Railroad" && property.Group != "Utility")
                        {
                            AddLodgingToBoard(property);
                        }

                        //Update balance on panel
                        foreach (Label lbl in LblPlayersBalance)
                        {
                            if (lbl.Name == currentPlayer.Name)
                            {
                                lbl.Content = $"{currentPlayer.Name} balance: " + currentPlayer.Balance;
                            }
                        }
                    }
                    else //Otherwise, do nothing
                    {
                        return;
                    }
                }
                else
                {
                    //If the player is the owner, do nothing OR add houses/hotel
                    if (property.Owner == PlayerViewModel.CurrentPlayer) return;

                    //If the player is not the owner, pass the functions to debit from current player and pay rent to the owner
                    currentPlayer.ChangeBalance(value => currentPlayer.Balance -= value, property.Rent[0]);
                    property.Owner.ChangeBalance(value => property.Owner.Balance += value, property.Rent[0]);

                    //Update their balance on the screen
                    foreach (Label lbl in LblPlayersBalance)
                    {
                        if (lbl.Name == currentPlayer.Name)
                        {
                            lbl.Content = $"{currentPlayer.Name} balance: " + currentPlayer.Balance;
                        }
                        if (lbl.Name == property.Owner.Name)
                        {
                            lbl.Content = $"{property.Owner.Name} balance: " + property.Owner.Balance;
                        }
                    }

                    //TODO - check 
                }
            }
        }

        //Call the next player to roll the dice
        private static void ChangePlayer()
        {
            //If the player is the last in the list, call the first one
            if (PlayerViewModel.Players.IndexOf(PlayerViewModel.CurrentPlayer) >= PlayerViewModel.Players.Count - 1)
                PlayerViewModel.CurrentPlayer = PlayerViewModel.Players[0];
            else
            {
                //If not the last, call the next player
                int _ind = PlayerViewModel.Players.IndexOf(PlayerViewModel.CurrentPlayer) + 1;
                PlayerViewModel.CurrentPlayer = PlayerViewModel.Players[_ind];
            }
        }

        //Check if any of the players if bankrupt after game logic
        public void CheckBankruptcy()
        {
            foreach(PlayerViewModel player in PlayerViewModel.Players)
            {
                if(player.Balance <= 0)
                {
                    MessageBox.Show($"{player.Name} has gone bankrupt...");

                    //TODO - remove player and update status on screen (game over?)
                }
            }
        }

        public void GoToJail()
        {
            //Get both current player and current property
            PlayerViewModel currentPlayer = PlayerViewModel.CurrentPlayer;

            //If the player is behind position 10 (prison)
            if(currentPlayer.Position < 10)
            {
                int move = 10 - currentPlayer.Position;
                currentPlayer.MovePlayer(move);
            }
            else //If the player is ahead of position 10
            {
                int move = 10 - currentPlayer.Position + 40;
                currentPlayer.MovePlayer(move);
            }
        }

        private void AddLodgingToBoard(PropertyModel property)
        {
            // Check if the property already has a hotel, if so, do nothing
            if (property.HousesBuilt >= 5)
            {
                return;
            }

            // Create a new control:
            Image lodgingImage = new Image();


            // Set the data source: (offSet -> first house / offSetSecondary -> second house)
            int offSetCol = 0; 
            int offSetRow = 0;
            int offSetColSecondary = 0;
            int offSetRowSecondary = 0;

            // Set the Grid row and column offset
            SetGridOffsets(property, out offSetCol, out offSetRow, out offSetColSecondary, out offSetRowSecondary);

            LodgingModel houseModel = new LodgingModel("house", property.Row + offSetRow + offSetRowSecondary, property.Column + offSetCol + offSetColSecondary, 1, 1);
            LodgingModel doubleHouseModel = new LodgingModel("house_double", property.Row + offSetRow + offSetRowSecondary, property.Column + offSetCol + offSetColSecondary, 1, 1);
            LodgingModel hotelModel = new LodgingModel("hotel", property.Row + offSetRow + offSetRowSecondary, property.Column + offSetCol + offSetColSecondary, 1, 2);

            // Set the data model:
            LodgingModel lodgingModel = null;

            // Check if the property can have a house/hotel 
            if (property.HousesBuilt == 0)
            {
                lodgingModel = houseModel;
                CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);
            }
            else if (property.HousesBuilt == 1)
            {
                lodgingModel = houseModel;
                CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);
            }
            else if (property.HousesBuilt == 2)
            {
                lodgingModel = houseModel;
                //RemoveExistingImages(property);
                CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);
            }
            else if (property.HousesBuilt == 3)
            {
                lodgingModel = doubleHouseModel;
                //RemoveExistingImages(property);
                CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);
            }
            else if (property.HousesBuilt == 4)
            {
                lodgingModel = hotelModel;
                //RemoveExistingImages(property);
                CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);
            }

            // Create the Binding objects to set the path to the properties:
            BoardGrid.Children.Add(lodgingImage);
        }

        private void CreateAndBindLodgingImage(PropertyModel property, Image lodgingImage, LodgingModel lodgingModel, int offSetRow, int offSetCol)
        {
            if (lodgingModel == null)
            {
                return;
            }

            // Update the property's house count
            property.HousesBuilt++;

            // Create the Binding objects to set the path to the properties:
            BindLodgingImage(lodgingImage, lodgingModel);

            // Apply the binding to the controls' properties:
            lodgingImage.SetBinding(Grid.RowProperty, CreateBinding("Row", lodgingModel, offSetRow));
            lodgingImage.SetBinding(Grid.ColumnProperty, CreateBinding("Column", lodgingModel, offSetCol));
            lodgingImage.SetBinding(Grid.RowSpanProperty, CreateBinding("RowSpan", lodgingModel));
            lodgingImage.SetBinding(Grid.ColumnSpanProperty, CreateBinding("ColumnSpan", lodgingModel));
            lodgingImage.SetBinding(Image.SourceProperty, CreateBinding("ImgSrc", lodgingModel));
        }

        //private void RemoveExistingImages(PropertyModel property)
        //{
        //    // Remove existing images
        //    var imagesToRemove = BoardGrid.Children.OfType<Image>().Where(child => GetPropertyFromImage(child)?.Equals(property) == true).ToList();

        //    foreach (var imageToRemove in imagesToRemove)
        //    {
        //        BoardGrid.Children.Remove(imageToRemove);
        //    }
        //}

        private void SetGridOffsets(PropertyModel property, out int offSetCol, out int offSetRow, out int offSetColSecondary, out int offSetRowSecondary)
        {
            offSetCol = 0;
            offSetRow = 0;
            offSetColSecondary = 0;
            offSetRowSecondary = 0;

            //Top
            if (property.Row >= 0 && property.Row <= 3 && property.Column >= 0 && property.Column <= 21)
            {
                offSetCol = 0;
                offSetRow = 2;
                if (property.HousesBuilt == 1 || property.HousesBuilt == 3)
                {
                    offSetColSecondary = 1;
                    offSetRowSecondary = 0;
                }
            }
            //Right
            else if (property.Row >= 4 && property.Row <= 21 && property.Column >= 22 && property.Column <= 24)
            {
                offSetCol = 0;
                offSetRow = 0;
                if (property.HousesBuilt == 1 || property.HousesBuilt == 3)
                {
                    offSetColSecondary = 0;
                    offSetRowSecondary = 1;
                }

            }
            //Bottom
            else if (property.Row >= 22 && property.Row <= 24 && property.Column >= 0 && property.Column <= 21)
            {
                offSetCol = 1;
                offSetRow = 0;
                if (property.HousesBuilt == 1 || property.HousesBuilt == 3)
                {
                    offSetColSecondary = 0;
                    offSetRowSecondary = 0;
                }
            }
            //Left
            else if (property.Row >= 0 && property.Row <= 21 && property.Column >= 0 && property.Column <= 3)
            {
                offSetCol = 2;
                offSetRow = 1;
                if (property.HousesBuilt == 1 || property.HousesBuilt == 3)
                {
                    offSetColSecondary = 0;
                    offSetRowSecondary = 0;
                }
            }
        }

        private void BindLodgingImage(Image lodgingImage, LodgingModel lodgingModel)
        {
            // Create the Binding objects to set the path to the properties:
            lodgingImage.SetBinding(Grid.RowProperty, CreateBinding("Row", lodgingModel));
            lodgingImage.SetBinding(Grid.ColumnProperty, CreateBinding("Column", lodgingModel));
            lodgingImage.SetBinding(Grid.RowSpanProperty, CreateBinding("RowSpan", lodgingModel));
            lodgingImage.SetBinding(Grid.ColumnSpanProperty, CreateBinding("ColumnSpan", lodgingModel));
            lodgingImage.SetBinding(Image.SourceProperty, CreateBinding("ImgSrc", lodgingModel));
        }

        private Binding CreateBinding(string propertyPath, object source, int offset = 0)
        {
            Binding binding = new Binding(propertyPath);
            binding.Source = source;
            binding.Path = new PropertyPath(propertyPath);
            if (offset != 0)
            {
                //binding.Converter = new OffsetConverter(offset);
            }
            return binding;
        }

    }
}

