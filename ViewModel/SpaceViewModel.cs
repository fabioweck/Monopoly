using Monopoly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Monopoly.ViewModel
{
    public class SpaceViewModel
    {
        public static Dictionary<int, SpaceModel> spaceModels = new Dictionary<int, SpaceModel>();
        public static Dictionary<int, PlayerViewModel> spaceOwners = new Dictionary<int, PlayerViewModel>();


        public SpaceViewModel()
        {

        }

        // Assign ownership of a property.
        public void AddOwner(PropertyModel property, PlayerViewModel pvm)
        {
            property.Owner = pvm;
            spaceOwners[property.SpaceNumber] = pvm;
        }

        public void BuyProperty(PropertyModel property, PlayerViewModel pvm)
        {
            AddOwner(property, pvm);
            property.OwnerName = pvm.Name;
            pvm.Player.Balance -= property.Price;
        }

        public void RemoveOwner(PropertyModel property, PlayerViewModel pvm)
        {
            property.Owner = null;
            spaceOwners.Remove(property.SpaceNumber);
        }


        public PlayerViewModel GetOwner(int spaceNumber)
        {
            if (spaceOwners.ContainsKey(spaceNumber))
            {
                return spaceOwners[spaceNumber];
            }
            return null;
        }

        public delegate void AddLodgingToBoard(PropertyModel property);


        // Resolves what happens when the player lands on a space.
        //public static void Resolve(Grid boardGrid, List<Label> lblPlayersBalance, AddLodgingToBoard addLodging)
        public static void Resolve(Grid boardGrid, List<Label> lblPlayersBalance, Action<Grid, PropertyModel> addLodgingToBoard)
        {

            //Get both current player and current property
            PlayerViewModel currentPlayer = PlayerViewModel.CurrentPlayer;
            var currentSpace = SpaceViewModel.spaceModels[currentPlayer.Position];
            SolidColorBrush playerColorBrush = new SolidColorBrush();

            if (currentPlayer.Position == 30)
            {
                MessageBox.Show("Go to the jail...", ":(", MessageBoxButton.OK);
                PlayerViewModel.GoToJail();
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
                        if (property.Row >= 0 && property.Row <= 3 && property.Column >= 0 && property.Column <= 21)
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
                        propertyLabel.Foreground = MainWindow.GetPlayerColor(PlayerViewModel.CurrentPlayer.instanceNumber);

                        // Add the label to the Grid
                        boardGrid.Children.Add(propertyLabel);

                        // Add a house to the board (Check if the property is a Railroad or Utility, and if so, do not add a house)
                        if (property.Group != "Railroad" && property.Group != "Utility")
                        {
                            addLodgingToBoard(boardGrid, property);
                        }

                        //Update balance on panel
                        foreach (Label lbl in lblPlayersBalance)
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
                    foreach (Label lbl in lblPlayersBalance)
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

        public void PositionSpaces()
        {

        }

        delegate void ResolveSpaceAction();



        public static void PopulateBoard()
        {
            spaceModels.Add(0,
                new SpaceModel(
                    "Start",
                    "Starting point. Pass here to receive $200.",
                    "Neutral", // Neutral because the money is earned when we pass, not when we land.
                    "Tile0",                          
                   1, 1, 3, 3, 0));

            SpaceModel sm1 = new PropertyModel(
                    "Meditarranean Avenue",                             // Name
                    "A property belonging to group 'Brown'. Space #1",  // Description
                    "Tile01_Brown",                  // Image
                    "Brown",                                            // Group
                    60,                                                 // Price to buy
                    50,                                                 // Cost per house
                    2, 10, 30, 90, 160, 250,                            // Rent[]
                    1, 4, 3, 2);
            spaceModels.Add(1, sm1);

            spaceModels.Add(2,
                new SpaceModel(
                    "Community Chest",
                    "Community Chest Cards. Space #2",
                    "Card",
                    "Tile02_Chest",
                    1, 6, 3, 2, 1));

            SpaceModel sm3 = new PropertyModel(
                   "Baltic Avenue",                                     // Name
                   "A property belonging to group 'Brown'. Space #3",   // Description
                   "Tile03_Brown",                   // Image
                   "Brown",                                             // Group
                   60,                                                  // Price to buy
                   50,                                                  // Cost per house
                   4, 20, 60, 180, 320, 450,                            // Rent[]
                   1, 8, 3, 2);
            spaceModels.Add(3, sm3);

            spaceModels.Add(4,
                new SpaceModel(
                    "Tax Income",
                    "Pay Taxes! Space#4",
                    "Bank",
                    "Tile04_Tax",                         
                    1, 10, 3, 2, 2));

            SpaceModel sm5 = new PropertyModel(
                    "Reading Railroad",                                 // Name
                    "Railroad Company. Space#5",                        // Description
                    "Tile05_Rail",                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                   1, 12, 3, 2);
            spaceModels.Add(5, sm5);

            SpaceModel sm6 = new PropertyModel(
                    "Oriental Avenue",                                  // Name
                    "A property belonging to group 'Teal'. Space#6",    // Description
                    "Tile06_Teal",                   // Image
                    "Teal",                                             // Group
                    100,                                                // Price to buy
                    50,                                                 // Cost per house
                    6, 30, 90, 270, 400, 550,                           // Rent[]
                    1, 14, 3, 2);
            spaceModels.Add(6, sm6);

            spaceModels.Add(7,
                new SpaceModel(
                    "Chance",
                    "Chance Cards. Space#7",
                    "Card",
                    "Tile07_Chance",
                   1, 16, 3, 2, 3));

            SpaceModel sm8 = new PropertyModel(
                    "Vermont Avenue",                                   // Name
                    "A property belonging to group 'Teal'. Space#8",    // Description
                    "Tile08_Teal",                   // Image
                    "Teal",                                             // Group
                    100,                                                // Price to buy
                    50,                                                 // Cost per house
                    6, 30, 90, 270, 400, 550,                           // Rent[]
                    1, 18, 3, 2);
            spaceModels.Add(8, sm8);

            SpaceModel sm9 = new PropertyModel(
                    "Connecticut Avenue",                               // Name
                    "A property belonging to group 'Teal'. Space#9",    // Description
                    "Tile09_Teal",                   // Image
                    "Teal",                                             // Group
                    120,                                                // Price to buy
                    50,                                                 // Cost per House
                    8, 40, 100, 300, 450, 600,                          // Rent[]
                    1, 20, 3, 2);
            spaceModels.Add(9, sm9);                      

            spaceModels.Add(10,
                new SpaceModel(
                    "Jail Visitor",
                    "You are just visiting. Space#10",
                    "Neutral",
                    "Tile10_Visitor",
                   1, 22, 3, 3, 4));

            SpaceModel sm11 = new PropertyModel(
                    "St. Charles Place",                                // Name
                    "A property belonging to group 'Pink'. Space#11",   // Description
                    "Tile11_Pink",                   // Image
                    "Pink",                                             // Group
                    140,                                                // Price to buy
                    100,                                                // Cost per House
                    10, 50, 150, 450, 625, 750,                         // Rent[]
                    4, 22, 2, 3);
            spaceModels.Add(11,sm11);

            SpaceModel sm12 = new PropertyModel(
                    "Electric Company",
                    "Utility Company. Space#12",
                    "Tile12_Light",
                    150,
                    4, 10,      // Multiply this by the dice roll
                    6, 22, 2, 3);
            spaceModels.Add(12, sm12);


            SpaceModel sm13 = new PropertyModel(
                    "States Avenue",                                    // Name
                    "A property belonging to group 'Pink'. Space#13",   // Description
                    "Tile13_Pink",                   // Image
                    "Pink",                                             // Group
                    140,                                                // Price to buy
                    100,                                                // Cost per House
                    10, 50, 150, 450, 625, 750,                         // Rent[]
                    8, 22, 2, 3);
            spaceModels.Add(13, sm13);

            SpaceModel sm14 = new PropertyModel(
                    "Virginia Avenue",                                  // Name
                    "A property belonging to group 'Pink'. Space#14",   // Description
                    "Tile14_Pink",                   // Image
                    "Pink",                                             // Group
                    160,                                                // Price to buy
                    100,                                                // Cost per House
                    12, 60, 180, 500, 700, 900,                         // Rent[]
                    10, 22, 2, 3);
            spaceModels.Add(14, sm14);

            SpaceModel sm15 = new PropertyModel(
                    "Pennsylvania Railroad",                            // Name
                    "Railroad Company. Space#15",                       // Description
                    "Tile15_Rail",                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                    12, 22, 2, 3);
            spaceModels.Add(15, sm15);
            
            SpaceModel sm16 = new PropertyModel(
                    "St. James Place",                                  // Name
                    "A property belonging to group 'Orange'. Space#16", // Description
                    "Tile16_Orange",                 // Image
                    "Orange",                                           // Group
                    180,                                                // Price to buy
                    100,                                                // Cost per House
                    14, 70, 200, 550, 750, 950,                         // Rent[]
                    14, 22, 2, 3);
            spaceModels.Add(16, sm16);

            spaceModels.Add(17,
                new SpaceModel(
                    "Community Chest",
                    "Community Chest Cards. Space #17",
                    "Card",
                    "Tile17_Chest",                        
                    16, 22, 2, 3, 5));

            SpaceModel sm18 = new PropertyModel(
                    "Tennessee Avenue",                                 // Name
                    "A property belonging to group 'Orange'. Space#18", // Description
                    "Tile18_Orange",                 // Image
                    "Orange",                                           // Group
                    180,                                                // Price to buy
                    100,                                                // Cost per House
                    14, 70, 200, 550, 750, 950,                         // Rent[]
                    18, 22, 2, 3);
            spaceModels.Add(18, sm18);

            SpaceModel sm19 = new PropertyModel(
                    "New York Avenue",                                  // Name
                    "A property belonging to group 'Orange'. Space#19", // Description
                    "Tile19_Orange",                 // Image
                    "Orange",                                           // Group
                    200,                                                // Price to buy
                    100,                                                // Cost per House
                    16, 80, 220, 600, 800, 1000,                        // Rent[]
                    20, 22, 2, 3);
            spaceModels.Add(19, sm19);
            
            spaceModels.Add(20,
                new SpaceModel(
                    "Free Parking",
                    "Free Parking. Space #20",
                    "Neutral",
                    "Tile20_Parking",
                   22, 22, 3, 3, 6));

            SpaceModel sm21 = new PropertyModel(
                    "Kentucky Avenue",                                  // Name
                    "A property belonging to group 'Red'. Space#21",    // Description
                    "Tile21_Red",                    // Image
                    "Red",                                              // Group
                    220,                                                // Price to buy
                    150,                                                // Cost per House
                    18, 90, 250, 700, 875, 1050,                        // Rent[]
                    22, 20, 3, 2);
            spaceModels.Add(21, sm21);

            spaceModels.Add(22,
                new SpaceModel(
                    "Chance",
                    "Chance Cards. Space#22",
                    "Card",
                    "Tile22_Chance",
                   22, 18, 3, 2, 7));

            SpaceModel sm23 = new PropertyModel(
                    "Indiana Avenue",                                   // Name
                    "A property belonging to group 'Red'. Space#23",    // Description
                    "Tile23_Red",                    // Image
                    "Red",                                              // Group
                    220,                                                // Price to buy
                    150,                                                // Cost per House
                    18, 90, 250, 700, 875, 1050,                        // Rent[]
                    22, 16, 3, 2);
            spaceModels.Add(23, sm23);

            SpaceModel sm24 = new PropertyModel(
                    "Illinois Avenue",                                  // Name
                    "A property belonging to group 'Red'. Space#24",    // Description
                    "Tile24_Red",                    // Image
                    "Red",                                              // Group
                    240,                                                // Price to buy
                    150,                                                // Cost per House
                    20, 100, 300, 750, 925, 1100,                       // Rent[]
                    22, 14, 3, 2);
            spaceModels.Add(24, sm24);

            SpaceModel sm25 = new PropertyModel(
                    "B.O. Railroad",                                    // Name
                    "Railroad Company. Space#25",                       // Description
                    "Tile25_Rail",                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                    22, 12, 3, 2);
            spaceModels.Add(25, sm25);

            SpaceModel sm26 = new PropertyModel(
                    "Atlantic Avenue",                                  // Name
                    "A property belonging to group 'Yellow'. Space#26", // Description
                    "Tile26_Yellow",                 // Image
                    "Yellow",                                           // Group
                    260,                                                // Price to buy
                    150,                                                // Cost per House
                    22, 110, 360, 800, 975, 1150,                       // Rent[]
                    22, 10, 3, 2);
            spaceModels.Add(26, sm26);

            SpaceModel sm27 = new PropertyModel(
                    "Ventnor Avenue",                                   // Name
                    "A property belonging to group 'Yellow'. Space#27", // Description
                    "Tile27_Yellow",                 // Image
                    "Yellow",                                           // Group
                    260,                                                // Price to buy
                    150,                                                // Cost per House
                    22, 110, 360, 800, 975, 1150,                       // Rent[]
                    22, 8, 3, 2);
            spaceModels.Add(27, sm27);

            SpaceModel sm28 = new PropertyModel(
                    "Water Company",
                    "Utility Company. Space#28",
                    "Tile28_Water",
                    150,
                    4, 10, // Multiply this by the dice roll
                   22, 6, 3, 2);
            spaceModels.Add(28, sm28);

            SpaceModel sm29 = new PropertyModel(
                    "Marvin Gardens",                                   // Name
                    "A property belonging to group 'Yellow'. Space#29", // Description
                    "Tile29_Yellow",                 // Image
                    "Yellow",                                           // Group
                    280,                                                // Price to buy
                    150,                                                // Cost per House
                    24, 120, 360, 850, 1025, 1200,                      // Rent[]
                    22, 4, 3, 2);
            spaceModels.Add(29, sm29);

            spaceModels.Add(30,
                new SpaceModel(
                    "Go To Jail",
                    "Go to jail. Space#30",
                    "Jail",
                    "Tile30_GoToJail",
                   22, 1, 3, 3, 8));

            SpaceModel sm31 = new PropertyModel(
                    "Pacific Avenue",                                   // Name
                    "A property belonging to group 'Green'. Space#31",  // Description
                    "Tile31_Green",                  // Image
                    "Green",                                            // Group
                    300,                                                // Price to buy
                    200,                                                // Cost per House
                    26, 130, 390, 900, 1100, 1275,                      // Rent[]
                    20, 1, 2, 3);
            spaceModels.Add(31, sm31);

            SpaceModel sm32 = new PropertyModel(
                    "North Carolina Avenue",                            // Name
                    "A property belonging to group 'Green'. Space#32",  // Description
                    "Tile32_Green",                  // Image
                    "Green",                                            // Group
                    300,                                                // Price to buy
                    200,                                                // Cost per House
                    26, 130, 390, 900, 1100, 1275,                      // Rent[]
                    18, 1, 2, 3);
            spaceModels.Add(32, sm32);

            spaceModels.Add(33,
                new SpaceModel(
                    "Community Chest",
                    "Community Chest Cards. Space #33",
                    "Card",
                    "Tile33_Chest",
                   16, 1, 2, 3, 9));

            SpaceModel sm34 = new PropertyModel(
                    "Pennsylvania Avenue",                              // Name
                    "A property belonging to group 'Green'. Space#34",  // Description
                    "Tile34_Green",                  // Image
                    "Green",                                            // Group
                    320,                                                // Price to buy
                    200,                                                // Cost per House
                    28, 150, 45, 1000, 1200, 1400,                      // Rent[]
                    14, 1, 2, 3);
            spaceModels.Add(34, sm34);

            SpaceModel sm35 = new PropertyModel(
                    "Short Line",                                       // Name
                    "Railroad Company. Space#35",                       // Description
                    "Tile35_Rail",                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                    12, 1, 2, 3);
            spaceModels.Add(35, sm35);

            spaceModels.Add(36,
                new SpaceModel(
                    "Chance",
                    "Chance Cards. Space#36",
                    "Card",
                    "Tile36_Chance",
                   10, 1, 2, 3, 10));

            SpaceModel sm37 = new PropertyModel(
                    "Park Place",                                       // Name
                    "A property belonging to group 'Blue'. Space#37",   // Description
                    "Tile37_Blue",                   // Image
                    "Blue",                                             // Group
                    350,                                                // Price to buy
                    200,                                                // Cost per House
                    35, 175, 500, 1100, 1300, 1500,                     // Rent[]
                    8, 1, 2, 3);
            spaceModels.Add(37, sm37);

            spaceModels.Add(38,
                new SpaceModel(
                    "Luxury Tax",
                    "Pay Taxes! Space#38",
                    "Bank",
                    "Tile38_Tax",
                   6, 1, 2, 3, 11));

            SpaceModel sm39 = new PropertyModel(
                    "Boardwalk",                                        // Name
                    "A property belonging to group 'Blue'. Space#37",   // Description
                    "Tile39_Blue",                   // Image
                    "Blue",                                             // Group
                    400,                                                // Price to buy
                    200,                                                // Cost per House
                    50, 200, 600, 1400, 1700, 2000,                     // Rent[]
                    4, 1, 2, 3);                                                     
            spaceModels.Add(39, sm39);
        }
    }
}
