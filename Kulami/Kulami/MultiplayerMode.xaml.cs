﻿using System;
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
        private Storyboard helpStoryboard;
        private Storyboard helpStoryboard2;
        public MultiplayerMode()
        {
            InitializeComponent();
            ImageBrush backgrnd = new ImageBrush();
            ImageBrush localBtnBackgrnd = new ImageBrush();
            ImageBrush onlineBtnBackgrnd = new ImageBrush();
            ImageBrush backButtonib = new ImageBrush();
            ImageBrush nextButtonib = new ImageBrush();
            ImageBrush sh = new ImageBrush();
            ImageBrush msh = new ImageBrush();


            backButtonib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/backButton.png", UriKind.Absolute));
            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            localBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/LocalButton.png", UriKind.Absolute));
            onlineBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/OnlineButton.png", UriKind.Absolute));
            nextButtonib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/nextButton.png", UriKind.Absolute));
            sh.ImageSource = new BitmapImage(new Uri(startupPath + "/images/screenHelpButton.png", UriKind.Absolute));
            msh.ImageSource = new BitmapImage(new Uri(startupPath + "/images/MultiplayerScreenHelp.png", UriKind.Absolute));

            SelectionBackground.Background = backgrnd;
            LocalModeButton.Background = localBtnBackgrnd;
            OnlineModeButton.Background = onlineBtnBackgrnd;
            BackButton.Background = backButtonib;
            NextButton.Background = nextButtonib;
            screenHelpBtn.Background = sh;
            MultiplayerScreenHelp.Background = msh;

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

            DoubleAnimation helpScreenAnimation = new DoubleAnimation();
            helpScreenAnimation.From = -1440;
            helpScreenAnimation.To = 0;
            helpScreenAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.8));

            DoubleAnimation helpScreenAnimation2 = new DoubleAnimation();
            helpScreenAnimation2.From = 0;
            helpScreenAnimation2.To = -1440;
            helpScreenAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.8));

            helpStoryboard = new Storyboard();
            helpStoryboard2 = new Storyboard();

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(FadeIn);
            myStoryboard.Children.Add(FadeIn2);
            myStoryboard.Children.Add(FadeOut);
            myStoryboard.Children.Add(FadeOut2);

            helpStoryboard.Children.Add(helpScreenAnimation);
            helpStoryboard2.Children.Add(helpScreenAnimation2);

            Storyboard.SetTargetName(FadeIn, PlayerNameTextBox.Name);
            Storyboard.SetTargetProperty(FadeIn, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(FadeIn2, NextButton.Name);
            Storyboard.SetTargetProperty(FadeIn2, new PropertyPath(Rectangle.OpacityProperty));

            Storyboard.SetTargetName(FadeOut, OnlineModeButton.Name);
            Storyboard.SetTargetProperty(FadeOut, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(FadeOut2, LocalModeButton.Name);
            Storyboard.SetTargetProperty(FadeOut2, new PropertyPath(Rectangle.OpacityProperty));

            Storyboard.SetTargetName(helpScreenAnimation, MultiplayerScreenHelp.Name);
            Storyboard.SetTargetProperty(helpScreenAnimation, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetName(helpScreenAnimation2, MultiplayerScreenHelp.Name);
            Storyboard.SetTargetProperty(helpScreenAnimation2, new PropertyPath(Canvas.LeftProperty));


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
            PlayerNameTextBox.Focus();
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
            string playerName = (string) PlayerNameTextBox.Text;
            if (playerName == null)
                playerName = "Anonymous";
            LidgrenKulamiPeer.KulamiPeer networkPeer = new LidgrenKulamiPeer.KulamiPeer(3070);
            networkPeer.Start();
            Switcher.Switch(new WaitingForConnectionPage());
            await StartNetworkGame(playerName, networkPeer);
        }

        private async Task StartNetworkGame(string playerName, LidgrenKulamiPeer.KulamiPeer networkPeer)
        {
            bool shouldContinue = true;
            Random rnd = new Random();
            DateTime start = DateTime.Now;
            DateTime end;
            bool keepWaiting;
            if (networkPeer.listener.connection == null)
                keepWaiting = true;
            else if (networkPeer.listener.connection.Status == Lidgren.Network.NetConnectionStatus.None)
                keepWaiting = true;
            else
                keepWaiting = (networkPeer.listener.connection.Status != Lidgren.Network.NetConnectionStatus.Connected);

            while (keepWaiting)
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
                    networkPeer = null;
                    Console.WriteLine("reconnecting...");
                    networkPeer = new LidgrenKulamiPeer.KulamiPeer(3070);
                    networkPeer.Start();
                }
                else
                    keepWaiting = (networkPeer.listener.connection.Status != Lidgren.Network.NetConnectionStatus.Connected);
                
                end = DateTime.Now;
                if ((end - start).TotalSeconds > 30)
                {
                    shouldContinue = false;
                    networkPeer.killPeer();
                    networkPeer = null;
                    break;
                }
            }

            if (!shouldContinue)
            {
                networkPeer.killPeer();
                networkPeer = null;
                Switcher.Switch(new NoConnectionsFoundPage());
            }
            else
            {
                networkPeer.sendMove(playerName);
                string opponentName = networkPeer.getMove();
                while (opponentName == null)
                {
                    await Task.Delay(1000);
                    opponentName = networkPeer.getMove();
                }

                Switcher.Switch(new OpponentNamePage(opponentName));
                await Task.Delay(3000);

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

        private void screenHelpBtn_Click(object sender, RoutedEventArgs e)
        {
            helpStoryboard.Begin(MultiplayerScreenHelp);
        }

        private void screenHelpBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush sh = new ImageBrush();
            sh.ImageSource = new BitmapImage(new Uri(startupPath + "/images/screenHelpButtonHover.png", UriKind.Absolute));
            screenHelpBtn.Background = sh;
        }

        private void screenHelpBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush sh = new ImageBrush();
            sh.ImageSource = new BitmapImage(new Uri(startupPath + "/images/screenHelpButton.png", UriKind.Absolute));
            screenHelpBtn.Background = sh;
        }

        private void MultiplayerScreenHelp_Click(object sender, RoutedEventArgs e)
        {
            helpStoryboard2.Begin(MultiplayerScreenHelp);

        }
    }
}
