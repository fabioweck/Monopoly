using Monopoly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.ViewModel
{
    public class LodgingViewModel
    {
        public static Dictionary<int, LodgingModel> LodgingModel = new Dictionary<int, LodgingModel>();

        public LodgingViewModel()
        {

        }

        //public static void PopulateBoard()
        //{

        //    LodgingModel lm1 = new LodgingModel(
        //            "hotel",                                            // Image
        //            3, 4, 1, 2);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(1, lm1);

        //    LodgingModel lm2 = new LodgingModel(
        //            "house",                                            // Image
        //            3, 8, 1, 1);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(2, lm2);

        //    LodgingModel lm3 = new LodgingModel(
        //            "house",                                            // Image
        //            3, 14, 1, 1);                                      // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(3, lm3);

        //    LodgingModel lm4 = new LodgingModel(
        //            "house",                                            // Image
        //            3, 15, 1, 1);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(4, lm4);

        //    LodgingModel lm5 = new LodgingModel(
        //            "house",                                            // Image
        //            3, 18, 1, 1);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(5, lm5);

        //    LodgingModel lm6 = new LodgingModel(
        //            "house_double",                                     // Image
        //            3, 19, 1, 1);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(6, lm6);

        //    LodgingModel lm7 = new LodgingModel(
        //            "house_double",                                     // Image
        //            3, 20, 1, 1);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(7, lm7);

        //    LodgingModel lm8 = new LodgingModel(
        //            "house_double",                                     // Image
        //            3, 21, 1, 1);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(8, lm8);

        //    LodgingModel lm9 = new LodgingModel(
        //            "hotel",                                            // Image
        //            4, 22, 2, 1);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(9, lm9);

        //    LodgingModel lm10 = new LodgingModel(
        //            "house",                                            // Image
        //            8, 22, 1, 1);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(10, lm10);

        //    LodgingModel lm11 = new LodgingModel(
        //            "house",                                            // Image
        //            10, 22, 1, 1);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(11, lm11);

        //    LodgingModel lm12 = new LodgingModel(
        //            "house",                                            // Image
        //            11, 22, 1, 1);                                        // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(12, lm12);

        //    LodgingModel lm13 = new LodgingModel(
        //            "house",                                            // Image
        //            14, 22, 1, 1);                                      // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(13, lm13);

        //    LodgingModel lm14 = new LodgingModel(
        //            "house_double",                                     // Image
        //            15, 22, 1, 1);                                        // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(14, lm14);

        //    LodgingModel lm15 = new LodgingModel(
        //            "house_double",                                     // Image
        //            18, 22, 1, 1);                                       // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(15, lm15);

        //    LodgingModel lm16 = new LodgingModel(
        //            "house_double",                                     // Image
        //            19, 22, 1, 1);                                        // Row, Column, RowSpan, ColumnSpan
        //    LodgingModel.Add(16, lm16);

        //}
    }
}
