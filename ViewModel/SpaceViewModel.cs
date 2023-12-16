using Monopoly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Monopoly.View;
using System.Windows.Documents;

namespace Monopoly.ViewModel
{
    public class SpaceViewModel
    {
        delegate void AddLodgingToBoard(PropertyModel property);
        delegate void ResolveSpaceAction();

        public static Dictionary<int, SpaceModel> spaceModels = new Dictionary<int, SpaceModel>();
        // public static Dictionary<int, PlayerViewModel> propertyOwners = new Dictionary<int, PlayerViewModel>();

        //Suggestion for the dictionary
        public static Dictionary<PlayerViewModel, List<PropertyModel>> propertyOwners = new Dictionary<PlayerViewModel, List<PropertyModel>>();

        public SpaceViewModel()
        {

        }

        // Assign ownership of a property.
        public static void AddOwner(PropertyModel property, PlayerViewModel pvm)
        {
            property.Owner = pvm;
            propertyOwners[pvm].Add(property);
        }

        // Purchases the property, changing player balance and adding the ownership label
        public static void BuyProperty(PropertyModel property, PlayerViewModel pvm, Grid boardGrid)
        {
            AddOwner(property, pvm);
            property.OwnerName = pvm.Name;
            pvm.Player.Balance -= property.Price;

            // Create a label for the property
            Label propertyLabel = new Label();
            propertyLabel.Content = property.Owner.Name;
            string _n = property.Name.Replace(" ", "").Replace(".", "").Replace("&","");

            propertyLabel.Name = _n;
            propertyLabel.FontSize = 12;
            propertyLabel.FontWeight = FontWeights.Bold;
            propertyLabel.SetValue(Grid.ColumnSpanProperty, property.ColumnSpan);
            propertyLabel.SetValue(Grid.RowSpanProperty, property.RowSpan);

            // Set the Grid row and column
            //Top
            if (property.Row >= 0 && property.Row <= 3 && property.Column >= 0 && property.Column <= 21)
            {
                propertyLabel.SetValue(Grid.RowProperty, property.Row + 3);
                propertyLabel.SetValue(Grid.ColumnProperty, property.Column);
            }
            //Right
            if (property.Row >= 4 && property.Row <= 21 && property.Column >= 22 && property.Column <= 24)
            {
                propertyLabel.SetValue(Grid.RowProperty, property.Row);
                propertyLabel.SetValue(Grid.ColumnProperty, property.Column - 1);
            }
            //Bottom
            if (property.Row >= 22 && property.Row <= 24 && property.Column >= 0 && property.Column <= 21)
            {
                propertyLabel.SetValue(Grid.RowProperty, property.Row - 1);
                propertyLabel.SetValue(Grid.ColumnProperty, property.Column);
            }
            //Left
            if (property.Row >= 0 && property.Row <= 21 && property.Column >= 0 && property.Column <= 3)
            {
                propertyLabel.SetValue(Grid.RowProperty, property.Row);
                propertyLabel.SetValue(Grid.ColumnProperty, property.Column + 3);
            }
            //Left-top corner
            if (property.Row >= 4 && property.Row <= 5 && property.Column >= 0 && property.Column <= 3)
            {
                propertyLabel.SetValue(Grid.RowProperty, 5);
                propertyLabel.SetValue(Grid.ColumnProperty, property.Column + 3);
            }
            //Left-bottom corner
            if (property.Row >= 20 && property.Row <= 21 && property.Column >= 0 && property.Column <= 3)
            {
                propertyLabel.SetValue(Grid.RowProperty, 20);
                propertyLabel.SetValue(Grid.ColumnProperty, property.Column + 3);
            }
            //Right-top corner
            if (property.Row >= 4 && property.Row <= 5 && property.Column >= 22 && property.Column <= 24)
            {
                propertyLabel.SetValue(Grid.RowProperty, 5);
                propertyLabel.SetValue(Grid.ColumnProperty, property.Column - 1);
            }

            // Label with the same color of the player
            propertyLabel.Foreground = PlayerViewModel.CurrentPlayer.Color;

            // Add the label to the Grid
            boardGrid.Children.Add(propertyLabel);

            //Update Players Panel
            foreach (TextBox textBox in MainWindow.txtBoxPanelPlayers)
            {
                if (textBox.Name == pvm.Name)
                {
                    UpdatePlayerPanel(textBox, pvm);
                }
            }
        }

