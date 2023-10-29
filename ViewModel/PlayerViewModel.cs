using Monopoly.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Monopoly.ViewModel
{
    public class PlayerViewModel : INotifyPropertyChanged
    {

        public static PlayerModel Player1 { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public int Column 
        { 
            get { return Player1.Position[0];}

            set {
                    Player1.Position[0] = value;
                    OnPropertyChanged(nameof(Column));
            } 
        }

        public int Row 
        { 
            get { return Player1.Position[0]; }

            set
            {
                Player1.Position[1] = value;
                OnPropertyChanged(nameof(Row));
            }
        }

        public string Name { get { return Player1.Name; } }

        public PlayerViewModel()
        {
            Player1 = new PlayerModel("Player1", new int[] {2,2});
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MovePlayer(int column, int row)
        {
            Column = column;
            Player1.Position[1] = row;
        }
    }
}
