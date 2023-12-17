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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monopoly.View
{
    /// <summary>
    /// Interaction logic for PlayerPropertiesPanel.xaml
    /// </summary>
    public partial class PlayerPropertiesPanel : UserControl, INotifyPropertyChanged
    {
        int _ind = 0;
        public List<PropertyModel> properties;
        private ImageSource _imgSrc;
        private static Grid _boardGrid;
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

        public PlayerPropertiesPanel(PlayerViewModel owner, Dictionary<PlayerViewModel, List<PropertyModel>> propertyOwners, Grid boardGrid)
        {
            InitializeComponent();
            properties = propertyOwners[owner];
            if (properties.Count > 0)
                Houses.Content = $"Houses: {properties[0].HousesBuilt}";
            else Houses.Content = "No properties.";

            // reset index to display the first property of the list
            _ind = 0;

            // set the image source of the displayed card
            if (properties.Count > 0)
            {
                ImageSource = properties[_ind].ImgSrc;
                CardPicture.Source = ImageSource;
            }
            _boardGrid = boardGrid;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public void Update()
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
                Houses.Content = $"No properties.";
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
