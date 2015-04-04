using System;
using System.Collections.Generic;
using System.IO;
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

namespace Kulami
{
    /// <summary>
    /// Interaction logic for NoConnectionsFoundPage.xaml
    /// </summary>
    public partial class NoConnectionsFoundPage : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public NoConnectionsFoundPage()
        {
            InitializeComponent();
            ImageBrush backgrnd = new ImageBrush();
            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            Background.Background = backgrnd;

            ImageBrush hb = new ImageBrush();
            hb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButton.png", UriKind.Absolute));
            homeButton.Background = hb;
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainPage());
        }

        private void homeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush hb = new ImageBrush();
            hb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButtonHover.png", UriKind.Absolute));
            homeButton.Background = hb;
        }

        private void homeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush hb = new ImageBrush();
            hb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButton.png", UriKind.Absolute));
            homeButton.Background = hb;
        }
    }
}
