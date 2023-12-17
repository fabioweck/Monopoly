using Monopoly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.ViewModel
{
    public class DieViewModel
    {
        public DieModel Die1 { get; set; }
        public DieModel Die2 { get; set; }

        public DieViewModel()
        {
            Die1 = new DieModel();
            Die2 = new DieModel();
        }

        //Simulate the dice rolling 
        public int[] RollDice()
        {
            Random rnd = new Random();

            //Generates numbers between 1 and 6 and simulate each face of a single die
            int[] randomNumber = { Die1.Faces[rnd.Next(1, 6)], Die1.Faces[rnd.Next(1, 6)] };

            return randomNumber;
        }
    }
}
