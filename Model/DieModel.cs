using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model
{
    public class DieModel
    {
        public int[] Faces {  get; set; }

        public DieModel()
        {          
            Faces = new int[6] { 1, 2, 3, 4, 5, 6 };
        }
    }
}
