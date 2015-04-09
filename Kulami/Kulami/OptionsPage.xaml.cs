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
    /// Interaction logic for OptionsPage.xaml
    /// </summary>
    public partial class OptionsPage : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public OptionsPage()
        {
            InitializeComponent();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            OptionsBackground.Background = ib;

            ImageBrush bb = new ImageBrush();
            bb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/backButton.png", UriKind.Absolute));
            BackButton.Background = bb;

            ImageBrush pb = new ImageBrush();
            pb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/playButton.png", UriKind.Absolute));
            PlayButton.Background = pb;

            ImageBrush sound = new ImageBrush();
            if (SoundSetting.SoundOn)
                sound.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButton.png", UriKind.Absolute));
            else
                sound.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButton.png", UriKind.Absolute));
            SoundEffect.Background = sound;

            ImageBrush music = new ImageBrush();
            if(SoundSetting.MusicOn)
                music.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOnButton.png", UriKind.Absolute));
            else
                music.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButton.png", UriKind.Absolute));
            Music.Background = music;

        }
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainPage());
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

        private void SoundEffectsOn_Click(object sender, RoutedEventArgs e)
        {
            SoundSetting.SoundOn = !SoundSetting.SoundOn;
            ImageBrush sb = new ImageBrush();
            sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButtonHover.png", UriKind.Absolute));
            if (SoundSetting.SoundOn)
            {
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButtonHover.png", UriKind.Absolute));
            }
            else
            {
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButtonHover.png", UriKind.Absolute));
            }
            SoundEffect.Background = sb;
        }

        private void SoundEffectsOn_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush sb = new ImageBrush();
            if (SoundSetting.SoundOn)
            {
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButtonHover.png", UriKind.Absolute));
            }
            else
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButtonHover.png", UriKind.Absolute));

            SoundEffect.Background = sb;
        }

        private void SoundEffectsOn_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush sb = new ImageBrush();
            if (SoundSetting.SoundOn)
            {
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOnButton.png", UriKind.Absolute));
            }
            else
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/soundOffButton.png", UriKind.Absolute));

            SoundEffect.Background = sb;
        }

        private void MusicOn_Click(object sender, RoutedEventArgs e)
        {
            SoundSetting.MusicOn = !SoundSetting.MusicOn;

            ImageBrush mb = new ImageBrush();
            mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButtonHover.png", UriKind.Absolute));
            if (SoundSetting.MusicOn)
            {
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOnButtonHover.png", UriKind.Absolute));
            }
            else
            {
                mb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButtonHover.png", UriKind.Absolute));
            }

            Music.Background = mb;
        }

        private void MusicOn_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush sb = new ImageBrush();
            if (SoundSetting.MusicOn)
            {
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOnButtonHover.png", UriKind.Absolute));
            }
            else
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButtonHover.png", UriKind.Absolute));

            Music.Background = sb;
        }

        private void MusicOn_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush sb = new ImageBrush();
            if (SoundSetting.MusicOn)
            {
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOnButton.png", UriKind.Absolute));
            }
            else
                sb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/musicOffButton.png", UriKind.Absolute));

            Music.Background = sb;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new VideoIntroScreen());
        }

        private void PlayButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush pb = new ImageBrush();
            pb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/playButtonHover.png", UriKind.Absolute));
            PlayButton.Background = pb;
        }

        private void PlayButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush pb = new ImageBrush();
            pb.ImageSource = new BitmapImage(new Uri(startupPath + "/images/playButton.png", UriKind.Absolute));
            PlayButton.Background = pb;
        }
    }
}