        public static void SellProperty(PropertyModel property, PlayerViewModel pvm, Grid boardGrid)
        {
            RemoveOwner(property, pvm);
            pvm.Player.Balance += (int)property.Price/ 2;
            var childToRemove = boardGrid.Children.OfType<Label>()
                        .FirstOrDefault(x => x.Name == property.Name.Replace(" ", "").Replace(".", "").Replace("&", ""));


            if (childToRemove != null)
            {
                boardGrid.Children.Remove(childToRemove);
            }
        }

        public static void RemoveOwner(PropertyModel property, PlayerViewModel pvm)
        {
            property.Owner = null;
            propertyOwners[pvm].Remove(property);
        }

        public static void SellHouse(PropertyModel property, PlayerViewModel pvm)
        {
            if (property.HousesBuilt > 0)
            {
                pvm.Player.Balance += (int)property.HousePrice / 2;
                property.HousesBuilt--;
            }
            else MessageBox.Show("No more houses!");

        }


        // Resolves what happens when the player lands on a space.
        public static void Resolve(Grid boardGrid, List<TextBox> txtBoxPanelPlayers, Action<Grid, PropertyModel> addLodgingToBoard, MainWindow board)
        {

            //Get both current player and current property
            PlayerViewModel currentPlayer = PlayerViewModel.CurrentPlayer;
            var currentSpace = spaceModels[currentPlayer.Position];
            SolidColorBrush playerColorBrush = new SolidColorBrush();

            //If player position is over place 30, it means go to prison
            if (currentPlayer.Position == 30)
            {
                MessageBox.Show($"{PlayerViewModel.CurrentPlayer.Name}, go to jail...", ":(", MessageBoxButton.OK);
                PlayerViewModel.GoToJail();
                return;
            }

            //If player position is over place 2, 17 or 33, it means Community card
            if(new int[] { 2, 17, 33 }.Contains(currentPlayer.Position))
            {
                CardViewModel.FlipACard("Community");

                string description = CardViewModel.CurrentCard.Description;
                CardView card = new CardView("Community Chest Card", description);
                card.ShowDialog();

                string effect = CardViewModel.CurrentCard.Effect;

                switch(effect)
                {
                    case "Move":
                        int move = CardViewModel.CurrentCard.Move;
                        currentPlayer.MovePlayer(move);
                        break;
                    case "Value":
                        int amount = CardViewModel.CurrentCard.Value;
                        if (amount < 0) TryToSellAssets(-amount, currentPlayer, boardGrid);
                        PayTaxesOrFees(-amount, currentPlayer, boardGrid, board);
                        break;
                    case "JailFree":
                        currentPlayer.CommunityCard = "Jail Free Card";
                        CardViewModel.RemoveCommunityCard(CardViewModel.CurrentCard);
                        break;
                    case "Go":
                        PlayerViewModel.GotToFirstPlace();
                        break;
                    case "NextRail":
                        PlayerViewModel.GoToNextRailroad();
                        break;
                    case "Jail":
                        PlayerViewModel.GoToJail();
                        break;
                }
            }

            //If player position is over place 7, 17 or 33, it means Chance card
            if (new int[] { 7, 22, 36 }.Contains(currentPlayer.Position))
            {
                CardViewModel.FlipACard("Chance");

                string description = CardViewModel.CurrentCard.Description;
                CardView card = new CardView("Chance Card", description);
                card.ShowDialog();

                string effect = CardViewModel.CurrentCard.Effect;

                switch (effect)
                {
                    case "Move":
                        int move = CardViewModel.CurrentCard.Move;
                        currentPlayer.MovePlayer(move);
                        break;
                    case "Value":
                        int amount = CardViewModel.CurrentCard.Value;
                        if (amount < 0) TryToSellAssets(-amount, currentPlayer, boardGrid);
                        PayTaxesOrFees(-amount, currentPlayer, boardGrid, board);
                        break;
                    case "JailFree":
                        currentPlayer.ChanceCard = "Jail Free Card";
                        CardViewModel.RemoveCommunityCard(CardViewModel.CurrentCard);
                        break;
                    case "Go":
                        PlayerViewModel.GotToFirstPlace();
                        break;
                    case "NextRail":
                        PlayerViewModel.GoToNextRailroad();
                        break;
                    case "Jail":
                        PlayerViewModel.GoToJail();
                        break;
                }
            }

            //Check if the space is a property
            if (currentSpace.GetType() == typeof(PropertyModel))
            {
                //if yes, cast to Property type
                PropertyModel property = (PropertyModel)currentSpace;

                if (property.Group != "Railroad" && property.Group != "Utility")
                {
                    //Once the property has no owner, offer it
                    if (property.Owner == null)
                    {
                       HandlePropertyPurchase(currentPlayer, property, boardGrid, txtBoxPanelPlayers);
                    }
                    else
                    {
                        // Check if is possible upgrade and ask if the owner want to upgrade lodging
                        bool canUpgradeProperty = CanUpgradeProperty(property, PlayerViewModel.CurrentPlayer);

                        if (canUpgradeProperty)
                        {
                            MessageBoxResult upgradeResult = MessageBox.Show($"{PlayerViewModel.CurrentPlayer.Name}, do you want to upgrade lodging on this property?", "Upgrade Lodging", MessageBoxButton.YesNo);

                            if (upgradeResult == MessageBoxResult.Yes)
                            {
                                addLodgingToBoard?.Invoke(boardGrid, property);
                            }
                            return;
                        }

                        //If the player is the owner, do nothing
                        if (property.Owner == PlayerViewModel.CurrentPlayer) return;

                        PayPropertyRent(currentPlayer, property, boardGrid, board);

                        //Update their balance on the screen
                        //Update Players Panel
                        foreach (TextBox textBox in txtBoxPanelPlayers)
                        {
                            if (textBox.Name == currentPlayer.Name)
                            {
                                UpdatePlayerPanel(textBox, currentPlayer);
                            }
                        }
                    }
                }

                else if (property.Group == "Railroad")
                {
                    //If the player is the owner, do nothing
                    if (property.Owner == PlayerViewModel.CurrentPlayer) return;

                    //Once the property has no owner, offer to buy it to the current player
                    if (property.Owner == null)
                    {
                        HandlePropertyPurchase(currentPlayer, property, boardGrid, txtBoxPanelPlayers);
                    }
                    else
                    {
                        //If the player is not the owner, pass the functions to debit from current player and pay rent to the owner
                        PayRailroadRent(currentPlayer, property, boardGrid, board);
                    }   
                }

                else if (property.Group == "Utility")
                {
                    //If the player is the owner, do nothing
                    if (property.Owner == PlayerViewModel.CurrentPlayer) return;

                    //Once the property has no owner, offer to buy it to the current player
                    if (property.Owner == null)
                    {
                        HandlePropertyPurchase(currentPlayer, property, boardGrid, txtBoxPanelPlayers);
                    }
                    else
                    {
                        PayUtilitiesRent(currentPlayer, property, boardGrid, board);
                    }
                }
            }
            //If it is a space model, it only can be taxes
            else
            {
                //Define the variable rent
                int rent = 0;
                switch(currentSpace.Name)
                {
                    //In case of luxury tax, the player pays 100 dollars
                    case "Luxury Tax":
                        rent = 100;
                        break;
                    //In case of tax income, the player pays 200 dollars
                    case "Tax Income":
                        rent = 200;
                        break;
                }
                if (rent > 0)
                {
                    MessageBox.Show($"{PlayerViewModel.CurrentPlayer.Name}, pay {rent}.");

                    // If cannot pay rent, and has properties to sell:
                    TryToSellAssets(rent, currentPlayer, boardGrid);

                    // Handle tax payment
                    PayTaxesOrFees(rent, currentPlayer, boardGrid, board);
                }
            }
        }

