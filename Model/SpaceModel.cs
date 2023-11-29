using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Monopoly.Model
{
    public class SpaceModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } = "Neutral";
        public string Image = null;
        private string v1;
        private string v2;
        private string v3;
        private string v4;
        private int v5;
        private int v6;
        private int v7;
        private int v8;
        private string img;

        public ImageSource ImgSrc
        {
            get
            {
                return new BitmapImage(new Uri($"pack://application:,,,/Images/{Image}.png"));
            }
        }

            
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; } = 2;
        public int ColumnSpan { get; set; } = 2;
        public int SpaceNumber { get; set; }



        public SpaceModel(string name, string description, string type, string img, int row, int column, int rowSpan, int columnSpan, int spaceNumber)
        {
            Name = name;
            Description = description;
            Type = type;
            Image = img;
            Row = row;
            Column = column;
            RowSpan = rowSpan;
            ColumnSpan = columnSpan;
            SpaceNumber = spaceNumber;
        }

        public SpaceModel(string name, string description, string img, int row, int column, int rowSpan, int columnSpan, int spaceNumber)
        {
            Name = name;
            Description = description;
            Image = img;
            Row = row;
            Column = column;
            RowSpan = rowSpan;
            ColumnSpan = columnSpan;
            SpaceNumber = spaceNumber;
        }

        public SpaceModel(string name, string description, string img, int row, int column, int rowSpan, int columnSpan)
        {
            Name = name;
            Description = description;
            this.img = img;
            Row = row;
            Column = column;
            RowSpan = rowSpan;
            ColumnSpan = columnSpan;
        }

        public static void ReadCard(SpaceModel sm)
        {
            if (sm.Type == "Card")
            {
                MessageBox.Show("Obtained a card");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
