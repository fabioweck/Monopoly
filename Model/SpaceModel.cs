using Monopoly.ViewModel;
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

        //Holds the image path of a space
        public ImageSource ImgSrc
        {
            get
            {
                return new BitmapImage(new Uri($"pack://application:,,,/Images/{Image}.png"));
            }
        }

        //Holds the properties used to display on board
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
            Image = img;
            Row = row;
            Column = column;
            RowSpan = rowSpan;
            ColumnSpan = columnSpan;
        }
    }
}
