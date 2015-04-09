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
    /// Interaction logic for Scores.xaml
    /// </summary>
    public partial class Scores : UserControl
    {
        private String gameResult;
        private Storyboard boardCaptureStoryboard;
        private Storyboard boardCaptureStoryboard2;
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        ImageBrush cb;

        public Scores(GameStatistics stats, string bluePlayer = "blue", string redPlayer = "red")
        {
            InitializeComponent();
            //gameTimeLabel.Content = stats.ElapsedTime;
            planetsConqueredLabel.Content = stats.RedPlanetsConquered.ToString();
            sectorsConqueredLabel.Content = stats.RedSectorsWon.ToString();
            sectorsLostLabel.Content = stats.RedSectorsLost.ToString();
            totalScoreLabel.Content = stats.RedPoints.ToString();
            //gameTimeLabel2.Content = stats.ElapsedTime;
            planetsConqueredLabel2.Content = stats.BluePlanetsConquered.ToString();
            sectorsConqueredLabel2.Content = stats.BlueSectorsWon.ToString();
            sectorsLostLabel2.Content = stats.BlueSectorsLost.ToString();
            totalScoreLabel2.Content = stats.BluePoints.ToString();
            player2Label.Content = bluePlayer;
            player1Label.Content = redPlayer;
            if (stats.BluePoints > stats.RedPoints)
                winnerBlueButton.Visibility = Visibility.Visible;
            else if (stats.RedPoints > stats.BluePoints)
                winnerRedButton.Visibility = Visibility.Visible;

            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GameStatsBackground.png", UriKind.Absolute));
            ScoresBackground.Background = ib;

            ImageBrush sb = new ImageBrush();
            sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GenericBackground.png", UriKind.Absolute));
            GameBoardCapture.Background = sb;

            ImageBrush wbb = new ImageBrush();
            wbb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/winnerStar.png", UriKind.Absolute));
            winnerBlueButton.Background = wbb;

            ImageBrush wrb = new ImageBrush();
            wrb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/winnerStar.png", UriKind.Absolute));
            winnerRedButton.Background = wrb;

            ImageBrush hb = new ImageBrush();
            hb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButton.png", UriKind.Absolute));
            homeButton.Background = hb;

            ImageBrush gbb = new ImageBrush();
            gbb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/gameBoardButton.png", UriKind.Absolute));
            gameBoardButton.Background = gbb;

            cb = new ImageBrush();
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            cb.ImageSource = new BitmapImage(new Uri(filePath + "/SpaceBallsEndGameBoard.png", UriKind.Absolute));
            CaptureBackground.Background = cb;

            DoubleAnimation captureScreenAnimation = new DoubleAnimation();
            captureScreenAnimation.From = -1440;
            captureScreenAnimation.To = 0;
            captureScreenAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));

            DoubleAnimation captureScreenAnimation2 = new DoubleAnimation();
            captureScreenAnimation2.From = 0;
            captureScreenAnimation2.To = -1440;
            captureScreenAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.4));

            boardCaptureStoryboard = new Storyboard();
            boardCaptureStoryboard2 = new Storyboard();

            boardCaptureStoryboard.Children.Add(captureScreenAnimation);
            boardCaptureStoryboard2.Children.Add(captureScreenAnimation2);

            Storyboard.SetTargetName(captureScreenAnimation, GameBoardCapture.Name);
            Storyboard.SetTargetProperty(captureScreenAnimation, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetName(captureScreenAnimation2, GameBoardCapture.Name);
            Storyboard.SetTargetProperty(captureScreenAnimation2, new PropertyPath(Canvas.LeftProperty));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainPage());
        }

        private void homeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush hb = new ImageBrush();
            hb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButtonHover.png", UriKind.Absolute));
            homeButton.Background = hb;
        }

        private void homeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush hb = new ImageBrush();
            hb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButton.png", UriKind.Absolute));
            homeButton.Background = hb;
        }

        private void gameBoardButton_Click(object sender, RoutedEventArgs e)
        {
            boardCaptureStoryboard.Begin(GameBoardCapture);
        }

        private void gameBoardButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush gbb = new ImageBrush();
            gbb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/gameBoardButtonOn.png", UriKind.Absolute));
            gameBoardButton.Background = gbb;
        }

        private void gameBoardButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush gbb = new ImageBrush();
            gbb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/gameBoardButton.png", UriKind.Absolute));
            gameBoardButton.Background = gbb;
        }

        private void GameBoardCapture_Click(object sender, RoutedEventArgs e)
        {
            boardCaptureStoryboard2.Begin(GameBoardCapture);
        }
    }
}
