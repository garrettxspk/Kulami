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
    /// Interaction logic for WaitingForConnectionPage.xaml
    /// </summary>
    public partial class WaitingForConnectionPage : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        LidgrenKulamiPeer.KulamiPeer networkPeer;
        public WaitingForConnectionPage(LidgrenKulamiPeer.KulamiPeer peer)
        {
            InitializeComponent();
            networkPeer = peer;
            ImageBrush backgrnd = new ImageBrush();
            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            ImageBrush backButtonib = new ImageBrush();
            backButtonib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/backButton.png", UriKind.Absolute));
            Background.Background = backgrnd;
            BackButton.Background = backButtonib;

            //Task.Delay(6000);
            
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            networkPeer.killPeer();
            networkPeer = null;
            MultiplayerMode.ShouldBreakOut = true;
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

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("key down");
            if (e.Key == Key.Escape)
                Switcher.Switch(new MainPage());
        }
    }
}
