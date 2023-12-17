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
        public delegate void GetOption(string result);

        public bool YesOrNo;
        public GetOption SetOption;

        public MessageBoxView(string title, string content)
        {
            InitializeComponent();

            txtTitle.Text = title;
            txtContent.Text = content;

            btnYes.Visibility = Visibility.Hidden;
            btnNo.Visibility = Visibility.Hidden;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        public MessageBoxView(string title, string content, GetOption setOption, bool yesOrNo = false)
        {
            InitializeComponent();

            txtTitle.Text = title;
            txtContent.Text = content;
            YesOrNo = yesOrNo;
            SetOption = setOption;

            if (YesOrNo)
            {
                btnOk.Visibility = Visibility.Hidden;
                btnYes.Focus();
            }
            else
            {
                btnYes.Visibility = Visibility.Hidden;
                btnNo.Visibility = Visibility.Hidden;
                btnOk.Focus();
            }

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
        }


        //Identify a button was clicked and return its content
        public void OptionClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string option = button.Content.ToString();
            SetOption(option);
            this.Close();
        }
    }
}
