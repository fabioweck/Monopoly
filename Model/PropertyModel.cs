﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model
{
    internal class PropertyModel : SpaceModel 
    {
        public string Group { get; set; }
        public int Price { get; set; } = 0;
        public int[] Rent { get; set; } = { };
        public int HousePrice { get; set; } = 0;
        public int HousesBuilt { get; set; } = 0;
        public PlayerModel Owner = null;

        // For Railroad companies:
        public PropertyModel(string name, string description, Bitmap img, int price, int rent0, int rent1, int rent2, int rent3) : base(name, description, img)
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
        public PropertyModel(string name, string description, Bitmap img, int price, int rent0, int rent1) : base(name, description, img)
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
        public PropertyModel(string name, string description, Bitmap img, string group, int price, int housePrice, int rent0, int rent1, int rent2, int rent3, int rent4, int rent5) : base(name, description, img) 
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