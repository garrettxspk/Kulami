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
            ImageBrush backButtonib = new ImageBrush();

            backButtonib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/backButton.png", UriKind.Absolute));
            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            localBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/LocalButton.png", UriKind.Absolute));
            onlineBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/OnlineButton.png", UriKind.Absolute));

            SelectionBackground.Background = backgrnd;
            LocalModeButton.Background = localBtnBackgrnd;
            OnlineModeButton.Background = onlineBtnBackgrnd;
            BackButton.Background = backButtonib;

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
            Switcher.Switch(new WaitingForConnectionPage());
            
            long id = Math.Abs(networkPeer.listener.getLocalId());
            long waitTime = id / 1000000000000037;
            waitTime = waitTime / 10;
            int waitTimeInMSecs = (int)waitTime;
            Console.WriteLine(waitTimeInMSecs.ToString());
            await Task.Delay(waitTimeInMSecs);
            networkPeer.Start();
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

            Switcher.Switch(new LANGamePage(networkPeer, networkingBoardNum, meFirst));
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
