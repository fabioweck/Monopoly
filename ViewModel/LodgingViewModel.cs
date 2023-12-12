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
            int offSetCol = 0;
            int offSetRow = 0;
            int offSetColSecondary = 0;
            int offSetRowSecondary = 0;

            // Set the Grid row and column offset
            SetGridOffsets(property, out offSetCol, out offSetRow, out offSetColSecondary, out offSetRowSecondary);

            LodgingModel houseModel = new LodgingModel("house", property.Row + offSetRow + offSetRowSecondary, property.Column + offSetCol + offSetColSecondary, 1, 1);
            LodgingModel doubleHouseModel = new LodgingModel("house_double", property.Row + offSetRow + offSetRowSecondary, property.Column + offSetCol + offSetColSecondary, 1, 1);
            LodgingModel hotelModel = new LodgingModel("hotel", property.Row + offSetRow + offSetRowSecondary, property.Column + offSetCol + offSetColSecondary, 1, 2);

            // Set the data model:
            LodgingModel lodgingModel = null;

            // Check if the property can have a house/hotel 
            if (property.HousesBuilt == 0)
            {
                lodgingModel = houseModel;
                CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);
            }
            else if (property.HousesBuilt == 1)
            {
                lodgingModel = houseModel;
                CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);
            }
            else if (property.HousesBuilt == 2)
            {
                lodgingModel = houseModel;
                //RemoveExistingImages(property);
                CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);
            }
            else if (property.HousesBuilt == 3)
            {
                lodgingModel = doubleHouseModel;
                //RemoveExistingImages(property);
                CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);
            }
            else if (property.HousesBuilt == 4)
            {
                lodgingModel = hotelModel;
                //RemoveExistingImages(property);
                CreateAndBindLodgingImage(property, lodgingImage, lodgingModel, offSetRow, offSetCol);
            }

            // Create the Binding objects to set the path to the properties:
            boardGrid.Children.Add(lodgingImage);
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

            // Apply the binding to the controls' properties:
            lodgingImage.SetBinding(Grid.RowProperty, CreateBinding("Row", lodgingModel, offSetRow));
            lodgingImage.SetBinding(Grid.ColumnProperty, CreateBinding("Column", lodgingModel, offSetCol));
            lodgingImage.SetBinding(Grid.RowSpanProperty, CreateBinding("RowSpan", lodgingModel));
            lodgingImage.SetBinding(Grid.ColumnSpanProperty, CreateBinding("ColumnSpan", lodgingModel));
            lodgingImage.SetBinding(Image.SourceProperty, CreateBinding("ImgSrc", lodgingModel));
        }

        //private void RemoveExistingImages(PropertyModel property)
        //{
        //    // Remove existing images
        //    var imagesToRemove = BoardGrid.Children.OfType<Image>().Where(child => GetPropertyFromImage(child)?.Equals(property) == true).ToList();

        //    foreach (var imageToRemove in imagesToRemove)
        //    {
        //        BoardGrid.Children.Remove(imageToRemove);
        //    }
        //}

        private void SetGridOffsets(PropertyModel property, out int offSetCol, out int offSetRow, out int offSetColSecondary, out int offSetRowSecondary)
        {
            offSetCol = 0;
            offSetRow = 0;
            offSetColSecondary = 0;
            offSetRowSecondary = 0;

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

            }
            //Bottom
            else if (property.Row >= 22 && property.Row <= 24 && property.Column >= 0 && property.Column <= 21)
            {
                offSetCol = 1;
                offSetRow = 0;
                if (property.HousesBuilt == 1 || property.HousesBuilt == 3)
                {
                    offSetColSecondary = 0;
                    offSetRowSecondary = 0;
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
                    offSetRowSecondary = 0;
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

        private Binding CreateBinding(string propertyPath, object source, int offset = 0)
        {
            Binding binding = new Binding(propertyPath);
            binding.Source = source;
            binding.Path = new PropertyPath(propertyPath);
            if (offset != 0)
            {
                //binding.Converter = new OffsetConverter(offset);
            }
            return binding;
        }
    }
}
