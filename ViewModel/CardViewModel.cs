using Monopoly.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.ViewModel
{
    public class CardViewModel : INotifyPropertyChanged
    {
        public CardModel Card { get; set; }

        public string Description
        {
            get
            {
                return Card.Description;
            }
        }

        public int Column
        {
            get
            {
                return Card.Position.X;
            }
            set
            {
                Card.Position = new Point(value, Card.Position.Y);
                OnPropertyChanged(nameof(Column));
            }
        }

        public int Row
        {
            get
            {
                return Card.Position.Y;
            }

            set
            {
                Card.Position = new Point(Card.Position.X, value);
                OnPropertyChanged(nameof(Row));
            }
        }

        public CardViewModel()
        {
            Card = new CardModel("Go to jail!", 3, 3);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MoveCard(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }
}
