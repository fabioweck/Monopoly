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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.DieView dieView = new View.DieView(PlayerViewModel.CurrentPlayer);
            dieView.Show();
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
                myLabel.Background = Brushes.Aqua;

                //-----------------------------------------------------

                //Label label = new Label();

                //label.Content = Players[i].Name;
                //Grid.SetRow(label, Players[i].Row);
                //Grid.SetColumn(label, Players[i].Column);
                

                LblPlayers.Add(myLabel);
                BoardGrid.Children.Add(myLabel);

            };
        }
    }
}

