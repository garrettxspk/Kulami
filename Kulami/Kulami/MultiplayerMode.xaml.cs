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
using System.Windows.Media.Animation;
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
        private Storyboard myStoryboard;
        public MultiplayerMode()
        {
            InitializeComponent();
            ImageBrush backgrnd = new ImageBrush();
            ImageBrush localBtnBackgrnd = new ImageBrush();
            ImageBrush onlineBtnBackgrnd = new ImageBrush();
            ImageBrush backButtonib = new ImageBrush();
            ImageBrush nextButtonib = new ImageBrush();


            backButtonib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/backButton.png", UriKind.Absolute));
            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            localBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/LocalButton.png", UriKind.Absolute));
            onlineBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/OnlineButton.png", UriKind.Absolute));
            nextButtonib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/nextButton.png", UriKind.Absolute));

            SelectionBackground.Background = backgrnd;
            LocalModeButton.Background = localBtnBackgrnd;
            OnlineModeButton.Background = onlineBtnBackgrnd;
            BackButton.Background = backButtonib;
            NextButton.Background = nextButtonib;

            PlayerNameTextBox.IsEnabled = false;
            NextButton.IsEnabled = false;

            DoubleAnimation FadeIn = new DoubleAnimation();
            FadeIn.From = 0.0;
            FadeIn.To = 1.0;
            FadeIn.Duration = new Duration(TimeSpan.FromSeconds(1));
            FadeIn.AutoReverse = false;

            DoubleAnimation FadeIn2 = new DoubleAnimation();
            FadeIn2.From = 0.0;
            FadeIn2.To = 1.0;
            FadeIn2.Duration = new Duration(TimeSpan.FromSeconds(1));
            FadeIn2.AutoReverse = false;

            DoubleAnimation FadeOut = new DoubleAnimation();
            FadeOut.From = 1.0;
            FadeOut.To = 0.0;
            FadeOut.Duration = new Duration(TimeSpan.FromSeconds(1));
            FadeOut.AutoReverse = false;

            DoubleAnimation FadeOut2 = new DoubleAnimation();
            FadeOut2.From = 1.0;
            FadeOut2.To = 0.0;
            FadeOut2.Duration = new Duration(TimeSpan.FromSeconds(1));
            FadeOut2.AutoReverse = false;

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(FadeIn);
            myStoryboard.Children.Add(FadeIn2);
            myStoryboard.Children.Add(FadeOut);
            myStoryboard.Children.Add(FadeOut2);
            Storyboard.SetTargetName(FadeIn, PlayerNameTextBox.Name);
            Storyboard.SetTargetProperty(FadeIn, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(FadeIn2, NextButton.Name);
            Storyboard.SetTargetProperty(FadeIn2, new PropertyPath(Rectangle.OpacityProperty));

            Storyboard.SetTargetName(FadeOut, OnlineModeButton.Name);
            Storyboard.SetTargetProperty(FadeOut, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(FadeOut2, LocalModeButton.Name);
            Storyboard.SetTargetProperty(FadeOut2, new PropertyPath(Rectangle.OpacityProperty));


        }

        private void LocalModeButton_Click(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
            Switcher.Switch(new LocalGamePage());
        }

        private void OnlineModeButton_Click(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
            PlayerNameTextBox.IsEnabled = true;
            NextButton.IsEnabled = true;
            LocalModeButton.IsEnabled = false;
            OnlineModeButton.IsEnabled = false;
            ModeLabel.Content = "Type in your name";
            myStoryboard.Begin(NextButton);
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

        private async void NextButtonClick(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
            string playerName = (string) NextButton.Content;
            //Switcher.Switch(new ConnectionLostPage());
            Random rnd = new Random();
            Switcher.Switch(new WaitingForConnectionPage());
            //int waitTime = rnd.Next(5, 4001);
            //await Task.Delay(waitTime);
            LidgrenKulamiPeer.KulamiPeer networkPeer = new LidgrenKulamiPeer.KulamiPeer(3070);
            networkPeer.Start();

            bool keepWaiting;
            if (networkPeer.listener.connection == null)
                keepWaiting = true;
            else if (networkPeer.listener.connection.Status == Lidgren.Network.NetConnectionStatus.None)
                keepWaiting = true;
            else
                keepWaiting = (networkPeer.listener.connection.Status != Lidgren.Network.NetConnectionStatus.Connected);
            
            //while (networkPeer.listener.connection == null)
            while(keepWaiting)
            {
                await Task.Delay(1000);
                Console.WriteLine("Waiting for connection");
                if (networkPeer.listener.connection == null)
                    keepWaiting = true;
                else if (networkPeer.listener.connection.Status == Lidgren.Network.NetConnectionStatus.None)
                    keepWaiting = true;
                else if (networkPeer.listener.connection.Status == Lidgren.Network.NetConnectionStatus.Disconnected)
                {
                    keepWaiting = true;
                    networkPeer.killPeer();
                    Console.WriteLine("reconnecting...");
                    networkPeer = new LidgrenKulamiPeer.KulamiPeer(3071);
                    networkPeer.Start();
                }
                else
                    keepWaiting = (networkPeer.listener.connection.Status != Lidgren.Network.NetConnectionStatus.Connected);
            }

            int networkingBoardNum = 0;

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

        private void NextButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush nb = new ImageBrush();
            nb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/nextButtonOn.png", UriKind.Absolute));
            NextButton.Background = nb;
        }

        private void NextButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush nb = new ImageBrush();
            nb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/nextButton.png", UriKind.Absolute));
            NextButton.Background = nb;
        }
    }
}
