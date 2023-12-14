using Monopoly.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for PlayerCountQuestion.xaml
    /// </summary>
    public partial class PlayerCountQuestion : Window
    {
        public PlayerCountQuestion()
        {
            InitializeComponent();
            
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void NumberOfPlayers(object sender, RoutedEventArgs e)
        {
            Button button;
            
            try
            {
                button = (Button)sender;
                switch (button.Content)
                {
                    case "2":
                        MainWindow.NumberOfPlayers = 2;
                        break;
                    case "3":
                        MainWindow.NumberOfPlayers = 3;
                        break;
                    case "4":
                        MainWindow.NumberOfPlayers = 4;
                        break;
                    default:
                        Application.Current.Shutdown();
                        break;
                }
            }
            catch(NotSupportedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            this.Close();

        }
    }
}
