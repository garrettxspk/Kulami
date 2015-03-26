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
        private SoundEffectsPlayer soundEffectPlayer = new SoundEffectsPlayer();
        public MultiplayerMode()
        {
            InitializeComponent();
            ImageBrush backgrnd = new ImageBrush();
            ImageBrush localBtnBackgrnd = new ImageBrush();
            ImageBrush onlineBtnBackgrnd = new ImageBrush();

            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            localBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/LocalButton.png", UriKind.Absolute));
            onlineBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/OnlineButton.png", UriKind.Absolute));

            SelectionBackground.Background = backgrnd;
            LocalModeButton.Background = localBtnBackgrnd;
            OnlineModeButton.Background = onlineBtnBackgrnd;
        }

        private void LocalModeButton_Click(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
            Switcher.Switch(new LocalGamePage());
        }

        private async void OnlineModeButton_Click(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
            LidgrenKulamiPeer.KulamiPeer networkPeer = new LidgrenKulamiPeer.KulamiPeer();
            while (networkPeer.listener.connection == null)
            {
                await Task.Delay(1000);
                Console.WriteLine("Waiting for connection");
            }

            int networkingBoardNum = 0;
            Random rnd = new Random();
            int myRandomBoardNum = rnd.Next(1, 8);

            networkPeer.sendMove(myRandomBoardNum.ToString());
            string move = networkPeer.getMove();
            while (move == null)
            {
                await Task.Delay(1000);
                move = networkPeer.getMove();
            }
            int opponentRandomBoardNum = Convert.ToInt32(move);

            networkingBoardNum = (myRandomBoardNum + opponentRandomBoardNum) / 2;
            while (myRandomBoardNum == opponentRandomBoardNum)
            {
                myRandomBoardNum = rnd.Next(1, 8);
                networkPeer.sendMove(myRandomBoardNum.ToString());
                move = networkPeer.getMove();
                while (move == null)
                {
                    await Task.Delay(1000);
                    move = networkPeer.getMove();
                }
                opponentRandomBoardNum = Convert.ToInt32(move);
            }

            bool meFirst;
            if (myRandomBoardNum > opponentRandomBoardNum)
                meFirst = true;
            else
                meFirst = false;

            Switcher.Switch(new LANGamePage(networkPeer));
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void LocalModeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush localBtnBackgrnd = new ImageBrush();
            localBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/LocalButtonOn.png", UriKind.Absolute));
            LocalModeButton.Background = localBtnBackgrnd;

        }

        private void LocalModeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush localBtnBackgrnd = new ImageBrush();
            localBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/LocalButton.png", UriKind.Absolute));
            LocalModeButton.Background = localBtnBackgrnd;

        }
        private void OnlineModeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush onlineBtnBackgrnd = new ImageBrush();
            onlineBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/OnlineButtonOn.png", UriKind.Absolute));
            OnlineModeButton.Background = onlineBtnBackgrnd;

        }

        private void OnlineModeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush onlineBtnBackgrnd = new ImageBrush();
            onlineBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/OnlineButton.png", UriKind.Absolute));
            OnlineModeButton.Background = onlineBtnBackgrnd;

        }
    }
}
