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
        public int instanceNumber = 0;

        public PlayerModel Player { get; set; }
        public static PlayerViewModel CurrentPlayer = null;
        public static List<PlayerModel> PlayerModels = new List<PlayerModel>();
        public static List<PlayerViewModel> Players = new List<PlayerViewModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public int Column 
        { 
            get { return (int)PlayerModels[instanceNumber].Position.X;}

            set {
                Point p = PlayerModels[instanceNumber].Position;
                PlayerModels[instanceNumber].Position = new Point(p.X + value, p.Y);
                OnPropertyChanged(nameof(Column));
            } 
        }

        public int Row 
        { 
            get { return (int)PlayerModels[instanceNumber].Position.Y; }

            set
            {
                Point p = PlayerModels[instanceNumber].Position;
                PlayerModels[instanceNumber].Position = new Point(p.X, p.Y + value);
                OnPropertyChanged(nameof(Row));
            }
        }

        public string Name { get { return Player.Name; } }

        public PlayerViewModel()
        {
            instanceNumber = PVMcount;
            PVMcount++;
            Point point; 
            if (PVMcount == 1) point = new Point(1, 1);
            else if (PVMcount == 2) point = new Point(1, 3);
            else if (PVMcount == 3) point = new Point(3, 1);
            else point = new Point(3, 3);
            Player = new PlayerModel($"P{PVMcount}", point);
            PlayerModels.Add(Player);
            Players.Add(this);
            CurrentPlayer = Players[0];
        }

        public void MovePlayer(int column, int row)
        {
            Column = column;
            Row = row;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // This is to assist in debugging:
        public override string ToString()
        {
            return $"{Name} :: R{Row} | C{Column}";
        }
    }
}
