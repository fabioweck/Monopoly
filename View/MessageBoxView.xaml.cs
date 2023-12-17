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
    /// Interaction logic for MessageBoxView.xaml
    /// </summary>
    public partial class MessageBoxView : Window
    {
        //To accept the method which will take the result
        public delegate void GetOption(string result);

        //If the user wans the buttons "Yes" and "No" or only the "Ok" button
        public bool YesOrNo;
        public GetOption SetOption;

        //Simple message box with title and text
        public MessageBoxView(string title, string content)
        {
            InitializeComponent();

            txtTitle.Text = title;
            txtContent.Text = content;

            btnYes.Visibility = Visibility.Hidden;
            btnNo.Visibility = Visibility.Hidden;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        //Overload to open a window with yes and no buttons/ok button and return a response
        public MessageBoxView(string title, string content, GetOption setOption, bool yesOrNo = false)
        {
            InitializeComponent();

            txtTitle.Text = title;
            txtContent.Text = content;
            YesOrNo = yesOrNo;
            SetOption = setOption;

            //Hide the button ok
            if (YesOrNo)
            {
                btnOk.Visibility = Visibility.Hidden;
            }
            //Hide the buttons yes and no
            else
            {
                btnYes.Visibility = Visibility.Hidden;
                btnNo.Visibility = Visibility.Hidden;
            }

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
        }

        //Identify a button was clicked and return its content
        public void OptionClicked(object sender, RoutedEventArgs e)
        {
            //Define the option chose
            Button button = sender as Button;

            //Get the option
            string option = button.Content.ToString();

            //Pass to the delegate to return a response
            SetOption(option);
            this.Close();
        }
    }
}
