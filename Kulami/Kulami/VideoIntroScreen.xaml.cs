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
    /// Interaction logic for VideoIntroScreen.xaml
    /// </summary>
    public partial class VideoIntroScreen : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public VideoIntroScreen()
        {
            InitializeComponent();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SkipButton.png", UriKind.Absolute));
            skipBtn.Background = ib;
            VideoControl.Source = new Uri(startupPath + "/images/SpaceBallsIntro.wmv", UriKind.Absolute);
            VideoControl.Play();
            
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void skipBtn_Click(object sender, RoutedEventArgs e)
        {
            VideoControl.Stop();
            Switcher.Switch(new MainPage());
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

        private void VideoControl_MediaEnded(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainPage());
        }

    }
}
