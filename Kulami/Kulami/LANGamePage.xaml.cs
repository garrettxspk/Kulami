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
    /// Interaction logic for EasyGamePage.xaml
    /// </summary>
    public partial class LANGamePage : UserControl, ISwitchable
    {
        private IEnumerable<Button> allButtons;
        private Dictionary<string, Button> buttonNames;
        private KulamiEngine engine;
        private MediaPlayer soundTrackMediaPlayer = new MediaPlayer();
        private MediaPlayer soundEffectsMediaPlayer = new MediaPlayer();
        private Storyboard gameOverStoryboard;
        private Storyboard HumanConquerStoryboard;
        private Storyboard AIConquerStoryboard;
        private SoundEffectsPlayer soundEffectPlayer = new SoundEffectsPlayer();
        bool connected = true;
        private LidgrenKulamiPeer.KulamiPeer networkPeer;
        bool soundOn = true;
        bool musicOn = true;
        bool player1turn = true;
        bool radarOn = true;
        string myColor;
        string opponentsColor;
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public delegate void StartLANGame();
        public LANGamePage(LidgrenKulamiPeer.KulamiPeer nPeer, int boardNum, bool meFirst)
        {
            InitializeComponent();

            networkPeer = nPeer;

            if (meFirst)
            {
                PlayerTurnLabel.Visibility = System.Windows.Visibility.Visible;
                OpponentTurnLabel.Visibility = System.Windows.Visibility.Hidden;
                player1turn = true;
                myColor = "Red";
                opponentsColor = "Blue";
            }
            else
            {
                PlayerTurnLabel.Visibility = System.Windows.Visibility.Hidden;
                OpponentTurnLabel.Visibility = System.Windows.Visibility.Visible;
                player1turn = false;
                myColor = "Blue";
                opponentsColor = "Red";
            }

            string songPath = startupPath + "/sound/music/Soundtrack.mp3";
            soundTrackMediaPlayer.Open(new Uri(songPath));
            soundTrackMediaPlayer.MediaEnded += new EventHandler(Song_Ended);
            soundTrackMediaPlayer.Play();

            buttonNames = new Dictionary<string, Button>();
            allButtons = GameBackground.Children.OfType<Button>();
            foreach (Button b in allButtons)
            {
                buttonNames.Add(b.Name.ToString(), b);
            }

            engine = new KulamiEngine();
            engine.StartGame(GameType.LANMultiplayer, boardNum);
            soundEffectPlayer.PlayStartGameSound();

            InitializeImages(boardNum);
            if (!meFirst)
            {
                if (!IsConnected())
                {
                    Disconnect();
                }
                else
                    MakeOpponentMove();
            }
        }

        private bool IsConnected()
        {
            bool result = true;
            if (networkPeer == null)
                result = false;
            else
                result = (networkPeer.listener.connection.Status == Lidgren.Network.NetConnectionStatus.Connected);
            return result;
        }

        private void Disconnect()
        {
            if (networkPeer != null)
            {
                networkPeer.killPeer();
                networkPeer = null;
            }
            connected = false;
            Switcher.Switch(new ConnectionLostPage());
        }

        private void Song_Ended(object sender, EventArgs e)
        {
            soundTrackMediaPlayer.Position = TimeSpan.Zero;
            soundTrackMediaPlayer.Play();
        }

        private async void planetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!engine.CurrentGame.IsGameOver() && connected)
            {
                Button btn = sender as Button;
                string btnName = btn.Name.ToString();
                int row, col;
                row = Convert.ToInt32(btnName.Substring(6, 1));
                col = Convert.ToInt32(btnName.Substring(7, 1));
                Console.WriteLine(row + " " + col);

                if (engine.CurrentGame.IsValidMove(row, col))
                {
                    if (player1turn)
                    {
                        MakeHumanMove(btn, row, col, myColor);
                        if (IsConnected())
                            networkPeer.sendMove("R" + row.ToString() + col.ToString());
                        else
                            Disconnect();
                        PlayerTurnLabel.Visibility = Visibility.Hidden;
                        OpponentTurnLabel.Visibility = Visibility.Visible;
                        if (!engine.CurrentGame.IsGameOver() && connected)
                        {
                            if (!IsConnected())
                            {
                                Disconnect();
                            }
                            else
                                await MakeOpponentMove();
                        }
                    }

                    if (engine.CurrentGame.IsGameOver() && connected)
                    {
                        soundTrackMediaPlayer.Close();
                        gameOverStoryboard.Begin(GameBackground);

                        if (engine.CurrentGame.GameType != GameType.LocalMultiplayer)
                        {
                            if (engine.CurrentGame.GameStats.RedPoints > engine.CurrentGame.GameStats.BluePoints)
                            {
                                if (myColor == "Red")
                                {
                                    WinnerLabel.Content = "You Win!";
                                    soundEffectPlayer.WinSound();
                                }
                                else
                                {
                                    WinnerLabel.Content = "You Lose!";
                                    soundEffectPlayer.LostSound();
                                }
                            }
                            else if (engine.CurrentGame.GameStats.RedPoints < engine.CurrentGame.GameStats.BluePoints)
                            {
                                if (myColor == "Blue")
                                {
                                    WinnerLabel.Content = "You Win!";
                                    soundEffectPlayer.WinSound();
                                }
                                else
                                {
                                    WinnerLabel.Content = "You Lose!";
                                    soundEffectPlayer.LostSound();
                                }
                            }
                            else
                            {
                                WinnerLabel.Content = "It's a tie!";
                            }

                        }

                        await Task.Delay(4000);
                        gameOverStoryboard.Begin(GameBackground);
                        soundTrackMediaPlayer.Close();
                        soundEffectPlayer.Close();
                        Switcher.Switch(new Scores(engine.CurrentGame.GameStats));

                    }
                }
            }
        }

        private async Task MakeOpponentMove()
        {
            bool skip = false;
            if (IsConnected())
            {
                string opponentMove = networkPeer.getMove();
                while (opponentMove == null)
                {
                    await Task.Delay(1000);
                    if (networkPeer != null)
                    {
                        opponentMove = networkPeer.getMove();
                    }
                    else
                    {
                        skip = true;
                        break;
                    }
                }
                if (!skip)
                {
                    int opponentRow = Convert.ToInt32(opponentMove.Substring(1, 1));
                    int opponentCol = Convert.ToInt32(opponentMove.Substring(2, 1));
                    string opponentMoveBtnName = "planet" + opponentRow.ToString() + opponentCol.ToString();
                    Button opponentMoveBtn = buttonNames[opponentMoveBtnName];
                    MakeHumanMove(opponentMoveBtn, opponentRow, opponentCol, opponentsColor);
                    PlayerTurnLabel.Visibility = Visibility.Visible;
                    OpponentTurnLabel.Visibility = Visibility.Hidden;
                }
            }
            else
                Disconnect();
        }

        private void MakeHumanMove(Button b, int row, int col, string playerColor)
        {
            if (IsConnected())
            {
                Point point = new Point(Canvas.GetLeft(b), Canvas.GetTop(b));
                ImageBrush ButtonImage = new ImageBrush();
                ButtonImage.ImageSource = new BitmapImage(new Uri(startupPath + "/images/" + playerColor + "Plan1.png", UriKind.Absolute));

                if (playerColor == "Red")
                {
                    planetConquerOne.PointToScreen(point);
                    TranslateTransform transform = new TranslateTransform(point.X, point.Y);
                    planetConquerOne.RenderTransform = transform;
                    HumanConquerStoryboard.Begin(planetConquerOne);
                }
                else if (playerColor == "Blue")
                {
                    planetConquerTwo.PointToScreen(point);
                    TranslateTransform transform = new TranslateTransform(point.X, point.Y);
                    planetConquerTwo.RenderTransform = transform;
                    AIConquerStoryboard.Begin(planetConquerTwo);
                }

                b.Background = ButtonImage;

                engine.CurrentGame.Board.MakeMoveOnBoard(playerColor[0] + row.ToString() + col.ToString());
                if (soundOn)
                    soundEffectPlayer.MakeMoveSound();
                if (engine.CurrentGame.Board.WasSectorConquered(playerColor[0] + row.ToString() + col.ToString()))
                {
                    if (soundOn)
                        soundEffectPlayer.ControlSectorSound();
                }
                HighlightAvailableMovesOnBoard();
                engine.CurrentGame.Board.PrintGameBoard();
                player1turn = !player1turn;
            }
            else
                Disconnect();
        }

        private void HighlightAvailableMovesOnBoard()
        {
            if (radarOn)
            {
                List<Coordinate> availableMoves = engine.CurrentGame.GetAllAvailableMoves();
                ImageBrush InvalidHole = new ImageBrush();
                InvalidHole.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GenericPlanDisabled.png", UriKind.Absolute));

                ImageBrush ValidHole = new ImageBrush();
                ValidHole.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GenericPlan.png", UriKind.Absolute));

                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if (!engine.CurrentGame.Board.IsHoleFilled(row, col))
                        {
                            Button b = buttonNames["planet" + row.ToString() + col.ToString()];
                            b.Background = InvalidHole;
                        }
                    }
                }

                foreach (Coordinate c in availableMoves)
                {
                    Button b = buttonNames["planet" + c.Row.ToString() + c.Col.ToString()];
                    b.Background = ValidHole;
                }

            }
        }

        #region Button Event Handlers
        private void TurnOffMoveHelp()
        {

            ImageBrush ValidHole = new ImageBrush();
            ValidHole.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GenericPlan.png", UriKind.Absolute));

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (!engine.CurrentGame.Board.IsHoleFilled(row, col))
                    {
                        Button b = buttonNames["planet" + row.ToString() + col.ToString()];
                        b.Background = ValidHole;
                    }
                }
            }


        }

        private void Toggle_Sound(object sender, RoutedEventArgs e)
        {
            ImageBrush sb = new ImageBrush();
            sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButtonHover.png", UriKind.Absolute));
            soundOn = !soundOn;
            if (soundOn)
            {
                soundEffectPlayer.UnMute();
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButtonHover.png", UriKind.Absolute));
            }
            else
            {
                soundEffectPlayer.Mute();
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButtonHover.png", UriKind.Absolute));
            }
            toggleSound_Btn.Background = sb;
        }

        private void toggleRadar_Btn_Click(object sender, RoutedEventArgs e)
        {
            radarOn = !radarOn;
            ImageBrush rb = new ImageBrush();
            if (radarOn)
            {
                HighlightAvailableMovesOnBoard();
                rb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/radarOnButtonHover.png", UriKind.Absolute));
            }
            else
            {
                TurnOffMoveHelp();
                rb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/radarOffButtonHover.png", UriKind.Absolute));
            }

            toggleRadar_Btn.Background = rb;
        }

        private void toggleRadar_Btn_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void toggleRadar_Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush rb = new ImageBrush();
            if (radarOn)
            {
                rb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/radarOnButtonHover.png", UriKind.Absolute));
            }
            else
                rb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/radarOffButtonHover.png", UriKind.Absolute));

            toggleRadar_Btn.Background = rb;
        }

        private void toggleRadar_Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush rb = new ImageBrush();
            if (radarOn)
            {
                rb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/radarOnButton.png", UriKind.Absolute));
            }
            else
                rb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/radarOffButton.png", UriKind.Absolute));

            toggleRadar_Btn.Background = rb;
        }

        private void toggleSound_Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush sb = new ImageBrush();
            if (soundOn)
            {
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButtonHover.png", UriKind.Absolute));
            }
            else
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButtonHover.png", UriKind.Absolute));

            toggleSound_Btn.Background = sb;
        }

        private void toggleSound_Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush sb = new ImageBrush();
            if (soundOn)
            {
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButton.png", UriKind.Absolute));
            }
            else
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButton.png", UriKind.Absolute));

            toggleSound_Btn.Background = sb;
        }

        private void advanceButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Scores(engine.CurrentGame.GameStats));
        }

        private void toggleMusicBtn_Click(object sender, RoutedEventArgs e)
        {
            musicOn = !musicOn; // musicOn = !musicOn;
            ImageBrush mb = new ImageBrush();
            mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButtonHover.png", UriKind.Absolute));
            if (musicOn) //music on
            {
                soundTrackMediaPlayer.IsMuted = false;
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOnButtonHover.png", UriKind.Absolute));
            }
            else
            {
                soundTrackMediaPlayer.IsMuted = true;
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButtonHover.png", UriKind.Absolute));
            }

            toggleMusicBtn.Background = mb;
        }

        private void toggleMusicBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush mb = new ImageBrush();
            if (musicOn) //musicOn
            {
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOnButtonHover.png", UriKind.Absolute));
            }
            else
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButtonHover.png", UriKind.Absolute));

            toggleMusicBtn.Background = mb;
        }

        private void toggleMusicBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush mb = new ImageBrush();
            if (musicOn) //musicOn
            {
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOnButton.png", UriKind.Absolute));
            }
            else
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButton.png", UriKind.Absolute));

            toggleMusicBtn.Background = mb;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            engine.CurrentGame.ForceEndGame();
            soundTrackMediaPlayer.Close();
            gameOverStoryboard.Begin(GameBackground);

            networkPeer.killPeer();
            networkPeer = null;
            connected = false;

            if (engine.CurrentGame.GameStats.RedPoints > engine.CurrentGame.GameStats.BluePoints)
            {
                WinnerLabel.Content = "Red Wins!";
                soundEffectPlayer.WinSound();
            }
            else if (engine.CurrentGame.GameStats.RedPoints < engine.CurrentGame.GameStats.BluePoints)
            {
                WinnerLabel.Content = "Blue Wins!";
                soundEffectPlayer.LostSound();
            }
            else
            {
                WinnerLabel.Content = "It's a tie!";
                soundEffectPlayer.LostSound();
            }
            await Task.Delay(4000);

            Switcher.Switch(new Scores(engine.CurrentGame.GameStats));

        }
        #endregion Button Event Handlers

        #region Graphics Initialization
        private void InitializeImages(int boardNumber)
        {
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GameBoard" + boardNumber + ".png", UriKind.Absolute));
            BoardBackground.Background = ib;

            ImageBrush gb = new ImageBrush();
            gb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GameBackground.jpg", UriKind.Absolute));
            backgroundButton.Background = gb;

            ImageBrush sb = new ImageBrush();
            sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButton.png", UriKind.Absolute));
            toggleSound_Btn.Background = sb;

            ImageBrush rb = new ImageBrush();
            rb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/radarOnButton.png", UriKind.Absolute));
            toggleRadar_Btn.Background = rb;

            ImageBrush mb = new ImageBrush();
            mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOnButton.png", UriKind.Absolute));
            toggleMusicBtn.Background = mb;

            ImageBrush ButtonImage = new ImageBrush();
            ButtonImage.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GenericPlan.png", UriKind.Absolute));
            ApplyBackgroundButtons(ButtonImage);

            ImageBrush RedConquer = new ImageBrush();
            RedConquer.ImageSource = new BitmapImage(new Uri(startupPath + "/images/PlanConquerRed.png", UriKind.Absolute));
            planetConquerOne.Background = RedConquer;

            ImageBrush BlueConquer = new ImageBrush();
            BlueConquer.ImageSource = new BitmapImage(new Uri(startupPath + "/images/PlanConquerBlue.png", UriKind.Absolute));
            planetConquerTwo.Background = BlueConquer;

            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0.0;
            fadeInAnimation.To = 1.0;
            fadeInAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));

            DoubleAnimation fadeOutAnimation = new DoubleAnimation();
            fadeOutAnimation.From = 1.0;
            fadeOutAnimation.To = 0.1;
            fadeOutAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));

            DoubleAnimation planetConquerOneAnimation = new DoubleAnimation();
            planetConquerOneAnimation.From = 0.0;
            planetConquerOneAnimation.To = 1.0;
            planetConquerOneAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            planetConquerOneAnimation.AutoReverse = true;

            DoubleAnimation planetConquerTwoAnimation = new DoubleAnimation();
            planetConquerTwoAnimation.From = 0.0;
            planetConquerTwoAnimation.To = 1.0;
            planetConquerTwoAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            planetConquerTwoAnimation.AutoReverse = true;

            gameOverStoryboard = new Storyboard();
            HumanConquerStoryboard = new Storyboard();
            AIConquerStoryboard = new Storyboard();

            gameOverStoryboard.Children.Add(fadeInAnimation);
            gameOverStoryboard.Children.Add(fadeOutAnimation);
            HumanConquerStoryboard.Children.Add(planetConquerOneAnimation);
            AIConquerStoryboard.Children.Add(planetConquerTwoAnimation);

            Storyboard.SetTargetName(fadeInAnimation, WinnerLabel.Name);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(fadeOutAnimation, GameBackground.Name);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(Rectangle.OpacityProperty));

            Storyboard.SetTargetName(planetConquerOneAnimation, planetConquerOne.Name);
            Storyboard.SetTargetProperty(planetConquerOneAnimation, new PropertyPath(Rectangle.OpacityProperty));

            Storyboard.SetTargetName(planetConquerTwoAnimation, planetConquerTwo.Name);
            Storyboard.SetTargetProperty(planetConquerTwoAnimation, new PropertyPath(Rectangle.OpacityProperty));

        }
        private void ApplyBackgroundButtons(ImageBrush ib)
        {
            planet00.Background = planet01.Background = planet02.Background = planet03.Background = planet04.Background = planet05.Background = planet06.Background = planet07.Background =
            planet10.Background = planet11.Background = planet12.Background = planet13.Background = planet14.Background = planet15.Background = planet16.Background = planet17.Background =
            planet20.Background = planet21.Background = planet22.Background = planet23.Background = planet24.Background = planet25.Background = planet26.Background = planet27.Background =
            planet30.Background = planet31.Background = planet32.Background = planet33.Background = planet34.Background = planet35.Background = planet36.Background = planet37.Background =
            planet40.Background = planet41.Background = planet42.Background = planet43.Background = planet44.Background = planet45.Background = planet46.Background = planet47.Background =
            planet50.Background = planet51.Background = planet52.Background = planet53.Background = planet54.Background = planet55.Background = planet56.Background = planet57.Background =
            planet60.Background = planet61.Background = planet62.Background = planet63.Background = planet64.Background = planet65.Background = planet66.Background = planet67.Background =
            planet70.Background = planet71.Background = planet72.Background = planet73.Background = planet74.Background = planet75.Background = planet76.Background = planet77.Background =
            ib;
        }
        #endregion Graphics Initialization

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
    }
}
