using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model
{
    internal class SpaceModel
    {
        public string Name { get; set; }
        public string Description { get; set; } // We should display this when hovering the mouse over the space.
        public string Type { get; set; } = "Neutral";
        public string Group { get; set; }
        public int Price { get; set; } = 0;
        public int[] Rent { get; set; } = { };




        public int HousePrice {  get; set; } = 0;
        public int Houses { get; set; } = 0;
        public Bitmap Image = null;

        public SpaceModel(string name, string description, string type, Bitmap img)
        {
            Name = name;
            Description = description;
            Type = type;
            Image = img;
        }

        // For Railroad companies:
        public SpaceModel(string name, string description, Bitmap img, int price, int rent0, int rent1, int rent2, int rent3)
        {
            Type = "Property";
            Group = "Railroad";
            Name = name;
            Description = description;
            Image = img;
            Price = price;
            Rent[0] = rent0;
            Rent[1] = rent1;
            Rent[2] = rent2;
            Rent[3] = rent3;
        }

        // For Utility companies:
        public SpaceModel(string name, string description, Bitmap img, int price, int rent0, int rent1)
        {
            Type = "Property";
            Group = "Utility";
            Name = name;
            Description = description;
            Image = img;
            Price = price;
            Rent[0] = rent0;
            Rent[1] = rent1;
        }


        // For properties:
        public SpaceModel(string name, string description, Bitmap img, string group, int price, int housePrice, int rent0, int rent1, int rent2, int rent3, int rent4, int rent5)
        {
            Type = "Property";
            Name = name;
            Description = description;
            Image = img;
            Group = group;
            Price = price;
            HousePrice = housePrice;
            Rent[0] = rent0;
            Rent[1] = rent1;
            Rent[2] = rent2;
            Rent[3] = rent3;
            Rent[4] = rent4;
            Rent[5] = rent5;
        }
    }
}
