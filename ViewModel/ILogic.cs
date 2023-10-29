using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.ViewModel
{
    public interface ILogic<T>
    {
        void Add();
        void Move(int column, int row);
        void Remove();
    }
}
