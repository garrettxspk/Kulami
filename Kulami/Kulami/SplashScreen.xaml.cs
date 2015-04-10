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
using System.Threading.Tasks;

namespace Kulami
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public SplashScreen()
        {
            InitializeComponent();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SkipButton.png", UriKind.Absolute));
            skipBtn.Background = ib;

            ImageBrush ss = new ImageBrush();
            ss.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SpaceBallsHome.png", UriKind.Absolute));
            SplashScreenCanvas.Background = ss;

            NextScreen();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private async void NextScreen()
        {
            await Task.Delay(2000);
            skipBtn.IsEnabled = false;
            Switcher.Switch(new VideoIntroScreen());
        }

        private void skipBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new VideoIntroScreen());
        }

        private void skipBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush qg = new ImageBrush();
            qg.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SkipButtonHover.png", UriKind.Absolute));
            skipBtn.Background = qg;
        }

        private void skipBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush qg = new ImageBrush();
            qg.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SkipButton.png", UriKind.Absolute));
            skipBtn.Background = qg;
        }
    }
}
