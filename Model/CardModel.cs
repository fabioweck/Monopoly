using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model
{
    public class CardModel
    {
        public string Description { get; set; }

        //Describes what happens after getting the card
        public string Effect { get; set; }

        //If a card holds a value
        public int Value { get; set; } = 0;

        //If a card requires the player to move
        public int Move { get; set; } = 0;

        public CardModel(string description, string effect, int value, int move = 0)
        {
            Description = description;
            Effect = effect;
            Value = value;
            Move = move;
        }
    }
}