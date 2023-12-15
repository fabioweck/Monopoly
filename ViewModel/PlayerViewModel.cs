using Monopoly.Model;
using Monopoly.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Monopoly.ViewModel
{
    /// <summary>
    /// <see cref="PlayerViewModel"/> controls the changes to <see cref="PlayerModel"/>'s poroperties and reflects those changes on <see cref="MainWindow"/>.
    /// </summary>
    public class PlayerViewModel : INotifyPropertyChanged
    {
        //Delegates
        public delegate void Transaction(int amount);

        // Static Fields

        // How many players are playing:
        public static int PVMcount = 0;

        public static PlayerViewModel CurrentPlayer = null;

        // A list of our player models:
        public static List<PlayerModel> PlayerModels = new List<PlayerModel>();

        // A list of our player view-models:
        public static List<PlayerViewModel> Players = new List<PlayerViewModel>();

        // Member Fields
        public int instanceNumber = 0;
        public PlayerModel Player { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        // Member Properties
        public string Name { get { return Player.Name; } }
        public SolidColorBrush Color { get { return Player.Color; } }
        public int Position { get; set; } = 0;
        public int Column 
        { 
            get { return (int)PlayerModels[instanceNumber].Position.X;}

            set {
                Point p = PlayerModels[instanceNumber].Position;
                PlayerModels[instanceNumber].Position = new Point(value, p.Y);
                OnPropertyChanged(nameof(Column));
            } 
        }

        public int Row 
        { 
            get { return (int)PlayerModels[instanceNumber].Position.Y; }

            set
            {
                Point p = PlayerModels[instanceNumber].Position;
                PlayerModels[instanceNumber].Position = new Point(p.X, value);
                OnPropertyChanged(nameof(Row));
            }
        }

        public int Balance
        {
            get { return PlayerModels[instanceNumber].Balance; }

            set { PlayerModels[instanceNumber].Balance = value; OnPropertyChanged(nameof(Balance));}
        }

        public string Card
        {
            get { return PlayerModels[instanceNumber].Card; }

            set { PlayerModels[instanceNumber].Card = value; OnPropertyChanged(nameof(Card)); }
        }

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

        public void LapCompleted(int amount)
        {
            Player.Balance += amount;
        }

        public void ChangeBalance(Transaction transaction, int amount)
        {
            transaction(amount);
        }

        // Moves the player, adjusting their token according to the segment of the board. Player tokens never overlap each other.
        public void MovePlayer(int spaces)
        {
            int newPosition = Position + spaces;
            if (newPosition > 39) { 
                newPosition -= 40;
                ChangeBalance(LapCompleted, 200);
            }
            var targetSpace = SpaceViewModel.spaceModels[newPosition];
                Column = targetSpace.Column;
                Row = targetSpace.Row;
                Position = newPosition;

            if ((newPosition > 10 && newPosition < 20))
                Column++;

            if ((newPosition > 20 && newPosition < 30))
                Row++;

            if(Players.Count > 1) 
                if (CurrentPlayer.Name ==  Players[1].Name)
                    Column++;

            if (Players.Count > 2)
                if (CurrentPlayer.Name == Players[2].Name)
                    Row++;

            if (Players.Count>3)
                if (CurrentPlayer.Name == Players[3].Name)
                {
                    Row++;
                    Column++;
                }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{Name} :: R{Row} | C{Column} :: B{Balance}";
        }

        public static void GoToJail()
        {
            //If the player is behind position 10 (prison)
            if (CurrentPlayer.Position < 10)
            {
                int move = 10 - CurrentPlayer.Position;
                CurrentPlayer.MovePlayer(move);
            }
            else //If the player is ahead of position 10, move back to prison
            {
                int move = 10 - CurrentPlayer.Position;
                CurrentPlayer.MovePlayer(move);
            }
        }

        public static void GoToNextRailroad()
        {
            // Move to Railroad at space #5
            if (CurrentPlayer.Position < 5)
            {
                int move = 5 - CurrentPlayer.Position;
                CurrentPlayer.MovePlayer(move);
            }

            // Move to Railroad at space #15
            else if (CurrentPlayer.Position > 5 && CurrentPlayer.Position < 15) 
            {
                int move = 15 - CurrentPlayer.Position;
                CurrentPlayer.MovePlayer(move);
            }

            // Move to Railroad at space #25
            else if (CurrentPlayer.Position > 15 && CurrentPlayer.Position < 25)
            {
                int move = 25 - CurrentPlayer.Position;
                CurrentPlayer.MovePlayer(move);
            }

            // Move to Railroad at space #35
            else if (CurrentPlayer.Position > 25 && CurrentPlayer.Position < 35)
            {
                int move = 35 - CurrentPlayer.Position;
                CurrentPlayer.MovePlayer(move);
            }
            // Move to Railroad at space #5
            else
            {
                int move = 5 - CurrentPlayer.Position + 40;
                CurrentPlayer.MovePlayer(move);
            }
        }

        // Move to the starting position, completing a lap.
        public static void GotToFirstPlace()
        {
            int move = 40 - CurrentPlayer.Position;
            CurrentPlayer.MovePlayer(move);
        }

        // Calcuclates the amount of properties owned by the player:
        public int PlayerTotalOfProperties()
        {
            int totalOfProperties = 0;

            foreach (var spaceModel in SpaceViewModel.spaceModels.Values)
            {
                if (spaceModel is PropertyModel property && property.Owner == this)
                {
                    totalOfProperties++;
                }
            }

            return totalOfProperties;
        }

        // If a player cannot afford something (their balance would become negative) they lose the game and are removed from the board:
        public void FileBankruptcy(Grid boardGrid, MainWindow board)
        {
            // Adjust each pvm's instance number
            foreach (PlayerViewModel _pvm in Players)
                if (_pvm.instanceNumber > CurrentPlayer.instanceNumber)
                    _pvm.instanceNumber--;

            // Remove label from the board and remove pvm and pm instance from lists
            boardGrid.Children.Remove(board.LblPlayers[CurrentPlayer.instanceNumber]);
            Players.Remove(this);
            PlayerModels.RemoveAt(instanceNumber);

            // Display bankruptcy screen for that player
            if(Players.Count > 1)
            {
                PlayerWentBankrupt bankrupt = new PlayerWentBankrupt(CurrentPlayer);
                bankrupt.Show();
            }

            // If there is only one player left, show victory screen.
            if (Players.Count == 1)
            {
                Victory _v = new Victory(board);
                _v.Show();
            }
        }
    }
}
