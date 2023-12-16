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

        public string CommunityCard
        {
            get { return PlayerModels[instanceNumber].CommunityCard; }

            set { PlayerModels[instanceNumber].CommunityCard = value; OnPropertyChanged(nameof(CommunityCard)); }
        }

        public string ChanceCard
        {
            get { return PlayerModels[instanceNumber].ChanceCard; }

            set { PlayerModels[instanceNumber].ChanceCard = value; OnPropertyChanged(nameof(ChanceCard)); }
        }

        public bool IsInJail
        {
            get { return PlayerModels[instanceNumber].isInJail; }

            set { PlayerModels[instanceNumber].isInJail = value; }
        }

        public int AttemptsToGetOutOfJail
        {
            get { return PlayerModels[instanceNumber].attemptsToGetOutOfJail; }

            set { PlayerModels[instanceNumber].attemptsToGetOutOfJail = value; }
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
            SpaceViewModel.propertyOwners.Add(this, new List<PropertyModel>());
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
            //Jail position on the board
            int jailPosition = 10;

            //Number of positions to move to go to the jail
            int move = jailPosition - CurrentPlayer.Position;

            //Check if the player is already in jail or needs to move to jail
            if (move != 0)
            {
                //Player needs to move to jail
                HandleMoveToJail(move);
            }
            else
            {
                //Player is already in jail and if they have 'Get Out of Jail Free' card free card, ask if they want to use it
                if (HasGetOutOfJailCard(CurrentPlayer.ChanceCard) || HasGetOutOfJailCard(CurrentPlayer.CommunityCard))
                {
                    AskToUseJailCard();
                }
            }
        }

        private static void HandleMoveToJail(int move)
        {
            //Check if the player has a 'Get Out of Jail Free' card
            if (HasGetOutOfJailCard(CurrentPlayer.ChanceCard) || HasGetOutOfJailCard(CurrentPlayer.CommunityCard))
            {
                AskToUseJailCard();
            }
            else
            {
                CurrentPlayer.IsInJail = true;
                move = -CurrentPlayer.Position + 10;
                CurrentPlayer.MovePlayer(move);
            }
        }

        public static void AskToUseJailCard()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to use the 'Get Out of Jail Free' card?", "Jail", MessageBoxButton.YesNo);

            if(!CurrentPlayer.IsInJail)
            {
                if (result == MessageBoxResult.Yes)
                {
                    UseJailCard();
                }
                else
                {
                    MovePlayerToJail();
                }
            }
            else
            {
                if (result == MessageBoxResult.Yes)
                {
                    UseJailCard();
                }
                else
                {
                    return;
                }
            }
        }

        private static void UseJailCard()
        {
            if (HasGetOutOfJailCard(CurrentPlayer.ChanceCard))
            {
                CurrentPlayer.ChanceCard = "No chance card";
                CardViewModel.AddChanceCard("Get Out of Jail Free", "JailFree", 0);
                CurrentPlayer.IsInJail = false;
            }
            else
            {
                CurrentPlayer.CommunityCard = "No community card";
                CardViewModel.AddCommunityCard("Get Out of Jail Free", "JailFree", 0);
                CurrentPlayer.IsInJail = false;
            }
        }

        private static void MovePlayerToJail()
        {
            //Number of positions to move the player to go to the jail
            int jailPosition = 10;
            int move = jailPosition - CurrentPlayer.Position;
            CurrentPlayer.IsInJail = true;

            CurrentPlayer.MovePlayer(move);
        }

        public static bool HasGetOutOfJailCard(string card)
        {
            return card == "Jail Free Card";
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
            MainWindow.isBankrupt = true;
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
                bankrupt.ShowDialog();
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
