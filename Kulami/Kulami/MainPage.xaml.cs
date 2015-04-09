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
        private Storyboard myStoryboard;
        private Storyboard helpStoryboard;
        private Storyboard helpStoryboard2;
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private SoundEffectsPlayer soundEffectPlayer = new SoundEffectsPlayer();
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
                ImageBrush ss1 = new ImageBrush();
                ImageBrush ss2 = new ImageBrush();
                ImageBrush ss3 = new ImageBrush();
                ImageBrush ss4 = new ImageBrush();
                ImageBrush ss5 = new ImageBrush();
                ImageBrush sh = new ImageBrush();
                ImageBrush msh = new ImageBrush();

                ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/BackgroundMain.png", UriKind.Absolute));
                qg.ImageSource = new BitmapImage(new Uri(startupPath + "/images/QuickGameButton.png", UriKind.Absolute));
                vs.ImageSource = new BitmapImage(new Uri(startupPath + "/images/VersusButton.png", UriKind.Absolute));
                op.ImageSource = new BitmapImage(new Uri(startupPath + "/images/OptionsButton.png", UriKind.Absolute));
                hp.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HelpButton.png", UriKind.Absolute));
                ex.ImageSource = new BitmapImage(new Uri(startupPath + "/images/ExitButton.png", UriKind.Absolute));
                ss1.ImageSource = new BitmapImage(new Uri(startupPath + "/images/StarSet1.png", UriKind.Absolute));
                ss2.ImageSource = new BitmapImage(new Uri(startupPath + "/images/StarSet2.png", UriKind.Absolute));
                ss3.ImageSource = new BitmapImage(new Uri(startupPath + "/images/StarSet3.png", UriKind.Absolute));
                ss4.ImageSource = new BitmapImage(new Uri(startupPath + "/images/StarSet4.png", UriKind.Absolute));
                ss5.ImageSource = new BitmapImage(new Uri(startupPath + "/images/StarSet5.png", UriKind.Absolute));
                sh.ImageSource = new BitmapImage(new Uri(startupPath + "/images/screenHelpButton.png", UriKind.Absolute));
                msh.ImageSource = new BitmapImage(new Uri(startupPath + "/images/MainScreenHelp.png", UriKind.Absolute));


                MainBackground.Background = ib;
                QuickGameButton.Background = qg;
                MultiplayerButton.Background = vs;
                OptionsButton.Background = op;
                HelpButton.Background = hp;
                ExitButton.Background = ex;
                StarSet1.Background = ss1;
                StarSet2.Background = ss2;
                StarSet3.Background = ss3;
                StarSet4.Background = ss4;
                StarSet5.Background = ss5;
                screenHelpBtn.Background = sh;
                MainScreenHelp.Background = msh;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.ToString());
            }

            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0.0;
            fadeInAnimation.To = 1.0;
            fadeInAnimation.Duration = new Duration(TimeSpan.FromSeconds(3));
            fadeInAnimation.AutoReverse = true;
            fadeInAnimation.RepeatBehavior = RepeatBehavior.Forever;
            
            DoubleAnimation fadeInAnimation2 = new DoubleAnimation();
            fadeInAnimation2.From = 0.0;
            fadeInAnimation2.To = 1.0;
            fadeInAnimation2.Duration = new Duration(TimeSpan.FromSeconds(5));
            fadeInAnimation2.AutoReverse = true;
            fadeInAnimation2.RepeatBehavior = RepeatBehavior.Forever;

            DoubleAnimation fadeInAnimation3 = new DoubleAnimation();
            fadeInAnimation3.From = 1.0;
            fadeInAnimation3.To = 0.0;
            fadeInAnimation3.Duration = new Duration(TimeSpan.FromSeconds(4));
            fadeInAnimation3.AutoReverse = true;
            fadeInAnimation3.RepeatBehavior = RepeatBehavior.Forever;

            DoubleAnimation fadeInAnimation4 = new DoubleAnimation();
            fadeInAnimation4.From = 1.0;
            fadeInAnimation4.To = 0.0;
            fadeInAnimation4.Duration = new Duration(TimeSpan.FromSeconds(4));
            fadeInAnimation4.AutoReverse = true;
            fadeInAnimation4.RepeatBehavior = RepeatBehavior.Forever;

            DoubleAnimation fadeInAnimation5 = new DoubleAnimation();
            fadeInAnimation5.From = 0.0;
            fadeInAnimation5.To = 1.0;
            fadeInAnimation5.Duration = new Duration(TimeSpan.FromSeconds(6));
            fadeInAnimation5.AutoReverse = true;
            fadeInAnimation5.RepeatBehavior = RepeatBehavior.Forever;

            DoubleAnimation helpScreenAnimation = new DoubleAnimation();
            helpScreenAnimation.From = -1440;
            helpScreenAnimation.To = 0;
            helpScreenAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));

            DoubleAnimation helpScreenAnimation2 = new DoubleAnimation();
            helpScreenAnimation2.From = 0;
            helpScreenAnimation2.To = -1440;
            helpScreenAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            

            myStoryboard = new Storyboard();
            helpStoryboard = new Storyboard();
            helpStoryboard2 = new Storyboard();

            myStoryboard.Children.Add(fadeInAnimation);
            myStoryboard.Children.Add(fadeInAnimation2);
            myStoryboard.Children.Add(fadeInAnimation3);
            myStoryboard.Children.Add(fadeInAnimation4);
            myStoryboard.Children.Add(fadeInAnimation5);

            helpStoryboard.Children.Add(helpScreenAnimation);
            helpStoryboard2.Children.Add(helpScreenAnimation2);


            Storyboard.SetTargetName(fadeInAnimation, StarSet1.Name);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(fadeInAnimation2, StarSet2.Name);
            Storyboard.SetTargetProperty(fadeInAnimation2, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(fadeInAnimation3, StarSet3.Name);
            Storyboard.SetTargetProperty(fadeInAnimation3, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(fadeInAnimation4, StarSet4.Name);
            Storyboard.SetTargetProperty(fadeInAnimation4, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(fadeInAnimation5, StarSet5.Name);
            Storyboard.SetTargetProperty(fadeInAnimation5, new PropertyPath(Rectangle.OpacityProperty));

            Storyboard.SetTargetName(helpScreenAnimation, MainScreenHelp.Name);
            Storyboard.SetTargetProperty(helpScreenAnimation, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetName(helpScreenAnimation2, MainScreenHelp.Name);
            Storyboard.SetTargetProperty(helpScreenAnimation2, new PropertyPath(Canvas.LeftProperty));
        }
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void QuickGameButton_Click(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
            Switcher.Switch(new DifficultySelectionPage());
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
            Switcher.Switch(new OptionsPage());
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
            Switcher.Switch(new MultiplayerMode());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            soundEffectPlayer.ButtonSound();
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

        private void StarSet1_Loaded(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin(this);
        }

        private void StarSet2_Loaded(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin(this);
        }

        private void StarSet3_Loaded(object sender, RoutedEventArgs e)
        {
           myStoryboard.Begin(this);

        }

        private void StarSet4_Loaded(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin(this);

        }

        private void StarSet5_Loaded(object sender, RoutedEventArgs e)
        {
           myStoryboard.Begin(this);

        }

        private void screenHelpBtn_Click(object sender, RoutedEventArgs e)
        {
            helpStoryboard.Begin(MainScreenHelp);
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

        private void MainScreenHelp_Click(object sender, RoutedEventArgs e)
        {
            helpStoryboard2.Begin(MainScreenHelp);
        }
    }
}
