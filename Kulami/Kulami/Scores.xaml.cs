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
    /// Interaction logic for Scores.xaml
    /// </summary>
    public partial class Scores : UserControl
    {
        private String gameResult;
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public Scores(GameStatistics stats)
        {
            InitializeComponent();
            //gameResult = result;
            //resultLabel.Content = gameResult;
            gameTimeLabel.Content = stats.ElapsedTime;
            planetsConqueredLabel.Content = stats.RedPlanetsConquered.ToString();
            sectorsConqueredLabel.Content = stats.RedSectorsWon.ToString();
            sectorsLostLabel.Content = stats.RedSectorsLost.ToString();
            totalScoreLabel.Content = stats.RedPoints.ToString();
            gameTimeLabel2.Content = stats.ElapsedTime;
            planetsConqueredLabel2.Content = stats.BluePlanetsConquered.ToString();
            sectorsConqueredLabel2.Content = stats.BlueSectorsWon.ToString();
            sectorsLostLabel2.Content = stats.BlueSectorsLost.ToString();
            totalScoreLabel2.Content = stats.BluePoints.ToString();
            winnerLabel.Content = (stats.BluePoints == stats.RedPoints) ? "It's a tie!" : "Game Over";
            winnerLabel.Content = (stats.BluePoints > stats.RedPoints) ? "Blue player wins!" : "Red player wins!";
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/GameStatsBackground.png", UriKind.Absolute));
            ScoresBackground.Background = ib;

            ImageBrush hb = new ImageBrush();
            hb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButton.png", UriKind.Absolute));
            homeButton.Background = hb;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainPage());
        }
    }
}
