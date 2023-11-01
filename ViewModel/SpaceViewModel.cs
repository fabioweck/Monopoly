using Monopoly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.ViewModel
{
    public class SpaceViewModel
    {
        public static Dictionary<int, SpaceModel> spaceModels = new Dictionary<int, SpaceModel>();

        public SpaceViewModel()
        {

        }

        // Assign ownership of a property.
        public void AddOwner(PropertyModel property, PlayerViewModel pvm)
        {
            property.Owner = pvm;
        }

        public void BuyProperty(PropertyModel property, PlayerViewModel pvm)
        {
            AddOwner(property, pvm);
            pvm.Player.Balance -= property.Price;
        }

        public void RemoveOwner(PropertyModel property, PlayerViewModel pvm)
        {
            property.Owner = null;
        }

        // Resolves what happens when the player lands on a space.
        public void Resolve(PropertyModel property, PlayerViewModel pvm)
        {
            // What happens when the player lands here?
            // Depending on the Type
            // If it's a property, offer to buy - or pay rent to owner.
            if (property.Type == "Property")
            {
                if (property.Owner == null)
                {

                }
            }
            // If it's a Card, open a card.
            // If it's Jail, go to jail.
            // If it's Bank, resolve money exchange.
            // If it's neutral, do nothing.
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
                    Properties.Resources.Tile0,                          
                   1, 1, 3, 3));

            SpaceModel sm1 = new PropertyModel(
                    "Meditarranean Avenue",                             // Name
                    "A property belonging to group 'Brown'. Space #1",  // Description
                    Properties.Resources.Tile01_Brown,                  // Image
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
                    Properties.Resources.Tile02_Chest,
                    1, 6, 3, 2));

            SpaceModel sm3 = new PropertyModel(
                   "Baltic Avenue",                                     // Name
                   "A property belonging to group 'Brown'. Space #3",   // Description
                   Properties.Resources.Tile03_Brown,                   // Image
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
                    Properties.Resources.Tile04_Tax,                         
                    1, 10, 3, 2));

            SpaceModel sm5 = new PropertyModel(
                    "Reading Railroad",                                 // Name
                    "Railroad Company. Space#5",                        // Description
                    Properties.Resources.Tile05_Rail,                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                   1, 12, 3, 2);
            spaceModels.Add(5, sm5);

            SpaceModel sm6 = new PropertyModel(
                    "Oriental Avenue",                                  // Name
                    "A property belonging to group 'Teal'. Space#6",    // Description
                    Properties.Resources.Tile06_Teal,                   // Image
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
                    Properties.Resources.Tile07_Chance,
                   1, 16, 3, 2));

            SpaceModel sm8 = new PropertyModel(
                    "Vermont Avenue",                                   // Name
                    "A property belonging to group 'Teal'. Space#8",    // Description
                    Properties.Resources.Tile08_Teal,                   // Image
                    "Teal",                                             // Group
                    100,                                                // Price to buy
                    50,                                                 // Cost per house
                    6, 30, 90, 270, 400, 550,                           // Rent[]
                    1, 18, 3, 2);
            spaceModels.Add(8, sm8);

            SpaceModel sm9 = new PropertyModel(
                    "Connecticut Avenue",                               // Name
                    "A property belonging to group 'Teal'. Space#9",    // Description
                    Properties.Resources.Tile09_Teal,                   // Image
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
                    Properties.Resources.Tile07_Chance,
                   1, 22, 3, 3));

            SpaceModel sm11 = new PropertyModel(
                    "St. Charles Place",                                // Name
                    "A property belonging to group 'Pink'. Space#11",   // Description
                    Properties.Resources.Tile11_Pink,                   // Image
                    "Pink",                                             // Group
                    140,                                                // Price to buy
                    100,                                                // Cost per House
                    10, 50, 150, 450, 625, 750,                         // Rent[]
                    4, 22, 2, 3);
            spaceModels.Add(11,sm11);

            SpaceModel sm12 = new PropertyModel(
                    "Electric Company",
                    "Utility Company. Space#12",
                    Properties.Resources.Tile12_Light,
                    150,
                    4, 10,      // Multiply this by the dice roll
                    6, 22, 2, 3);
            spaceModels.Add(12, sm12);


            SpaceModel sm13 = new PropertyModel(
                    "States Avenue",                                    // Name
                    "A property belonging to group 'Pink'. Space#13",   // Description
                    Properties.Resources.Tile13_Pink,                   // Image
                    "Pink",                                             // Group
                    140,                                                // Price to buy
                    100,                                                // Cost per House
                    10, 50, 150, 450, 625, 750,                         // Rent[]
                    8, 22, 2, 3);
            spaceModels.Add(13, sm13);

            SpaceModel sm14 = new PropertyModel(
                    "Virginia Avenue",                                  // Name
                    "A property belonging to group 'Pink'. Space#14",   // Description
                    Properties.Resources.Tile14_Pink,                   // Image
                    "Pink",                                             // Group
                    160,                                                // Price to buy
                    100,                                                // Cost per House
                    12, 60, 180, 500, 700, 900,                         // Rent[]
                    10, 22, 2, 3);
            spaceModels.Add(14, sm14);

            SpaceModel sm15 = new PropertyModel(
                    "Pennsylvania Railroad",                            // Name
                    "Railroad Company. Space#15",                       // Description
                    Properties.Resources.Tile15_Rail,                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                    12, 22, 2, 3);
            spaceModels.Add(15, sm15);
            
            SpaceModel sm16 = new PropertyModel(
                    "St. James Place",                                  // Name
                    "A property belonging to group 'Orange'. Space#16", // Description
                    Properties.Resources.Tile16_Orange,                 // Image
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
                    Properties.Resources.Tile17_Chest,                        
                    16, 22, 2, 3));

            SpaceModel sm18 = new PropertyModel(
                    "Tennessee Avenue",                                 // Name
                    "A property belonging to group 'Orange'. Space#18", // Description
                    Properties.Resources.Tile18_Orange,                 // Image
                    "Orange",                                           // Group
                    180,                                                // Price to buy
                    100,                                                // Cost per House
                    14, 70, 200, 550, 750, 950,                         // Rent[]
                    18, 22, 2, 3);
            spaceModels.Add(18, sm18);

            SpaceModel sm19 = new PropertyModel(
                    "New York Avenue",                                  // Name
                    "A property belonging to group 'Orange'. Space#19", // Description
                    Properties.Resources.Tile19_Orange,                 // Image
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
                    Properties.Resources.Tile20_Parking,
                   22, 22, 3, 3));

            SpaceModel sm21 = new PropertyModel(
                    "Kentucky Avenue",                                  // Name
                    "A property belonging to group 'Red'. Space#21",    // Description
                    Properties.Resources.Tile21_Red,                    // Image
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
                    Properties.Resources.Tile22_Chance,
                   22, 18, 3, 2));

            SpaceModel sm23 = new PropertyModel(
                    "Indiana Avenue",                                   // Name
                    "A property belonging to group 'Red'. Space#23",    // Description
                    Properties.Resources.Tile23_Red,                    // Image
                    "Red",                                              // Group
                    220,                                                // Price to buy
                    150,                                                // Cost per House
                    18, 90, 250, 700, 875, 1050,                        // Rent[]
                    22, 16, 3, 2);
            spaceModels.Add(23, sm23);

            SpaceModel sm24 = new PropertyModel(
                    "Illinois Avenue",                                  // Name
                    "A property belonging to group 'Red'. Space#24",    // Description
                    Properties.Resources.Tile24_Red,                    // Image
                    "Red",                                              // Group
                    240,                                                // Price to buy
                    150,                                                // Cost per House
                    20, 100, 300, 750, 925, 1100,                       // Rent[]
                    22, 14, 3, 2);
            spaceModels.Add(24, sm24);

            SpaceModel sm25 = new PropertyModel(
                    "B.O. Railroad",                                    // Name
                    "Railroad Company. Space#25",                       // Description
                    Properties.Resources.Tile25_Rail,                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                    22, 12, 3, 2);
            spaceModels.Add(25, sm25);

            SpaceModel sm26 = new PropertyModel(
                    "Atlantic Avenue",                                  // Name
                    "A property belonging to group 'Yellow'. Space#26", // Description
                    Properties.Resources.Tile26_Yellow,                 // Image
                    "Yellow",                                           // Group
                    260,                                                // Price to buy
                    150,                                                // Cost per House
                    22, 110, 360, 800, 975, 1150,                       // Rent[]
                    22, 10, 3, 2);
            spaceModels.Add(26, sm26);

            SpaceModel sm27 = new PropertyModel(
                    "Ventnor Avenue",                                   // Name
                    "A property belonging to group 'Yellow'. Space#27", // Description
                    Properties.Resources.Tile27_Yellow,                 // Image
                    "Yellow",                                           // Group
                    260,                                                // Price to buy
                    150,                                                // Cost per House
                    22, 110, 360, 800, 975, 1150,                       // Rent[]
                    22, 8, 3, 2);
            spaceModels.Add(27, sm27);

            SpaceModel sm28 = new PropertyModel(
                    "Water Company",
                    "Utility Company. Space#28",
                    Properties.Resources.Tile28_Water,
                    150,
                    4, 10, // Multiply this by the dice roll
                   22, 6, 3, 2);
            spaceModels.Add(28, sm28);

            SpaceModel sm29 = new PropertyModel(
                    "Marvin Gardens",                                   // Name
                    "A property belonging to group 'Yellow'. Space#29", // Description
                    Properties.Resources.Tile29_Yellow,                 // Image
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
                    Properties.Resources.Tile30_GoToJail,
                   22, 1, 3, 3));

            SpaceModel sm31 = new PropertyModel(
                    "Pacific Avenue",                                   // Name
                    "A property belonging to group 'Green'. Space#31",  // Description
                    Properties.Resources.Tile31_Green,                  // Image
                    "Green",                                            // Group
                    300,                                                // Price to buy
                    200,                                                // Cost per House
                    26, 130, 390, 900, 1100, 1275,                      // Rent[]
                    20, 1, 2, 3);
            spaceModels.Add(31, sm31);

            SpaceModel sm32 = new PropertyModel(
                    "North Carolina Avenue",                            // Name
                    "A property belonging to group 'Green'. Space#32",  // Description
                    Properties.Resources.Tile32_Green,                  // Image
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
                    Properties.Resources.Tile33_Chest,
                   16, 1, 2, 3));

            SpaceModel sm34 = new PropertyModel(
                    "Pennsylvania Avenue",                              // Name
                    "A property belonging to group 'Green'. Space#34",  // Description
                    Properties.Resources.Tile34_Green,                  // Image
                    "Green",                                            // Group
                    320,                                                // Price to buy
                    200,                                                // Cost per House
                    28, 150, 45, 1000, 1200, 1400,                      // Rent[]
                    14, 1, 2, 3);
            spaceModels.Add(34, sm34);

            SpaceModel sm35 = new PropertyModel(
                    "Short Line",                                       // Name
                    "Railroad Company. Space#35",                       // Description
                    Properties.Resources.Tile35_Rail,                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200,                                   // Rent[]
                    12, 1, 2, 3);
            spaceModels.Add(35, sm35);

            spaceModels.Add(36,
                new SpaceModel(
                    "Chance",
                    "Chance Cards. Space#36",
                    "Card",
                    Properties.Resources.Tile36_Chance,
                   10, 1, 2, 3));

            SpaceModel sm37 = new PropertyModel(
                    "Park Place",                                       // Name
                    "A property belonging to group 'Blue'. Space#37",   // Description
                    Properties.Resources.Tile37_Blue,                   // Image
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
                    Properties.Resources.Tile38_Tax,
                   6, 1, 2, 3));

            SpaceModel sm39 = new PropertyModel(
                    "Boardwalk",                                        // Name
                    "A property belonging to group 'Blue'. Space#37",   // Description
                    Properties.Resources.Tile37_Blue,                   // Image
                    "Blue",                                             // Group
                    400,                                                // Price to buy
                    200,                                                // Cost per House
                    50, 200, 600, 1400, 1700, 2000,                     // Rent[]
                    4, 1, 2, 3);                                                     
            spaceModels.Add(39, sm39);
        }
    }
}
