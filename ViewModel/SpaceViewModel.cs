using Monopoly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.ViewModel
{
    internal class SpaceViewModel
    {
        public Dictionary<int, SpaceModel> spaceModels;


        // Assign ownership of a property.
        public void AddOwner(SpaceModel space, PlayerModel player)
        {
            space.Owner = player;
        }

        public void BuyProperty(SpaceModel space, PlayerModel player)
        {
            AddOwner(space, player);
            player.Balance -= space.Price;
        }

        public void RemoveOwner(SpaceModel space, PlayerModel player)
        {
            space.Owner = null;
        }

        // Resolves what happens when the player lands on a space.
        public void Resolve(SpaceModel space, PlayerModel player)
        {
            // What happens when the player lands here?
            // Depending on the Type
            // If it's a property, offer to buy - or pay rent to owner.
            if (space.Type == "Property")
            {
                if (space.Owner == null)
                {

                }
            }
            // If it's a Card, open a card.
            // If it's Jail, go to jail.
            // If it's Bank, resolve money exchange.
            // If it's neutral, do nothing.
        }


        public void PopulateBoard()
        {
            spaceModels.Add(0,
                new SpaceModel(
                    "Start",
                    "Starting point. Pass here to receive $200.",
                    "Neutral", // Neutral because the money is earned when we pass, not when we land.
                    Properties.Resources.Tile0));

            spaceModels.Add(1,
                new SpaceModel(
                    "Meditarranean Avenue",                             // Name
                    "A property belonging to group 'Brown'. Space #1",  // Description
                    Properties.Resources.Tile01_Brown,                  // Image
                    "Brown",                                            // Group
                    60,                                                 // Price to buy
                    50,                                                 // Cost per house
                    2, 10, 30, 90, 160, 250));                          // Rent[]

            spaceModels.Add(2,
                new SpaceModel(
                    "Community Chest",
                    "Community Chest Cards. Space #2",
                    "Card",
                    Properties.Resources.Tile02_Chest));

            spaceModels.Add(3,
                new SpaceModel(
                    "Baltic Avenue",                                    // Name
                    "A property belonging to group 'Brown'. Space #3",  // Description
                    Properties.Resources.Tile03_Brown,                  // Image
                    "Brown",                                            // Group
                    60,                                                 // Price to buy
                    50,                                                 // Cost per house
                    4, 20, 60, 180, 320, 450));                         // Rent[]

            spaceModels.Add(4,
                new SpaceModel(
                    "Tax Income",
                    "Pay Taxes! Space#4",
                    "Bank",
                    Properties.Resources.Tile04_Tax));

            spaceModels.Add(5,
                new SpaceModel(
                    "Reading Railroad",                                 // Name
                    "Railroad Company. Space#5",                        // Description
                    Properties.Resources.Tile05_Rail,                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200));                                 // Rent[]

            spaceModels.Add(6,
                new SpaceModel(
                    "Oriental Avenue",                                  // Name
                    "A property belonging to group 'Teal'. Space#6",    // Description
                    Properties.Resources.Tile06_Teal,                   // Image
                    "Teal",                                             // Group
                    100,                                                // Price to buy
                    50,                                                 // Cost per house
                    6, 30, 90, 270, 400, 550));                         // Rent[]

            spaceModels.Add(7,
                new SpaceModel(
                    "Chance",
                    "Chance Cards. Space#7",
                    "Card",
                    Properties.Resources.Tile07_Chance));

            spaceModels.Add(8,
                new SpaceModel(
                    "Vermont Avenue",                                   // Name
                    "A property belonging to group 'Teal'. Space#8",    // Description
                    Properties.Resources.Tile08_Teal,                   // Image
                    "Teal",                                             // Group
                    100,                                                // Price to buy
                    50,                                                 // Cost per house
                    6, 30, 90, 270, 400, 550));                         // Rent[]
            
            spaceModels.Add(9,
                new SpaceModel(
                    "Connecticut Avenue",                               // Name
                    "A property belonging to group 'Teal'. Space#9",    // Description
                    Properties.Resources.Tile09_Teal,                   // Image
                    "Teal",                                             // Group
                    120,                                                // Price to buy
                    50,                                                 // Cost per House
                    8, 40, 100, 300, 450, 600));                        // Rent[]

            spaceModels.Add(10,
                new SpaceModel(
                    "Jail Visitor",
                    "You are just visiting. Space#10",
                    "Neutral",
                    Properties.Resources.Tile07_Chance));
            
            spaceModels.Add(11,
                new SpaceModel(
                    "St. Charles Place",                                // Name
                    "A property belonging to group 'Pink'. Space#11",   // Description
                    Properties.Resources.Tile11_Pink,                   // Image
                    "Pink",                                             // Group
                    140,                                                // Price to buy
                    100,                                                // Cost per House
                    10, 50, 150, 450, 625, 750));                       // Rent[]


            spaceModels.Add(12,
                new SpaceModel(
                    "Electric Company",
                    "Utility Company. Space#12",
                    Properties.Resources.Tile12_Light,
                    150,
                    4, 10));    // In this case, we will have to multiply this value by the rolled dice amount.

            spaceModels.Add(13,
                new SpaceModel(
                    "States Avenue",                                    // Name
                    "A property belonging to group 'Pink'. Space#13",   // Description
                    Properties.Resources.Tile13_Pink,                   // Image
                    "Pink",                                             // Group
                    140,                                                // Price to buy
                    100,                                                // Cost per House
                    10, 50, 150, 450, 625, 750));                       // Rent[]

            spaceModels.Add(14,
                new SpaceModel(
                    "Virginia Avenue",                                  // Name
                    "A property belonging to group 'Pink'. Space#14",   // Description
                    Properties.Resources.Tile14_Pink,                   // Image
                    "Pink",                                             // Group
                    160,                                                // Price to buy
                    100,                                                // Cost per House
                    12, 60, 180, 500, 700, 900));                       // Rent[]

            spaceModels.Add(15,
                new SpaceModel(
                    "Pennsylvania Railroad",                            // Name
                    "Railroad Company. Space#15",                       // Description
                    Properties.Resources.Tile15_Rail,                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200));                                 // Rent[]
            
            spaceModels.Add(16,
                new SpaceModel(
                    "St. James Place",                                  // Name
                    "A property belonging to group 'Orange'. Space#16", // Description
                    Properties.Resources.Tile16_Orange,                 // Image
                    "Orange",                                             // Group
                    180,                                                // Price to buy
                    100,                                                // Cost per House
                    14, 70, 200, 550, 750, 950));                       // Rent[]

            spaceModels.Add(17,
                new SpaceModel(
                    "Community Chest",
                    "Community Chest Cards. Space #17",
                    "Card",
                    Properties.Resources.Tile17_Chest));

            spaceModels.Add(18,
                new SpaceModel(
                    "Tennessee Avenue",                                 // Name
                    "A property belonging to group 'Orange'. Space#18", // Description
                    Properties.Resources.Tile18_Orange,                 // Image
                    "Orange",                                           // Group
                    180,                                                // Price to buy
                    100,                                                // Cost per House
                    14, 70, 200, 550, 750, 950));                       // Rent[]
            
            spaceModels.Add(19,
                new SpaceModel(
                    "New York Avenue",                                  // Name
                    "A property belonging to group 'Orange'. Space#19", // Description
                    Properties.Resources.Tile19_Orange,                 // Image
                    "Orange",                                           // Group
                    200,                                                // Price to buy
                    100,                                                // Cost per House
                    16, 80, 220, 600, 800, 1000));                      // Rent[]
            
            spaceModels.Add(20,
                new SpaceModel(
                    "Free Parking",
                    "Free Parking. Space #20",
                    "Neutral",
                    Properties.Resources.Tile20_Parking));
            
            spaceModels.Add(21,
                new SpaceModel(
                    "Kentucky Avenue",                                  // Name
                    "A property belonging to group 'Red'. Space#21",    // Description
                    Properties.Resources.Tile21_Red,                    // Image
                    "Red",                                              // Group
                    220,                                                // Price to buy
                    150,                                                // Cost per House
                    18, 90, 250, 700, 875, 1050));                      // Rent[]

            spaceModels.Add(22,
                new SpaceModel(
                    "Chance",
                    "Chance Cards. Space#22",
                    "Card",
                    Properties.Resources.Tile22_Chance));

            spaceModels.Add(23,
                new SpaceModel(
                    "Indiana Avenue",                                   // Name
                    "A property belonging to group 'Red'. Space#23",    // Description
                    Properties.Resources.Tile23_Red,                    // Image
                    "Red",                                              // Group
                    220,                                                // Price to buy
                    150,                                                // Cost per House
                    18, 90, 250, 700, 875, 1050));                      // Rent[]

            spaceModels.Add(24,
                new SpaceModel(
                    "Illinois Avenue",                                  // Name
                    "A property belonging to group 'Red'. Space#24",    // Description
                    Properties.Resources.Tile24_Red,                    // Image
                    "Red",                                              // Group
                    240,                                                // Price to buy
                    150,                                                // Cost per House
                    20, 100, 300, 750, 925, 1100));                     // Rent[]

            spaceModels.Add(25,
                new SpaceModel(
                    "B.O. Railroad",                                    // Name
                    "Railroad Company. Space#25",                       // Description
                    Properties.Resources.Tile25_Rail,                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200));                                 // Rent[]

            spaceModels.Add(26,
                new SpaceModel(
                    "Atlantic Avenue",                                  // Name
                    "A property belonging to group 'Yellow'. Space#26", // Description
                    Properties.Resources.Tile26_Yellow,                 // Image
                    "Yellow",                                           // Group
                    260,                                                // Price to buy
                    150,                                                // Cost per House
                    22, 110, 360, 800, 975, 1150));                     // Rent[]

            spaceModels.Add(27,
                new SpaceModel(
                    "Ventnor Avenue",                                   // Name
                    "A property belonging to group 'Yellow'. Space#27", // Description
                    Properties.Resources.Tile27_Yellow,                 // Image
                    "Yellow",                                           // Group
                    260,                                                // Price to buy
                    150,                                                // Cost per House
                    22, 110, 360, 800, 975, 1150));                     // Rent[]

            spaceModels.Add(28,
                new SpaceModel(
                    "Water Company",
                    "Utility Company. Space#28",
                    Properties.Resources.Tile28_Water,
                    150,
                    4, 10));    // In this case, we will have to multiply this value by the rolled dice amount.

            spaceModels.Add(29,
                new SpaceModel(
                    "Marvin Gardens",                                   // Name
                    "A property belonging to group 'Yellow'. Space#29", // Description
                    Properties.Resources.Tile29_Yellow,                 // Image
                    "Yellow",                                           // Group
                    280,                                                // Price to buy
                    150,                                                // Cost per House
                    24, 120, 360, 850, 1025, 1200));                    // Rent[]

            spaceModels.Add(30,
                new SpaceModel(
                    "Go To Jail",
                    "Go to jail. Space#30",
                    "Jail",
                    Properties.Resources.Tile30_GoToJail));

            spaceModels.Add(31,
                new SpaceModel(
                    "Pacific Avenue",                                   // Name
                    "A property belonging to group 'Green'. Space#31",  // Description
                    Properties.Resources.Tile31_Green,                  // Image
                    "Green",                                            // Group
                    300,                                                // Price to buy
                    200,                                                // Cost per House
                    26, 130, 390, 900, 1100, 1275));                    // Rent[]

            spaceModels.Add(32,
                new SpaceModel(
                    "North Carolina Avenue",                                   // Name
                    "A property belonging to group 'Green'. Space#32",  // Description
                    Properties.Resources.Tile32_Green,                  // Image
                    "Green",                                            // Group
                    300,                                                // Price to buy
                    200,                                                // Cost per House
                    26, 130, 390, 900, 1100, 1275));                    // Rent[]            

            spaceModels.Add(33,
                new SpaceModel(
                    "Community Chest",
                    "Community Chest Cards. Space #33",
                    "Card",
                    Properties.Resources.Tile33_Chest));

            spaceModels.Add(34,
                new SpaceModel(
                    "Pennsylvania Avenue",                              // Name
                    "A property belonging to group 'Green'. Space#34",  // Description
                    Properties.Resources.Tile34_Green,                  // Image
                    "Green",                                            // Group
                    320,                                                // Price to buy
                    200,                                                // Cost per House
                    28, 150, 45, 1000, 1200, 1400));                    // Rent[]    

            spaceModels.Add(35,
                new SpaceModel(
                    "Short Line",                                       // Name
                    "Railroad Company. Space#35",                       // Description
                    Properties.Resources.Tile35_Rail,                   // Image
                    200,                                                // Price to buy
                    25, 50, 100, 200));                                 // Rent[]

            spaceModels.Add(36,
                new SpaceModel(
                    "Chance",
                    "Chance Cards. Space#36",
                    "Card",
                    Properties.Resources.Tile36_Chance));

            spaceModels.Add(37,
                new SpaceModel(
                    "Park Place",                                       // Name
                    "A property belonging to group 'Blue'. Space#37",   // Description
                    Properties.Resources.Tile37_Blue,                   // Image
                    "Blue",                                             // Group
                    350,                                                // Price to buy
                    200,                                                // Cost per House
                    35, 175, 500, 1100, 1300, 1500));                   // Rent[]    

            spaceModels.Add(38,
                new SpaceModel(
                    "Luxury Tax",
                    "Pay Taxes! Space#38",
                    "Bank",
                    Properties.Resources.Tile38_Tax));

            spaceModels.Add(39,
                new SpaceModel(
                    "Boardwalk",                                        // Name
                    "A property belonging to group 'Blue'. Space#37",   // Description
                    Properties.Resources.Tile37_Blue,                   // Image
                    "Blue",                                             // Group
                    400,                                                // Price to buy
                    200,                                                // Cost per House
                    50, 200, 600, 1400, 1700, 2000));                   // Rent[] 
        }
    }
}
