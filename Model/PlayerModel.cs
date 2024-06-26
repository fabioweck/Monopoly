﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Monopoly.Model
{

    //This class holds model data and doesn't change its state/has no behavior

    public class PlayerModel
    {

        public string Name { get; set; }

        //Holds the player's position
        public System.Windows.Point Position { get; set; }

        public int Balance { get; set; } = 150;

        public string CommunityCard { get; set; } = "No community card";
        public string ChanceCard { get; set; } = "No chance card";

        //Check if the player was sent to jail
        public bool isInJail = false;

        public int attemptsToGetOutOfJail = 0;

        public SolidColorBrush Color { get; set; }
        public PlayerModel(string name, System.Windows.Point position)
        { 
            Name = name;
            Position = position;

            //Set the player color based on player's name
            switch (Name)
            {
                case "P1": // Player 1
                    Color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 102, 204));
                    break;
                case "P2": // Player 2
                    Color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 140, 0));
                    break;
                case "P3": // Player 3
                    Color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 69, 0));
                    break;
                case "P4": // Player 4
                    Color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 139, 34));
                    break;
                default:
                    Color = new SolidColorBrush(Colors.Gray);
                    break;
            }
        }

        public override string ToString()
        {
            return $"{Name} :: {Position.X} . {Position.Y}";
        }
    }
}
