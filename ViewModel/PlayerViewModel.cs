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
    /// <summary>
    /// <see cref="PlayerViewModel"/> controls the changes to <see cref="PlayerModel"/>'s poroperties and reflects those changes on <see cref="MainWindow"/>.
    /// </summary>
    public class PlayerViewModel : INotifyPropertyChanged
    {
        public static int PVMcount = 0;
        public static PlayerModel CurrentPlayer { get; set; }
        public static List<PlayerModel> PlayerModels { get; set; }

        //public static PlayerModel Player1 { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public int Column 
        { 
            get { return (int)CurrentPlayer.Position.X;}

            set {
                int x = (int)CurrentPlayer.Position.X;
                x = value;
                    OnPropertyChanged(nameof(Column));
            } 
        }

        public int Row 
        { 
            get { return (int)CurrentPlayer.Position.Y; }

            set
            {
                int y = (int)CurrentPlayer.Position.Y;
                y = value;
                OnPropertyChanged(nameof(Row));
            }
        }

        public string Name { get { return CurrentPlayer.Name; } }

        public PlayerViewModel()
        {
            PVMcount++;
            PlayerModels.Add(new PlayerModel($"Player{PVMcount}", new Point(2,2)));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MovePlayer(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }
}