        public static bool CanUpgradeProperty(PropertyModel property, PlayerViewModel player)
        {
            // Check if the player is the owner
            if (property.Owner != PlayerViewModel.CurrentPlayer)
            {
                return false;
            }

            // Check if the player owns all properties of the group
            if (!PlayerOwnsAllPropertiesOfGroup(player, property.Group))
            {
                ShowMessageBox($"{player.Name}, you must own all properties of the group to upgrade this property.", "Cannot Upgrade Property");
                return false;
            }

            // Check if the player has enough balance to upgrade the property
            if (player.Balance < property.HousePrice)
            {
                ShowMessageBox($"{player.Name}, you don't have enough balance to upgrade this property.", "Cannot Upgrade Property");
                return false;
            }

            // Check if the property can have more upgrades (max 5 upgrades)
            if (property.HousesBuilt >= 5)
            {
                ShowMessageBox($"{player.Name}, the maximum number of upgrades has already been reached on this property.", "Cannot Upgrade Property");
                return false;
            }

            // Check for homogeneous house building
            foreach (var space in spaceModels.Values.Where(s => s is PropertyModel propertyInGroup && propertyInGroup.Group == property.Group))
            {
                if (property.HousesBuilt > 0)
                {
                    // Check that if the current property has houses, all others in the group also have at least the same number
                    int minHousesInGroup = ((PropertyModel)space).HousesBuilt;
                    if (minHousesInGroup < property.HousesBuilt)
                    {
                        ShowMessageBox($"{player.Name}, you must upgrade properties homogeneously within the group with a maximum difference of 1 house.", "Cannot Upgrade Property");
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Overload for properties
        /// </summary>
        /// <param name="property"></param>
        /// <param name="rent"></param>
        /// <param name="currentPlayer"></param>
        public static void TryToSellAssets(PropertyModel property, int rent, PlayerViewModel currentPlayer, Grid boardGrid)
        {
            // If cannot pay rent, and has properties to sell:
            if (currentPlayer.Balance < rent && propertyOwners[currentPlayer].Count > 0)
            {
                int _control = 0;
                while (_control == 0)
                {
                    SellAssets sell = new SellAssets(currentPlayer, propertyOwners, boardGrid);
                    sell.ShowDialog();

                    // If player has enough balance or player no longer has any properties to sell, move on
                    if (currentPlayer.Balance >= rent || propertyOwners[currentPlayer].Count == 0)
                        _control = 1;
                }
            }
        }

        /// <summary>
        /// Overload for Taxes and Cards
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currentPlayer"></param>
        public static void TryToSellAssets(int value, PlayerViewModel currentPlayer, Grid boardGrid)
        {
            // If cannot pay rent, and has properties to sell:
            if (currentPlayer.Balance < value && propertyOwners[currentPlayer].Count > 0)
            {
                int _control = 0;
                while (_control == 0)
                {
                    SellAssets sell = new SellAssets(currentPlayer, propertyOwners, boardGrid);
                    sell.ShowDialog();

                    // If player has enough balance or player no longer has any properties to sell, move on
                    if (currentPlayer.Balance >= value || propertyOwners[currentPlayer].Count == 0)
                        _control = 1;
                }
            }
        }

        public static void PayRent(PropertyModel property, int rent, PlayerViewModel currentPlayer, Grid boardGrid, MainWindow board)
        { 
            // If still doesn't have enough money, pay what they have left and go bankrupt:
            if (currentPlayer.Balance < rent)
            {
                currentPlayer.ChangeBalance(value => currentPlayer.Balance -= value, currentPlayer.Balance);
                property.Owner.ChangeBalance(value => property.Owner.Balance += value, currentPlayer.Balance);

                currentPlayer.FileBankruptcy(boardGrid, board);
            }

            // Execute full payment transaction (one debit and one payment)
            else
            {
                currentPlayer.ChangeBalance(value => currentPlayer.Balance -= value, rent);
                property.Owner.ChangeBalance(value => property.Owner.Balance += value, rent);
            }
        }

        public static void PayPropertyRent(PlayerViewModel currentPlayer, PropertyModel property, Grid boardGrid, MainWindow board)
        {
            // property.Rent[property.HousesBuilt] is used to count the number of houses in a property
            int rent = property.Rent[property.HousesBuilt];

            // Check if the player owns all properties of the color/group
            bool ownsAllProperties = PlayerOwnsAllPropertiesOfGroup(PlayerViewModel.CurrentPlayer, property.Group);
            if (ownsAllProperties && property.HousesBuilt == 0)
            {
                rent = property.Rent[0] * 2;
            }
            //If the player is not the owner, pass the functions to debit from current player and pay rent to the owner
            MessageBox.Show($"{PlayerViewModel.CurrentPlayer.Name}, pay rent to {property.OwnerName}\n${rent}");

            // If cannot pay rent, and has properties to sell:
            TryToSellAssets(property, rent, currentPlayer, boardGrid);

            // Try to pay rent
            PayRent(property, rent, currentPlayer, boardGrid, board);
        }

        public static void PayRailroadRent(PlayerViewModel currentPlayer, PropertyModel property, Grid boardGrid, MainWindow board)
        {
            // Helper variable to count the number of "Railroad" properties an onwer has
            int numberOfProperties = 0;

            // Iterates over the spaceModels to find all "Railroad" belonging to owner
            foreach (var space in spaceModels)
            {
                if (space.Value.GetType() == typeof(PropertyModel))
                {
                    PropertyModel prop = (PropertyModel)space.Value;
                    if (prop.Group == "Railroad" && prop.OwnerName == property.OwnerName)
                    {
                        numberOfProperties++;
                    }
                }
            }

            int rent = property.Rent[numberOfProperties - 1];

            // If the player is not the owner, pass the functions to debit from current player and pay rent to the owner
            MessageBox.Show($"{PlayerViewModel.CurrentPlayer.Name}, pay rent to {property.OwnerName}\n${rent}");

            // If cannot pay rent, and has properties to sell:
            TryToSellAssets(property, rent, currentPlayer, boardGrid);

            // Try to pay rent
            PayRent(property, rent, currentPlayer, boardGrid, board);
        }

        public static void PayUtilitiesRent(PlayerViewModel currentPlayer, PropertyModel property, Grid boardGrid, MainWindow board)
        {
            //Helper variable to count the number of "Utilities" properties an onwer has
            int numberOfProperties = 0;

            //Iterates over the spaceModels to find all "Utilities" belonging to owner
            foreach (var space in spaceModels)
            {
                if (space.Value.GetType() == typeof(PropertyModel))
                {
                    PropertyModel prop = (PropertyModel)space.Value;
                    if (prop.Group == "Utility" && prop.OwnerName == property.OwnerName)
                    {
                        numberOfProperties++;
                    }
                }
            }

            int rent = property.Rent[numberOfProperties - 1] * DieView.Roll;

            MessageBox.Show($"{PlayerViewModel.CurrentPlayer.Name} landed on a utility.\nPay rent to {property.OwnerName}\n${rent}");

            // If cannot pay rent, and has properties to sell:
            TryToSellAssets(property, rent, currentPlayer, boardGrid);

            // Try to pay rent
            PayRent(property, rent, currentPlayer, boardGrid, board);
        }

        public static void PayTaxesOrFees(int rent, PlayerViewModel currentPlayer, Grid boardGrid, MainWindow board)
        {
            // If still doesn't have enough money, pay what they have left and go bankrupt:
            if (currentPlayer.Balance < rent)
            {
                currentPlayer.ChangeBalance(value => currentPlayer.Balance -= value, currentPlayer.Balance);
                currentPlayer.FileBankruptcy(boardGrid, board);
            }

            // Execute full payment transaction (one debit)
            else
            {
                currentPlayer.ChangeBalance(value => currentPlayer.Balance -= value, rent);
            }
        }

        public static void HandlePropertyPurchase(PlayerViewModel currentPlayer, PropertyModel property, Grid boardGrid, List<TextBox> txtBoxPanelPlayers)
        {
            string _t = $"{property.Name} ({property.Group})";

            // If the player has enough balance to purchase, handle the question and purchase
            if (currentPlayer.Balance >= property.Price)
            {
                MessageBoxResult result = MessageBox.Show($"{PlayerViewModel.CurrentPlayer.Name}, would you like to buy {_t} for ${property.Price}?", "Landed on a property.", MessageBoxButton.YesNo);

                //If the player wants to buy the property, pass the function to balance to perform the calculation
                if (result == MessageBoxResult.Yes)
                {
                    BuyProperty(property, currentPlayer, boardGrid);
                }
                else //Otherwise, do nothing
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show($"Not enough balance to purchase {_t} [{property.Price}].", "Insufficient Balance.", MessageBoxButton.OK);
            }
        }

        private static void ShowMessageBox(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }

        public static void UpdatePlayerPanel(TextBox textBox, PlayerViewModel player)
        {
            textBox.Text = $"Panel: {player.Name}" +
                           $"\nBalance: {player.Balance}" +
                           $"\nTotal Of Properties: {player.PlayerTotalOfProperties()}" +
                           $"\n{player.CommunityCard}" +
                           $"\n{player.ChanceCard}"; 
        }

        public static bool PlayerOwnsAllPropertiesOfGroup(PlayerViewModel player, string group)
        {
            return spaceModels.Values
                .Where(space => space is PropertyModel && ((PropertyModel)space).Group == group)
                .All(property => ((PropertyModel)property).Owner == player);
        }

        //public void PositionSpaces()
        //{

        //}     

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
                    "Tile01_Brown",                                     // Image
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
                   "Tile03_Brown",                                      // Image
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
                    "Tile05_Rail",                                      // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                   1, 12, 3, 2);
            spaceModels.Add(5, sm5);

            SpaceModel sm6 = new PropertyModel(
                    "Oriental Avenue",                                  // Name
                    "A property belonging to group 'Teal'. Space#6",    // Description
                    "Tile06_Teal",                                      // Image
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
                    "Tile08_Teal",                                      // Image
                    "Teal",                                             // Group
                    100,                                                // Price to buy
                    50,                                                 // Cost per house
                    6, 30, 90, 270, 400, 550,                           // Rent[]
                    1, 18, 3, 2);
            spaceModels.Add(8, sm8);

            SpaceModel sm9 = new PropertyModel(
                    "Connecticut Avenue",                               // Name
                    "A property belonging to group 'Teal'. Space#9",    // Description
                    "Tile09_Teal",                                      // Image
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
                    "Tile11_Pink",                                      // Image
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
                    "Tile13_Pink",                                      // Image
                    "Pink",                                             // Group
                    140,                                                // Price to buy
                    100,                                                // Cost per House
                    10, 50, 150, 450, 625, 750,                         // Rent[]
                    8, 22, 2, 3);
            spaceModels.Add(13, sm13);

            SpaceModel sm14 = new PropertyModel(
                    "Virginia Avenue",                                  // Name
                    "A property belonging to group 'Pink'. Space#14",   // Description
                    "Tile14_Pink",                                      // Image
                    "Pink",                                             // Group
                    160,                                                // Price to buy
                    100,                                                // Cost per House
                    12, 60, 180, 500, 700, 900,                         // Rent[]
                    10, 22, 2, 3);
            spaceModels.Add(14, sm14);

            SpaceModel sm15 = new PropertyModel(
                    "Pennsylvania Railroad",                            // Name
                    "Railroad Company. Space#15",                       // Description
                    "Tile15_Rail",                                      // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                    12, 22, 2, 3);
            spaceModels.Add(15, sm15);
            
            SpaceModel sm16 = new PropertyModel(
                    "St. James Place",                                  // Name
                    "A property belonging to group 'Orange'. Space#16", // Description
                    "Tile16_Orange",                                    // Image
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
                    "Tile18_Orange",                                    // Image
                    "Orange",                                           // Group
                    180,                                                // Price to buy
                    100,                                                // Cost per House
                    14, 70, 200, 550, 750, 950,                         // Rent[]
                    18, 22, 2, 3);
            spaceModels.Add(18, sm18);

            SpaceModel sm19 = new PropertyModel(
                    "New York Avenue",                                  // Name
                    "A property belonging to group 'Orange'. Space#19", // Description
                    "Tile19_Orange",                                    // Image
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
                    "Tile21_Red",                                       // Image
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
                    "Tile23_Red",                                       // Image
                    "Red",                                              // Group
                    220,                                                // Price to buy
                    150,                                                // Cost per House
                    18, 90, 250, 700, 875, 1050,                        // Rent[]
                    22, 16, 3, 2);
            spaceModels.Add(23, sm23);

            SpaceModel sm24 = new PropertyModel(
                    "Illinois Avenue",                                  // Name
                    "A property belonging to group 'Red'. Space#24",    // Description
                    "Tile24_Red",                                       // Image
                    "Red",                                              // Group
                    240,                                                // Price to buy
                    150,                                                // Cost per House
                    20, 100, 300, 750, 925, 1100,                       // Rent[]
                    22, 14, 3, 2);
            spaceModels.Add(24, sm24);

            SpaceModel sm25 = new PropertyModel(
                    "B.O. Railroad",                                    // Name
                    "Railroad Company. Space#25",                       // Description
                    "Tile25_Rail",                                      // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                    22, 12, 3, 2);
            spaceModels.Add(25, sm25);

            SpaceModel sm26 = new PropertyModel(
                    "Atlantic Avenue",                                  // Name
                    "A property belonging to group 'Yellow'. Space#26", // Description
                    "Tile26_Yellow",                                    // Image
                    "Yellow",                                           // Group
                    260,                                                // Price to buy
                    150,                                                // Cost per House
                    22, 110, 360, 800, 975, 1150,                       // Rent[]
                    22, 10, 3, 2);
            spaceModels.Add(26, sm26);

            SpaceModel sm27 = new PropertyModel(
                    "Ventnor Avenue",                                   // Name
                    "A property belonging to group 'Yellow'. Space#27", // Description
                    "Tile27_Yellow",                                    // Image
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
                    "Tile29_Yellow",                                    // Image
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
                    "Tile31_Green",                                     // Image
                    "Green",                                            // Group
                    300,                                                // Price to buy
                    200,                                                // Cost per House
                    26, 130, 390, 900, 1100, 1275,                      // Rent[]
                    20, 1, 2, 3);
            spaceModels.Add(31, sm31);

            SpaceModel sm32 = new PropertyModel(
                    "North Carolina Avenue",                            // Name
                    "A property belonging to group 'Green'. Space#32",  // Description
                    "Tile32_Green",                                     // Image
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
                    "Tile34_Green",                                     // Image
                    "Green",                                            // Group
                    320,                                                // Price to buy
                    200,                                                // Cost per House
                    28, 150, 45, 1000, 1200, 1400,                      // Rent[]
                    14, 1, 2, 3);
            spaceModels.Add(34, sm34);

            SpaceModel sm35 = new PropertyModel(
                    "Short Line",                                       // Name
                    "Railroad Company. Space#35",                       // Description
                    "Tile35_Rail",                                      // Image
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
                    "Tile37_Blue",                                      // Image
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
                    "A property belonging to group 'Blue'. Space#39",   // Description
                    "Tile39_Blue",                                      // Image
                    "Blue",                                             // Group
                    400,                                                // Price to buy
                    200,                                                // Cost per House
                    50, 200, 600, 1400, 1700, 2000,                     // Rent[]
                    4, 1, 2, 3);                                                     
            spaceModels.Add(39, sm39);
        }
    }
}
