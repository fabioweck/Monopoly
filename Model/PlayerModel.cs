using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Monopoly.Model
{

    //This class holds model data and doesn't change its state/has no behavior

    public class PlayerModel
    {
        public string Name { get; set; }
        public int[] Position { get; set; }

        public PlayerModel(string name, int[] position)
        { 
            Name = name;
            Position = position;
        }
    }
}
