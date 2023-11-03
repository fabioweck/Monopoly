using Monopoly.Model;
using Monopoly.View;
using Monopoly.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static int NumberOfPlayers = 0;
        public List<Label> LblPlayers = new List<Label>();

        public MainWindow()
        {
            InitializeComponent();
            SpaceViewModel.PopulateBoard();
            //CardView = new CardViewModel();
            //Card1.DataContext = CardView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.DieView dieView = new View.DieView();
            dieView.ShowDialog();
            PlayerViewModel.CurrentPlayer.MovePlayer(dieView.Roll);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //CurrentPlayer.MovePlayer(3, 4);
        }

        private void HowManyPlayers(object sender, EventArgs e)
        {
            PlayerCountQuestion playerCount = new PlayerCountQuestion();
            playerCount.ShowDialog();

            for(int i = 0; i < NumberOfPlayers; i++)
            {
                new PlayerViewModel();

                //-----------------------------------------------------
                Label myLabel = new Label();

                // Create a source object (in this case, a simple class with a property)
                PlayerViewModel myData = PlayerViewModel.Players[i];

                // Create a Binding object and set the path to the property you want to bind
                Binding bindingRow = new Binding("Row");
                Binding bindingColumn = new Binding("Column");
                Binding bindingName = new Binding("Name");

                // Set the source of the binding to your data object
                bindingRow.Source = myData;
                bindingColumn.Source = myData;
                bindingName.Source = myData;

                // Apply the binding to the Label's Content property
                myLabel.SetBinding(Label.ContentProperty, bindingName);
                myLabel.SetBinding(Grid.RowProperty, bindingRow);
                myLabel.SetBinding(Grid.ColumnProperty, bindingColumn);


                myLabel.Background = new SolidColorBrush(Color.FromRgb(200,200,250));


                //-----------------------------------------------------

                // Iterate through our dictionary and create data binding to properly display on View:

                

                //Label label = new Label();

                //label.Content = Players[i].Name;
                //Grid.SetRow(label, Players[i].Row);
                //Grid.SetColumn(label, Players[i].Column);
                

                LblPlayers.Add(myLabel);
                BoardGrid.Children.Add(myLabel);

            }

            foreach (var space in SpaceViewModel.spaceModels)
            {
                // Create a new control:
                Image image = new Image();

                // Create the data source:
                SpaceModel sm = space.Value;

                // Create the Binding objects to set the path to the properties:
                Binding bindRow = new Binding("Row");
                Binding bindCol = new Binding("Column");
                Binding bindRowSpan = new Binding("RowSpan");
                Binding bindColSpan = new Binding("ColumnSpan");
                Binding bindImg = new Binding("ImgSrc");

                // Set the source to the data source previously defined:
                bindRow.Source = sm;
                bindCol.Source = sm;
                bindRowSpan.Source = sm;
                bindColSpan.Source = sm;
                bindImg.Source = sm;

                // Apply the binding to the controls' properties:
                image.SetBinding(Grid.RowProperty, bindRow);
                image.SetBinding(Grid.ColumnProperty, bindCol);
                image.SetBinding(Grid.RowSpanProperty, bindRowSpan);
                image.SetBinding(Grid.ColumnSpanProperty, bindColSpan);
                image.SetBinding(Image.SourceProperty, bindImg);
                BoardGrid.Children.Add(image);

            }
        }

        private void DrawSpaces()
        {
            foreach(var space in SpaceViewModel.spaceModels)
            {
                
            }
        }
    }
}

