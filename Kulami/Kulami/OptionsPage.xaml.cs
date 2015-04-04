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
    /// Interaction logic for OptionsPage.xaml
    /// </summary>
    public partial class OptionsPage : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public OptionsPage()
        {
            InitializeComponent();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            OptionsBackground.Background = ib;

            ImageBrush bb = new ImageBrush();
            bb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/backButton.png", UriKind.Absolute));
            BackButton.Background = bb;
        }
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainPage());
        }

        private void BackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush bb = new ImageBrush();
            bb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/backButtonOn.png", UriKind.Absolute));
            BackButton.Background = bb;
        }

        private void BackButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush bb = new ImageBrush();
            bb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/backButton.png", UriKind.Absolute));
            BackButton.Background = bb;
        }
    }
}
