using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : UserControl, ISwitchable
    {
        private IEnumerable<Button> allButtons;
        private Dictionary<string, Button> buttonNames;
        private KulamiEngine engine;
        private EasyAI easyAI;
        private HardAI hardAI;
        private MediaPlayer soundTrackMediaPlayer = new MediaPlayer();
        private SoundEffectsPlayer soundEffectPlayer = new SoundEffectsPlayer();
        private Storyboard myStoryboard;
        bool soundOn = true;
        bool player1turn = true;
        bool easyLevelAIOn = false;
        bool hardLevelAIOn = false;
        bool radarOn = true;
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public GamePage(bool easyLevel, GameType gType)
        {
            InitializeComponent();

            string songPath = startupPath + "/sound/music/Soundtrack.mp3";
            soundTrackMediaPlayer.Open(new Uri(songPath));
            soundTrackMediaPlayer.MediaEnded += new EventHandler(Song_Ended);
            soundTrackMediaPlayer.Play();

            buttonNames = new Dictionary<string,Button>();
            allButtons = GameBackground.Children.OfType<Button>();
            foreach (Button b in allButtons)
            {
                buttonNames.Add(b.Name.ToString(), b);
            }

            int networkingBoardNum = 0;
            if (gType == GameType.LANMultiplayer)
            {
                Random rnd = new Random();
                int myRandomBoardNum = rnd.Next(1, 8);
                //send your number
                int opponentRandomBoardNum = 0; //recieve opponents number
                networkingBoardNum = (myRandomBoardNum + opponentRandomBoardNum)/2;
                while (myRandomBoardNum == opponentRandomBoardNum)
                {
                    myRandomBoardNum = rnd.Next(1, 8);
                    //send number
                    opponentRandomBoardNum = 0; //recieve opponents number
                }
                if (myRandomBoardNum > opponentRandomBoardNum)
                    player1turn = true;
                else
                    player1turn = false;
            }

            engine = new KulamiEngine();
            if (gType == GameType.LANMultiplayer)
                engine.StartGame(gType, networkingBoardNum);
            else
                engine.StartGame(gType);

            //replace this with boolean passed from the difficulty selection screen
            easyLevelAIOn = easyLevel;
            //
            if (engine.CurrentGame.GameType != GameType.LANMultiplayer)
            {
                Random rndMoveFirst = new Random();
                int playFirst = rndMoveFirst.Next(0, 2);

                if (playFirst == 1)
                    player1turn = true;
                else
                    player1turn = false;
            }

            if (engine.CurrentGame.GameType == GameType.LocalComputer)
            {
                if (easyLevelAIOn)
                    easyAI = new EasyAI(engine.CurrentGame);
                else
                    hardAI = new HardAI(engine.CurrentGame);
            }

            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GameBoard" + engine.GameBoardNumber + ".png", UriKind.Absolute));
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
            
            ImageBrush ButtonImage = new ImageBrush();
            ButtonImage.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GenericPlan.png", UriKind.Absolute));
            ApplyBackgroundButtons(ButtonImage);

            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0.0;
            fadeInAnimation.To = 1.0;
            fadeInAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));

            DoubleAnimation fadeOutAnimation = new DoubleAnimation();
            fadeOutAnimation.From = 1.0;
            fadeOutAnimation.To = 0.1;
            fadeOutAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(fadeInAnimation);
            myStoryboard.Children.Add(fadeOutAnimation);

            Storyboard.SetTargetName(fadeInAnimation, WinnerLabel.Name);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(fadeOutAnimation, GameBackground.Name);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(Rectangle.OpacityProperty));

            soundEffectPlayer.PlayStartGameSound();

            if (!player1turn && engine.CurrentGame.GameType == GameType.LocalComputer)
            {
                PlayerTurnLabel.Visibility = Visibility.Hidden;
                ComputerTurnLabel.Visibility = Visibility.Visible;
                MakeAIMove();
            }
            else if (engine.CurrentGame.GameType == GameType.LocalMultiplayer)
            {
                PlayerTurnLabel.Visibility = Visibility.Hidden;
                ComputerTurnLabel.Visibility = Visibility.Hidden;

                if (player1turn)
                    PlayerOneTurnLabel.Visibility = Visibility.Visible;
                else
                    PlayerTwoTurnLabel.Visibility = Visibility.Visible;
            }
        }

        private void Song_Ended(object sender, EventArgs e)
        {
            soundTrackMediaPlayer.Position = TimeSpan.Zero;
            soundTrackMediaPlayer.Play();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            engine.CurrentGame.ForceEndGame();
            soundTrackMediaPlayer.Close();
            if (engine.CurrentGame.GameStats.RedPoints > engine.CurrentGame.GameStats.BluePoints)
            {
                if(soundOn)
                    soundEffectPlayer.WinSound();
            }
            else
                if(soundOn)
                    soundEffectPlayer.LostSound();
            await Task.Delay(3000);
            //myStoryboard.Begin(GameBackground);
            //myStoryboard.Begin(WinnerLabel);
            Switcher.Switch(new Scores(engine.CurrentGame.GameStats));

        }

        private async void planetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!engine.CurrentGame.IsGameOver())
            {
                Button btn = sender as Button;
                string btnName = btn.Name.ToString();
                int row, col;
                row = Convert.ToInt32(btnName.Substring(6, 1));
                col = Convert.ToInt32(btnName.Substring(7, 1));
                Console.WriteLine(row + " " + col);
                if (engine.CurrentGame.IsValidMove(row, col))
                {
                    if (engine.CurrentGame.GameType == GameType.LocalComputer && player1turn)
                    {
                        MakeHumanMove(btn, row, col, "Red");
                        PlayerTurnLabel.Visibility = Visibility.Hidden;
                        ComputerTurnLabel.Visibility = Visibility.Visible;
                        //get move from AI
                        if (!engine.CurrentGame.IsGameOver())
                        {
                            await MakeAIMove();
                        }
                    }
                    else if (engine.CurrentGame.GameType == GameType.LocalMultiplayer && player1turn)
                    {
                        MakeHumanMove(btn, row, col, "Red");
                        PlayerOneTurnLabel.Visibility = Visibility.Hidden;
                        PlayerTwoTurnLabel.Visibility = Visibility.Visible;
                    }
                    else if (engine.CurrentGame.GameType == GameType.LocalMultiplayer && !player1turn)
                    {
                        MakeHumanMove(btn, row, col, "Blue");
                        PlayerOneTurnLabel.Visibility = Visibility.Visible;
                        PlayerTwoTurnLabel.Visibility = Visibility.Hidden;
                    }

                    if (engine.CurrentGame.IsGameOver())
                    {
                        if (engine.CurrentGame.GameType != GameType.LocalMultiplayer)
                        {
                            if (engine.CurrentGame.GameStats.RedPoints > engine.CurrentGame.GameStats.BluePoints)
                            {
                                if (soundOn)
                                    soundEffectPlayer.WinSound();
                            }
                        }
                        else
                            if (soundOn)
                                soundEffectPlayer.LostSound();
                        await Task.Delay(3000);
                        //myStoryboard.Begin(GameBackground);
                        //myStoryboard.Begin(WinnerLabel);
                        soundTrackMediaPlayer.Close();
                        soundEffectPlayer.Close();
                        Switcher.Switch(new Scores(engine.CurrentGame.GameStats));
                    }
                }
            }
        }

        private void MakeHumanMove(Button b, int row, int col, string playerColor)
        {
            ImageBrush ButtonImage = new ImageBrush();
            ButtonImage.ImageSource = new BitmapImage(new Uri(startupPath + "/images/" + playerColor + "Plan1.png", UriKind.Absolute));
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

        private async Task MakeAIMove()
        {
            string aiMove;
            if (easyLevelAIOn)
            {
                aiMove = easyAI.GetMove();
                await Task.Delay(3000);
            }
            else
            {
                aiMove = await Task.Run(() => hardAI.GetMove());
            }

            int aiRow = Convert.ToInt32(aiMove.Substring(1, 1));
            int aiCol = Convert.ToInt32(aiMove.Substring(2, 1));
            string aiMoveBtnName = "planet" + aiRow.ToString() + aiCol.ToString();
            Button aiMoveBtn = buttonNames[aiMoveBtnName];
            ImageBrush AIButtonImage = new ImageBrush();
            AIButtonImage.ImageSource = new BitmapImage(new Uri(startupPath + "/images/BluePlan1.png", UriKind.Absolute));
            aiMoveBtn.Background = AIButtonImage;
            engine.CurrentGame.Board.MakeMoveOnBoard(aiMove);
            if (soundOn)
                soundEffectPlayer.MakeMoveSound();
            if(engine.CurrentGame.Board.WasSectorConquered(aiMove))
            {
                if (soundOn)
                    soundEffectPlayer.ControlSectorSound();
            }
            HighlightAvailableMovesOnBoard();
            PlayerTurnLabel.Visibility = Visibility.Visible;
            ComputerTurnLabel.Visibility = Visibility.Hidden;
            player1turn = !player1turn;
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
                soundTrackMediaPlayer.Play();
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButtonHover.png", UriKind.Absolute));
            }
            else
            {
                soundTrackMediaPlayer.Pause();
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


       
    }
}
