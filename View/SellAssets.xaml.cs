using Monopoly.Model;
using Monopoly.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Monopoly.View
{
    /// <summary>
    /// Interaction logic for SellAssets.xaml
    /// </summary>
    public partial class SellAssets : Window, INotifyPropertyChanged
    {
        static int _ind = 0;
        public List<PropertyModel> properties;
        private ImageSource _imgSrc;
        public ImageSource ImageSource
        {
            get => _imgSrc;
            set
            {
                _imgSrc = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SellAssets(PlayerViewModel currentPlayer, Dictionary<PlayerViewModel, List<PropertyModel>> propertyOwners)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            pName.Content = currentPlayer.Name;
            properties = propertyOwners[currentPlayer];
            Houses.Content = $"Houses: {properties[0].HousesBuilt}";

            // reset index to display the first property of the list
            _ind = 0;

            // set the image source of the displayed card
            ImageSource = properties[_ind].ImgSrc;
            CardPicture.Source = ImageSource;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            if (properties.Count > 0)
            {
                if (_ind < properties.Count - 1) _ind++;
                else if (_ind == properties.Count - 1) _ind = 0;

                Update();
            }
        }

        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            if (properties.Count > 0)
            {
                if (_ind > 0) _ind--;
                else if (_ind == 0) _ind = properties.Count - 1;
                Update();
            }
        }

        private void btnSellHouse_Click(object sender, RoutedEventArgs e)
        {

            if (properties[_ind].HousesBuilt > 0)
            {
                SpaceViewModel.SellHouse(properties[_ind], PlayerViewModel.CurrentPlayer);
                if (properties.Count == 0) Close();
                Update();
            }
            else if (properties.Count == 0) Close();
        }

        private void btnSellProperty_Click(object sender, RoutedEventArgs e)
        {
            if (properties[_ind].HousesBuilt == 0)
            {
                SpaceViewModel.SellProperty(properties[_ind], PlayerViewModel.CurrentPlayer);
                _ind = 0;
                Update();
            }
            if (properties.Count == 0) Close();
        }

        private void Update()
        {
            if (properties.Count > 0)
            {
                ImageSource = properties[_ind].ImgSrc;
                CardPicture.Source = ImageSource;
                Houses.Content = $"Houses: {properties[_ind].HousesBuilt}";
            }
            else
            {
                ImageSource = null;
                Houses.Content = $"Houses: 0";
            }

            foreach (TextBox textBox in MainWindow.txtBoxPanelPlayers)
            {
                if (textBox.Name == PlayerViewModel.CurrentPlayer.Name)
                {
                    SpaceViewModel.UpdatePlayerPanel(textBox, PlayerViewModel.CurrentPlayer);
                }
            }
        }
    }
}
