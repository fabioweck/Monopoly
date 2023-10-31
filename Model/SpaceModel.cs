﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Model
{
    internal class SpaceModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } = "Neutral";
        public Bitmap Image = null;
        public int Row {  get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; } = 2;
        public int ColumnSpan { get; set; } = 2;


        public SpaceModel(string name, string description, string type, Bitmap img, int row, int column, int rowSpan, int columnSpan)
        {
            Name = name;
            Description = description;
            Type = type;
            Image = img;
            Row = row;
            Column = column;
            RowSpan = rowSpan;
            ColumnSpan = columnSpan;
        }

        public SpaceModel(string name, string description, Bitmap img, int row, int column, int rowSpan, int columnSpan)
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