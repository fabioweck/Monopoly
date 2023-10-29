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
        public List<PlayerViewModel> Players = new List<PlayerViewModel>();      
        public List<Label> LblPlayers = new List<Label>();

        public MainWindow()
        {
            InitializeComponent();
            //Player1 = new PlayerViewModel();
            //lblPlayer1.DataContext = CurrentPlayer;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //View.DieView dieView = new View.DieView(CurrentPlayer);
            //dieView.Show();
            //Label dice = new Label();
            //dice.Content = "Die 1: 3, Die 2: 4. Move 7 places.";
            //dice.FontSize = 20;
            //dice.Background = Brushes.White;
            //Grid.SetRow(dice, 5);
            //Grid.SetColumn(dice, 5);
            //Grid.SetColumnSpan(dice, 5);
            //BoardGrid.Children.Add(dice);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //CurrentPlayer.MovePlayer(3, 4);
        }

        private void HowManyPlayers(object sender, EventArgs e)
        {
            PlayerCountQuestion playerCount = new PlayerCountQuestion();
            playerCount.Show();

            for(int i = 0; i < LblPlayers.Count; i++)
            {
                Players.Add(new PlayerViewModel());

                //-----------------------------------------------------
                Label myLabel = new Label();

                // Create a source object (in this case, a simple class with a property)
                PlayerViewModel myData = Players[i];

                // Create a Binding object and set the path to the property you want to bind
                Binding bindingRow = new Binding(Players[i].Row.ToString());
                Binding bindingColumn = new Binding(Players[i].Column.ToString());
                Binding bindingName = new Binding(Players[i].Name.ToString());

                // Set the source of the binding to your data object
                bindingRow.Source = myData;
                bindingColumn.Source = myData;
                bindingName.Source = myData;

                // Apply the binding to the Label's Content property
                myLabel.SetBinding(Label.ContentProperty, bindingRow);
                myLabel.SetBinding(Label.ContentProperty, bindingColumn);
                //-----------------------------------------------------

                Binding myBinding = new Binding();
                myBinding.Source = Players[0];

                Label label = new Label();

                label.Content = Players[i].Name;
                Grid.SetRow(label, Players[i].Row);
                Grid.SetColumn(label, Players[i].Column);
                

                LblPlayers.Add(label);
                BoardGrid.Children.Add(label);

            };
        }
    }
}

