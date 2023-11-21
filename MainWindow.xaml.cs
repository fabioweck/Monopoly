using Monopoly.Model;
using Monopoly.View;
using Monopoly.ViewModel;
using System;
using System.Collections.Generic;
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
            LodgingViewModel.PopulateBoard();

            LblPlayersBalance = new List<Label>() { P1, P2, P3, P4 };

            //CardView = new CardViewModel();
            //Card1.DataContext = CardView;
        }

        private void RollDice_Click(object sender, RoutedEventArgs e)
        {
            View.DieView dieView = new View.DieView();
            dieView.ShowDialog();
            PlayerViewModel.CurrentPlayer.MovePlayer(dieView.Roll);
            ResolveLogic();
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

                myLabel.Background = new SolidColorBrush(Color.FromRgb(200, 200, 250));
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

            foreach (var lodging in LodgingViewModel.LodgingModel)
            {
                // Create a new control:
                Image lodgingImage = new Image();

                // Create the data source:
                LodgingModel lm = lodging.Value;

                // Create the Binding objects to set the path to the properties:
                Binding bindRow = new Binding("Row");
                Binding bindCol = new Binding("Column");
                Binding bindRowSpan = new Binding("RowSpan");
                Binding bindColSpan = new Binding("ColumnSpan");
                Binding bindImg = new Binding("ImgSrc");

                // Set the source to the data source previously defined:
                bindRow.Source = lm;
                bindCol.Source = lm;
                bindRowSpan.Source = lm;
                bindColSpan.Source = lm;
                bindImg.Source = lm;

                // Apply the binding to the controls' properties:
                lodgingImage.SetBinding(Grid.RowProperty, bindRow);
                lodgingImage.SetBinding(Grid.ColumnProperty, bindCol);
                lodgingImage.SetBinding(Grid.RowSpanProperty, bindRowSpan);
                lodgingImage.SetBinding(Grid.ColumnSpanProperty, bindColSpan);
                lodgingImage.SetBinding(Image.SourceProperty, bindImg);
                BoardGrid.Children.Add(lodgingImage);

            }
        }

        // After rolling the dice, resolve game's logic:
        public void ResolveLogic()
        {
            CheckPlayerOverProperty();

            ChangePlayer();

            CheckBankruptcy();
        }

        public void CheckPlayerOverProperty()
        {
            PlayerViewModel currentPlayer = PlayerViewModel.CurrentPlayer;
            var currentSpace = SpaceViewModel.spaceModels[PlayerViewModel.CurrentPlayer.Position];

            if (currentSpace.GetType() == typeof(PropertyModel))
            {
                PropertyModel property = (PropertyModel)currentSpace;

                if (property.Owner == null)
                {
                    MessageBoxResult result = MessageBox.Show($"Would you like to buy this property for ${property.Price}?", "Landed on a private property.", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        currentPlayer.ChangeBalance(value => currentPlayer.Balance -= value, property.Price);
                        property.Owner = PlayerViewModel.CurrentPlayer;

                        foreach (Label lbl in LblPlayersBalance)
                        {
                            if (lbl.Name == currentPlayer.Name)
                            {
                                lbl.Content = $"{currentPlayer.Name} balance: " + currentPlayer.Balance;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    currentPlayer.ChangeBalance(value => currentPlayer.Balance -= value, property.Rent[0]);
                    property.Owner.ChangeBalance(value => property.Owner.Balance += value, property.Rent[0]);
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
                }
            }
        }

        private static void ChangePlayer()
        {
            if (PlayerViewModel.Players.IndexOf(PlayerViewModel.CurrentPlayer) >= PlayerViewModel.Players.Count - 1)
                PlayerViewModel.CurrentPlayer = PlayerViewModel.Players[0];
            else
            {
                int _ind = PlayerViewModel.Players.IndexOf(PlayerViewModel.CurrentPlayer) + 1;
                PlayerViewModel.CurrentPlayer = PlayerViewModel.Players[_ind];
            }
        }

        public void CheckBankruptcy()
        {
            foreach(PlayerViewModel player in PlayerViewModel.Players)
            {
                if(player.Balance <= 0)
                {
                    MessageBox.Show($"{player.Name} has gone bankrupt...");
                }
            }
        }
    }
}

