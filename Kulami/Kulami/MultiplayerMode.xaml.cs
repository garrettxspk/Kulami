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
    /// Interaction logic for MultiplayerMode.xaml
    /// </summary>
    public partial class MultiplayerMode : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public MultiplayerMode()
        {
            InitializeComponent();
            ImageBrush backgrnd = new ImageBrush();
            ImageBrush localBtnBackgrnd = new ImageBrush();
            ImageBrush onlineBtnBackgrnd = new ImageBrush();

            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            localBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/MultiplayerLocalButton.png", UriKind.Absolute));
            onlineBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/MultiplayerOnlineButton.png", UriKind.Absolute));

            SelectionBackground.Background = backgrnd;
            LocalModeButton.Background = localBtnBackgrnd;
            OnlineModeButton.Background = onlineBtnBackgrnd;
        }

        private void LocalModeButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new GamePage(true, GameType.LocalMultiplayer));
        }

        private void OnlineModeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
    }
}
