using Monopoly.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace Monopoly.ViewModel
{
    public class LodgingViewModel
    {
        public static Dictionary<int, LodgingModel> LodgingModel = new Dictionary<int, LodgingModel>();
        
        // Store property and lodging pairs
        public List<Tuple<PropertyModel, LodgingModel, Image>> PropertyLodgingPairs = new List<Tuple<PropertyModel, LodgingModel, Image>>();

        public LodgingViewModel()
        {

        }

        public void AddLodgingToBoard(Grid boardGrid, PropertyModel property)
        {
            // Check if the property already has a hotel, if so, do nothing
            if (property.HousesBuilt >= 5)
            {
                return;
            }

            // Create a new control:
            Image lodgingImage = new Image();


            // Set the data source: (offSet -> first house / offSetSecondary -> second house)
            int offSetCol;
            int offSetRow;
            int offSetColSecondary;
            int offSetRowSecondary;
            int rowSpan;
            int columnSpan;

            // Set the Grid row and column offset
            SetGridOffsets(property, out offSetCol, out offSetRow, out offSetColSecondary, out offSetRowSecondary, out rowSpan, out columnSpan);

            LodgingModel houseModel = new LodgingModel("house", property.Row + offSetRow + offSetRowSecondary, property.Column + offSetCol + offSetColSecondary, rowSpan, columnSpan, property.HousesBuilt + 1);
            LodgingModel doubleHouseModel = new LodgingModel("house_double", property.Row + offSetRow + offSetRowSecondary, property.Column + offSetCol + offSetColSecondary, rowSpan, columnSpan, property.HousesBuilt + 1);
            LodgingModel hotelModel = new LodgingModel("hotel", property.Row + offSetRow + offSetRowSecondary, property.Column + offSetCol + offSetColSecondary, rowSpan, columnSpan, property.HousesBuilt + 1);

            // Set the data model:
            LodgingModel lodgingModel = null;

            // Check if the property can have a house/hotel 
            if (property.HousesBuilt == 0)
            {
                lodgingModel = houseModel;
            }
            else if (property.HousesBuilt == 1)
            {
                lodgingModel = houseModel;
            }
            else if (property.HousesBuilt == 2)
            {
                LodgingRemoval(boardGrid, 1, property.Name);
                lodgingModel = doubleHouseModel;
            }
            else if (property.HousesBuilt == 3)
            {
                LodgingRemoval(boardGrid, 2, property.Name);
                lodgingModel = doubleHouseModel;
            }
            else if (property.HousesBuilt == 4)
            {
                LodgingRemoval(boardGrid, 3, property.Name);
                LodgingRemoval(boardGrid, 4, property.Name);
                lodgingModel = hotelModel;
            }

            // Set the serial number
            lodgingModel.SerialNumber = property.HousesBuilt + 1;
            CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);

            // Add property and lodging pair to the list
            PropertyLodgingPairs.Add(new Tuple<PropertyModel, LodgingModel, Image>(property, lodgingModel, lodgingImage));

            // Create the Binding objects to set the path to the properties:
            boardGrid.Children.Add(lodgingImage);

            //DisplayAllPropertiesAndLodgingModels();
        }

        private void CreateAndBindLodgingImage(PropertyModel property, Image lodgingImage, LodgingModel lodgingModel, int offSetRow, int offSetCol)
        {
            if (lodgingModel == null)
            {
                return;
            }

            // Update the property's house count
            property.HousesBuilt++;

            // Create the Binding objects to set the path to the properties:
            BindLodgingImage(lodgingImage, lodgingModel);

            // Store the lodgingModel in the dictionary for future reference
            LodgingModel[lodgingModel.GetHashCode()] = lodgingModel;
        }


        private void UnbindLodgingImage(Grid boardGrid, Image lodgingImage)
        {
            // Clear the bindings for the specified Image control
            lodgingImage.ClearValue(Grid.RowProperty);
            lodgingImage.ClearValue(Grid.ColumnProperty);
            lodgingImage.ClearValue(Grid.RowSpanProperty);
            lodgingImage.ClearValue(Grid.ColumnSpanProperty);
            lodgingImage.ClearValue(Image.SourceProperty);

            // Remove the Image control from the Grid
            if (boardGrid.Children.Contains(lodgingImage))
            {
                boardGrid.Children.Remove(lodgingImage);
            }
        }

        private void SetGridOffsets(PropertyModel property, out int offSetCol, out int offSetRow, out int offSetColSecondary, out int offSetRowSecondary, out int rowSpan, out int columnSpan)
        {
            offSetCol = 0;
            offSetRow = 0;
            offSetColSecondary = 0;
            offSetRowSecondary = 0;
            rowSpan = 1;
            columnSpan = 1;

            //Top
            if (property.Row >= 0 && property.Row <= 3 && property.Column >= 0 && property.Column <= 21)
            {
                offSetCol = 0;
                offSetRow = 2;
                if (property.HousesBuilt == 1 || property.HousesBuilt == 3)
                {
                    offSetColSecondary = 1;
                    offSetRowSecondary = 0;
                }
                if (property.HousesBuilt == 4) // hotel
                {
                    offSetColSecondary = 0;
                    offSetRowSecondary = 0;
                    rowSpan = 1;
                    columnSpan = 2;
                }
            }
            //Right
            else if (property.Row >= 4 && property.Row <= 21 && property.Column >= 22 && property.Column <= 24)
            {
                offSetCol = 0;
                offSetRow = 0;
                if (property.HousesBuilt == 1 || property.HousesBuilt == 3)
                {
                    offSetColSecondary = 0;
                    offSetRowSecondary = 1;
                }
                if (property.HousesBuilt == 4) // hotel
                {
                    offSetColSecondary = 0;
                    offSetRowSecondary = 0;
                    rowSpan = 2;
                    columnSpan = 1;
                }

            }
            //Bottom
            else if (property.Row >= 22 && property.Row <= 24 && property.Column >= 0 && property.Column <= 21)
            {
                offSetCol = 1;
                offSetRow = 0;
                if (property.HousesBuilt == 1 || property.HousesBuilt == 3)
                {
                    offSetColSecondary = -1;
                    offSetRowSecondary = 0;
                }
                if (property.HousesBuilt == 4) // hotel
                {
                    offSetColSecondary = -1;
                    offSetRowSecondary = 0;
                    //rowSpan = 1;
                    columnSpan = 2;
                }
            }
            //Left
            else if (property.Row >= 0 && property.Row <= 21 && property.Column >= 0 && property.Column <= 3)
            {
                offSetCol = 2;
                offSetRow = 1;
                if (property.HousesBuilt == 1 || property.HousesBuilt == 3)
                {
                    offSetColSecondary = 0;
                    offSetRowSecondary = -1;
                }
                if (property.HousesBuilt == 4) // hotel
                {
                    offSetColSecondary = 0;
                    offSetRowSecondary = -1;
                    rowSpan = 2;
                    columnSpan = 1;
                }
            }
        }

        private void BindLodgingImage(Image lodgingImage, LodgingModel lodgingModel)
        {
            // Create the Binding objects to set the path to the properties:
            lodgingImage.SetBinding(Grid.RowProperty, CreateBinding("Row", lodgingModel));
            lodgingImage.SetBinding(Grid.ColumnProperty, CreateBinding("Column", lodgingModel));
            lodgingImage.SetBinding(Grid.RowSpanProperty, CreateBinding("RowSpan", lodgingModel));
            lodgingImage.SetBinding(Grid.ColumnSpanProperty, CreateBinding("ColumnSpan", lodgingModel));
            lodgingImage.SetBinding(Image.SourceProperty, CreateBinding("ImgSrc", lodgingModel));
        }

        private Binding CreateBinding(string propertyPath, object source)
        {
            Binding binding = new Binding(propertyPath);
            binding.Source = source;
            binding.Path = new PropertyPath(propertyPath);

            return binding;
        }

        private void LodgingRemoval(Grid boardGrid, int serialNumber, string propertyName)
        {
            Tuple<PropertyModel, LodgingModel, Image> lodgingTuple = GetLodgingBySerialNumberAndPropertyName(serialNumber, propertyName);
            
            if(lodgingTuple != null)
            {
                UnbindLodgingImage(boardGrid, lodgingTuple.Item3);
                boardGrid.Children.Remove(lodgingTuple.Item3);
                PropertyLodgingPairs.Remove(lodgingTuple);
            }
        }

        //DEBUG TUPLE
        public void DisplayAllPropertiesAndLodgingModels()
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("### new list ###");
            foreach (var pair in PropertyLodgingPairs)
            {
                PropertyModel property = pair.Item1;
                LodgingModel lodgingModel = pair.Item2;
                Image lodgingImage = pair.Item3;

                Console.WriteLine($"Property: {property.Name}, Row: {property.Row}, Column: {property.Column}, House B: {property.HousesBuilt}, HN: {property}");
                Console.WriteLine($"Row: {lodgingModel.Row}, Column: {lodgingModel.Column}, lodgingImage: {lodgingImage}, SN: {lodgingModel.SerialNumber}");
                Console.WriteLine("----------------------------------------");
            }
        }

        public Tuple<PropertyModel, LodgingModel, Image> GetLodgingBySerialNumberAndPropertyName(int serialNumber, string propertyName)
        {
            return PropertyLodgingPairs.FirstOrDefault(pair => pair.Item2.SerialNumber == serialNumber && pair.Item1.Name == propertyName);
        }



    }
}
