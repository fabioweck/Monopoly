using System;
using System.Collections.Generic;
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
    public class LodgingModel
    {

        public string Image = null;

        //Holds the image path of a lodging
        public ImageSource ImgSrc
        {
            get
            {
                return new BitmapImage(new Uri($"pack://application:,,,/Images/{Image}.png"));
            }
        }
        
        //Properties used to display it on the board/grid
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; } = 2;
        public int ColumnSpan { get; set; } = 2;
        public int SerialNumber { get; set; }

        public LodgingModel(string img, int row, int column, int rowSpan, int columnSpan, int serialNumber)
        {
            Image = img;
            Row = row;
            Column = column;
            RowSpan = rowSpan;
            ColumnSpan = columnSpan;
            SerialNumber = serialNumber;
        }
    }
}
