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
        public Point Position { get; set; }

        public CardModel(string description, int column, int row)
        {
            Description = description;
            Position = new Point(column, row);
        }
    }
}
