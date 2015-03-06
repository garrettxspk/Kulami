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
    /// Interaction logic for HelpScreen.xaml
    /// </summary>
    public partial class HelpScreen : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private int currentScreen = 1;

        public HelpScreen()
        {
            InitializeComponent();
            ImageBrush ib = new ImageBrush();
            ImageBrush backButtonib = new ImageBrush();
            ImageBrush nextButtonib = new ImageBrush();
            ImageBrush homeButtonib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HelpScreen1.png", UriKind.Absolute));
            backButtonib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/backButton.png", UriKind.Absolute));
            nextButtonib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/nextButton.png", UriKind.Absolute));
            homeButtonib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButton.png", UriKind.Absolute));
            HelpBackground.Background = ib;
            NextButton.Background = nextButtonib;
            BackButton.Background = backButtonib;
            HomeButton.Background = homeButtonib;

            if (currentScreen <= 1)
            {
                BackButton.Visibility = Visibility.Hidden;
                NextButton.Visibility = Visibility.Visible;
            }
            else
            {
                BackButton.Visibility = Visibility.Visible;
            }
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainPage());
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            currentScreen--;
            NextButton.Visibility = Visibility.Visible;
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HelpScreen" + currentScreen + ".png", UriKind.Absolute));
            HelpBackground.Background = ib;
            if (currentScreen <= 1)
                BackButton.Visibility = Visibility.Hidden;
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            currentScreen++;
            BackButton.Visibility = Visibility.Visible;
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HelpScreen" + currentScreen + ".png", UriKind.Absolute));
            HelpBackground.Background = ib;
            if (currentScreen >= 5)
                NextButton.Visibility = Visibility.Hidden;
        }
    }
}
