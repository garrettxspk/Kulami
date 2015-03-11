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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public MainPage()
        {
            try
            {
                InitializeComponent();
                ImageBrush ib = new ImageBrush();
                ImageBrush qg = new ImageBrush();
                ImageBrush vs = new ImageBrush();
                ImageBrush op = new ImageBrush();
                ImageBrush hp = new ImageBrush();
                ImageBrush ex = new ImageBrush();

                ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/BackgroundMain.png", UriKind.Absolute));
                qg.ImageSource = new BitmapImage(new Uri(startupPath + "/images/QuickGameButton.png", UriKind.Absolute));
                vs.ImageSource = new BitmapImage(new Uri(startupPath + "/images/VersusButton.png", UriKind.Absolute));
                op.ImageSource = new BitmapImage(new Uri(startupPath + "/images/OptionsButton.png", UriKind.Absolute));
                hp.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HelpButton.png", UriKind.Absolute));
                ex.ImageSource = new BitmapImage(new Uri(startupPath + "/images/ExitButton.png", UriKind.Absolute));

                MainBackground.Background = ib;
                QuickGameButton.Background = qg;
                MultiplayerButton.Background = vs;
                OptionsButton.Background = op;
                HelpButton.Background = hp;
                ExitButton.Background = ex;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.ToString());
            }

            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0.0;
            myDoubleAnimation.To = 1.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            myDoubleAnimation.AutoReverse = false;
            

        }
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void QuickGameButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new DifficultySelectionPage());
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new OptionsPage());
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MultiplayerMode());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new HelpScreen());
        }

        private void QuickGameButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush qg = new ImageBrush();
            qg.ImageSource = new BitmapImage(new Uri(startupPath + "/images/QuickGameButtonOn.png", UriKind.Absolute));
            QuickGameButton.Background = qg;

        }

        private void QuickGameButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush qg = new ImageBrush();
            qg.ImageSource = new BitmapImage(new Uri(startupPath + "/images/QuickGameButton.png", UriKind.Absolute));
            QuickGameButton.Background = qg;
        }

        private void MultiplayerButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush vs = new ImageBrush();
            vs.ImageSource = new BitmapImage(new Uri(startupPath + "/images/VersusButtonOn.png", UriKind.Absolute));
            MultiplayerButton.Background = vs;
        }

        private void MultiplayerButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush vs = new ImageBrush();
            vs.ImageSource = new BitmapImage(new Uri(startupPath + "/images/VersusButton.png", UriKind.Absolute));
            MultiplayerButton.Background = vs;
        }

        private void HelpButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush hp = new ImageBrush();
            hp.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HelpButtonOn.png", UriKind.Absolute));
            HelpButton.Background = hp;
        }

        private void HelpButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush hp = new ImageBrush();
            hp.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HelpButton.png", UriKind.Absolute));
            HelpButton.Background = hp;
        }

        private void OptionsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush op = new ImageBrush();
            op.ImageSource = new BitmapImage(new Uri(startupPath + "/images/OptionsButtonOn.png", UriKind.Absolute));
            OptionsButton.Background = op;
        }

        private void OptionsButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush op = new ImageBrush();
            op.ImageSource = new BitmapImage(new Uri(startupPath + "/images/OptionsButton.png", UriKind.Absolute));
            OptionsButton.Background = op;
        }

        private void ExitButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush ex = new ImageBrush();
            ex.ImageSource = new BitmapImage(new Uri(startupPath + "/images/ExitButtonOn.png", UriKind.Absolute));
            ExitButton.Background = ex;
        }

        private void ExitButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush ex = new ImageBrush();
            ex.ImageSource = new BitmapImage(new Uri(startupPath + "/images/ExitButton.png", UriKind.Absolute));
            ExitButton.Background = ex;
        }
    }
}
