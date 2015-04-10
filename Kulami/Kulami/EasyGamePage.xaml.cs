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
    /// Interaction logic for EasyGamePage.xaml
    /// </summary>
    public partial class EasyGamePage : UserControl, ISwitchable
    {
        private IEnumerable<Button> allButtons;
        private Dictionary<string, Button> buttonNames;
        private KulamiEngine engine;
        private EasyAI AI;
        private MediaPlayer soundTrackMediaPlayer = new MediaPlayer();
        private MediaPlayer soundEffectsMediaPlayer = new MediaPlayer();
        private Storyboard gameOverStoryboard;
        private Storyboard HumanConquerStoryboard;
        private Storyboard AIConquerStoryboard;
        private Storyboard helpStoryboard;
        private Storyboard helpStoryboard2;
        private Storyboard forfeitStoryboard;
        private Storyboard forfeitStoryboard2;

        private SoundEffectsPlayer soundEffectPlayer = new SoundEffectsPlayer();
        bool player1turn = true;
        bool radarOn = true;
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public EasyGamePage()
        {
            InitializeComponent();

            string songPath = startupPath + "/sound/music/Soundtrack.mp3";
            soundTrackMediaPlayer.Open(new Uri(songPath));
            soundTrackMediaPlayer.MediaEnded += new EventHandler(Song_Ended);
            soundTrackMediaPlayer.Play();
            if (!SoundSetting.MusicOn)
                soundTrackMediaPlayer.IsMuted = true;

            buttonNames = new Dictionary<string, Button>();
            allButtons = GameBoard.Children.OfType<Button>();
            foreach (Button b in allButtons)
            {
                buttonNames.Add(b.Name.ToString(), b);
            }

            engine = new KulamiEngine();
            engine.StartGame(GameType.LocalComputer);
            AI = new EasyAI(engine.CurrentGame);
            soundEffectPlayer.PlayStartGameSound();

            Random rndMoveFirst = new Random();
            int playFirst = rndMoveFirst.Next(0, 2);

            if (playFirst == 1)
                player1turn = true;
            else
            {
                player1turn = false;
                PlayerTurnLabel.Visibility = Visibility.Hidden;
                ComputerTurnLabel.Visibility = Visibility.Visible;
                try
                {
                    MakeAIMove();
                }
                catch
                { }
            }

            InitializeImages();
        }

        private void Song_Ended(object sender, EventArgs e)
        {
            soundTrackMediaPlayer.Position = TimeSpan.Zero;
            soundTrackMediaPlayer.Play();
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
                    if (player1turn)
                    {
                        MakeHumanMove(btn, row, col, "Red");
                        PlayerTurnLabel.Visibility = Visibility.Hidden;
                        ComputerTurnLabel.Visibility = Visibility.Visible;
                        //get move from AI
                        if (!engine.CurrentGame.IsGameOver())
                        {
                            try
                            {
                                await MakeAIMove();
                            }
                            catch
                            { }
                        }
                    }

                    if (engine.CurrentGame.IsGameOver())
                    {
                        DisableAllMovesOnBoard();
                        await Task.Delay(100);
                        CaptureGameBoard();
                        soundTrackMediaPlayer.Close();
                        gameOverStoryboard.Begin(GameBackground);

                        if (engine.CurrentGame.GameStats.RedPoints > engine.CurrentGame.GameStats.BluePoints)
                        {
                            WinnerLabel.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE85252"));
                            WinnerLabel.Content = "You Win!";
                            soundEffectPlayer.WinSound();
                        }
                        else if (engine.CurrentGame.GameStats.RedPoints < engine.CurrentGame.GameStats.BluePoints)
                        {
                            WinnerLabel.Content = "Computer Wins";
                            soundEffectPlayer.LostSound();
                        }
                        else
                        {
                            WinnerLabel.Content = "It's a tie!";
                            soundEffectPlayer.LostSound();
                        }

                        await Task.Delay(4000);
                        gameOverStoryboard.Begin(GameBackground);
                        soundEffectPlayer.Stop();
                        soundEffectPlayer.Close();
                        soundTrackMediaPlayer.Stop();
                        soundTrackMediaPlayer.Close();
                        Switcher.Switch(new Scores(engine.CurrentGame.GameStats, "Computer", "You"));

                    }
                }
            }
        }

        private async Task MakeAIMove()
        {
            string aiMove;
            aiMove = AI.GetMove();
            await Task.Delay(3000);

            int aiRow = Convert.ToInt32(aiMove.Substring(1, 1));
            int aiCol = Convert.ToInt32(aiMove.Substring(2, 1));
            string aiMoveBtnName = "planet" + aiRow.ToString() + aiCol.ToString();
            Button aiMoveBtn = buttonNames[aiMoveBtnName];

            Point point = new Point(Canvas.GetLeft(aiMoveBtn), Canvas.GetTop(aiMoveBtn));
            planetConquerTwo.PointToScreen(point);
            TranslateTransform transform = new TranslateTransform(point.X, point.Y);
            planetConquerTwo.RenderTransform = transform;

            ImageBrush AIButtonImage = new ImageBrush();
            AIButtonImage.ImageSource = new BitmapImage(new Uri(startupPath + "/images/BluePlan1.png", UriKind.Absolute));

            AIConquerStoryboard.Begin(planetConquerTwo);
            aiMoveBtn.Background = AIButtonImage;
            engine.CurrentGame.Board.MakeMoveOnBoard(aiMove);
            soundEffectPlayer.MakeMoveSound("Blue");
            HighlightAvailableMovesOnBoard();
            PlayerTurnLabel.Visibility = Visibility.Visible;
            ComputerTurnLabel.Visibility = Visibility.Hidden;
            player1turn = !player1turn;
        }

        private void MakeHumanMove(Button b, int row, int col, string playerColor)
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
            soundEffectPlayer.MakeMoveSound(playerColor[0].ToString());
            string fuelLeft = FuelIndicatorLabel.Content.ToString();
            try
            {
                fuelLeft = fuelLeft.Substring(0, fuelLeft.Length - 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                fuelLeft = "";
            }
            FuelIndicatorLabel.Content = fuelLeft;
            HighlightAvailableMovesOnBoard();
            engine.CurrentGame.Board.PrintGameBoard();
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

        private void DisableAllMovesOnBoard()
        {
            advanceButton.IsEnabled = false;
            toggleRadar_Btn.IsEnabled = false;
            ImageBrush InvalidHole = new ImageBrush();
            InvalidHole.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GenericPlanDisabled.png", UriKind.Absolute));

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
        }

        private void CaptureGameBoard()
        {
            RenderTargetBitmap rtbmp = new RenderTargetBitmap(1440, 900, 96, 96, PixelFormats.Pbgra32);
            rtbmp.Render(GameBoard);

            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(rtbmp));
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            filePath += "/SpaceBallsEndGameBoard.png";
            using (Stream fileStream = File.Create(filePath))
            {
                pngImage.Save(fileStream);
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
            SoundSetting.SoundOn = !SoundSetting.SoundOn;
            if (SoundSetting.SoundOn)
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
            if (SoundSetting.SoundOn)
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
            if (SoundSetting.SoundOn)
            {
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButton.png", UriKind.Absolute));
            }
            else
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButton.png", UriKind.Absolute));

            toggleSound_Btn.Background = sb;
        }

        private void advanceButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Scores(engine.CurrentGame.GameStats, "Computer", "You"));
        }

        private void toggleMusicBtn_Click(object sender, RoutedEventArgs e)
        {
            SoundSetting.MusicOn = !SoundSetting.MusicOn; // musicOn = !musicOn;
            ImageBrush mb = new ImageBrush();
            mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButtonHover.png", UriKind.Absolute));
            if (SoundSetting.MusicOn) //music on
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
            if (SoundSetting.MusicOn) //musicOn
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
            if (SoundSetting.MusicOn) //musicOn
            {
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOnButton.png", UriKind.Absolute));
            }
            else
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButton.png", UriKind.Absolute));

            toggleMusicBtn.Background = mb;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            forfeitStoryboard.Begin(ForfeitScreen);
            //DisableAllMovesOnBoard();
            //await Task.Delay(100);
            //CaptureGameBoard();
            //engine.CurrentGame.ForceEndGame();
            //soundTrackMediaPlayer.Close();
            //gameOverStoryboard.Begin(GameBackground);

            //if (engine.CurrentGame.GameStats.RedPoints > engine.CurrentGame.GameStats.BluePoints)
            //{
            //    WinnerLabel.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE85252"));
            //    WinnerLabel.Content = "You Win!";
            //    soundEffectPlayer.WinSound();
            //}
            //else if (engine.CurrentGame.GameStats.RedPoints < engine.CurrentGame.GameStats.BluePoints)
            //{
            //    WinnerLabel.Content = "Computer Wins!";
            //    soundEffectPlayer.LostSound();
            //}
            //else
            //{
            //    WinnerLabel.Content = "It's a tie!";
            //    soundEffectPlayer.LostSound();
            //}
            //await Task.Delay(4000);

            //Switcher.Switch(new Scores(engine.CurrentGame.GameStats, "Computer", "You"));

        }

        private void screenHelpBtn_Click(object sender, RoutedEventArgs e)
        {
            helpStoryboard.Begin(GameScreenHelp);
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

        private void advanceButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush ab = new ImageBrush();
            ab.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButtonHover.png", UriKind.Absolute));
            advanceButton.Background = ab;
        }

        private void advanceButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush ab = new ImageBrush();
            ab.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButton.png", UriKind.Absolute));
            advanceButton.Background = ab;
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.Stop();
            soundEffectPlayer.Close();
            soundTrackMediaPlayer.Stop();
            soundTrackMediaPlayer.Close();
            Switcher.Switch(new MainPage());
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            forfeitStoryboard2.Begin(ForfeitScreen);
        }

        private void GameScreenHelp_Click(object sender, RoutedEventArgs e)
        {
            helpStoryboard2.Begin(GameScreenHelp);
        }
        #endregion Button Event Handlers

        #region Graphics Initialization
        private void InitializeImages()
        {
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GameBoard" + engine.GameBoardNumber + ".png", UriKind.Absolute));
            BoardBackground.Background = ib;

            ImageBrush gb = new ImageBrush();
            gb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GameBackground.jpg", UriKind.Absolute));
            backgroundButton.Background = gb;

            ImageBrush sb = new ImageBrush();
            if (SoundSetting.SoundOn)
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButton.png", UriKind.Absolute));
            else
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButton.png", UriKind.Absolute));
            toggleSound_Btn.Background = sb;

            ImageBrush mb = new ImageBrush();
            if (SoundSetting.MusicOn)
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOnButton.png", UriKind.Absolute));
            else
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButton.png", UriKind.Absolute));
            toggleMusicBtn.Background = mb;

            ImageBrush rb = new ImageBrush();
            rb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/radarOnButton.png", UriKind.Absolute));
            toggleRadar_Btn.Background = rb;

            ImageBrush ButtonImage = new ImageBrush();
            ButtonImage.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GenericPlan.png", UriKind.Absolute));
            ApplyBackgroundButtons(ButtonImage);

            ImageBrush RedConquer = new ImageBrush();
            RedConquer.ImageSource = new BitmapImage(new Uri(startupPath + "/images/PlanConquerRed.png", UriKind.Absolute));
            planetConquerOne.Background = RedConquer;

            ImageBrush sh = new ImageBrush();
            ImageBrush msh = new ImageBrush();
            sh.ImageSource = new BitmapImage(new Uri(startupPath + "/images/screenHelpButton.png", UriKind.Absolute));
            msh.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GameScreenHelp.png", UriKind.Absolute));
            screenHelpBtn.Background = sh;
            GameScreenHelp.Background = msh;

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
            planetConquerOneAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
            planetConquerOneAnimation.AutoReverse = true;

            DoubleAnimation planetConquerTwoAnimation = new DoubleAnimation();
            planetConquerTwoAnimation.From = 0.0;
            planetConquerTwoAnimation.To = 1.0;
            planetConquerTwoAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
            planetConquerTwoAnimation.AutoReverse = true;

            DoubleAnimation helpScreenAnimation = new DoubleAnimation();
            helpScreenAnimation.From = -1440;
            helpScreenAnimation.To = 0;
            helpScreenAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));

            DoubleAnimation helpScreenAnimation2 = new DoubleAnimation();
            helpScreenAnimation2.From = 0;
            helpScreenAnimation2.To = -1440;
            helpScreenAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.4));

            ImageBrush ab = new ImageBrush();
            ab.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButton.png", UriKind.Absolute));
            advanceButton.Background = ab;

            DoubleAnimation forfeitAnimation = new DoubleAnimation();
            forfeitAnimation.From = -250;
            forfeitAnimation.To = 0;
            forfeitAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));

            DoubleAnimation forfeitAnimation2 = new DoubleAnimation();
            forfeitAnimation2.From = 0;
            forfeitAnimation2.To = -250;
            forfeitAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.4));

            forfeitStoryboard = new Storyboard();
            forfeitStoryboard2 = new Storyboard();

            forfeitStoryboard.Children.Add(forfeitAnimation);
            forfeitStoryboard2.Children.Add(forfeitAnimation2);

            Storyboard.SetTargetName(forfeitAnimation, ForfeitScreen.Name);
            Storyboard.SetTargetProperty(forfeitAnimation, new PropertyPath(Canvas.BottomProperty));
            Storyboard.SetTargetName(forfeitAnimation2, ForfeitScreen.Name);
            Storyboard.SetTargetProperty(forfeitAnimation2, new PropertyPath(Canvas.BottomProperty));

            helpStoryboard = new Storyboard();
            helpStoryboard2 = new Storyboard();

            helpStoryboard.Children.Add(helpScreenAnimation);
            helpStoryboard2.Children.Add(helpScreenAnimation2);

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

            Storyboard.SetTargetName(helpScreenAnimation, GameScreenHelp.Name);
            Storyboard.SetTargetProperty(helpScreenAnimation, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetName(helpScreenAnimation2, GameScreenHelp.Name);
            Storyboard.SetTargetProperty(helpScreenAnimation2, new PropertyPath(Canvas.LeftProperty));

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
