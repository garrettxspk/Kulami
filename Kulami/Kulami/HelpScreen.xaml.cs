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
    /// Interaction logic for HelpScreen.xaml
    /// </summary>
    public partial class HelpScreen : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private int currentScreen = 1;
        private Storyboard myStoryboard;
        private SoundEffectsPlayer soundEffectPlayer = new SoundEffectsPlayer();

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

            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0.0;
            myDoubleAnimation.To = 1.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            myDoubleAnimation.AutoReverse = false;

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myDoubleAnimation);
            Storyboard.SetTargetName(myDoubleAnimation, HelpBackground.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Rectangle.OpacityProperty));

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
            soundEffectPlayer.ButtonSound();
            Switcher.Switch(new MainPage());
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
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
            soundEffectPlayer.ButtonSound();
            currentScreen++;
            BackButton.Visibility = Visibility.Visible;
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HelpScreen" + currentScreen + ".png", UriKind.Absolute));
            HelpBackground.Background = ib;
            if (currentScreen >= 7)
                NextButton.Visibility = Visibility.Hidden;
        }

        private void HomeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush hb = new ImageBrush();
            hb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButtonHover.png", UriKind.Absolute));
            HomeButton.Background = hb;
        }

        private void HomeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush hb = new ImageBrush();
            hb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/homeButton.png", UriKind.Absolute));
            HomeButton.Background = hb;
        }

        private void HelpBackground_Loaded(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin(this);
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
