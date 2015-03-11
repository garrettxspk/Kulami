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
    /// Interaction logic for DifficultySelectionPage.xaml
    /// </summary>
    public partial class DifficultySelectionPage : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public DifficultySelectionPage()
        {
            InitializeComponent();
            ImageBrush backgrnd = new ImageBrush();
            ImageBrush easyBtnBackgrnd = new ImageBrush();
            ImageBrush hardBtnBackgrnd = new ImageBrush();

            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            easyBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/EasyButton.png", UriKind.Absolute));
            hardBtnBackgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HardButton.png", UriKind.Absolute));

            SelectionBackground.Background = backgrnd;
            EasyLevelButton.Background = easyBtnBackgrnd;
            HardLevelButton.Background = hardBtnBackgrnd;

        }

        private void EasyLevelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new GamePage(true, GameType.LocalComputer));
        }

        private void HardLevelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new GamePage(false, GameType.LocalComputer));
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void EasyLevelButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush backgrnd = new ImageBrush();
            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/EasyButtonOn.png", UriKind.Absolute));
            EasyLevelButton.Background = backgrnd;

        }

        private void EasyLevelButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush backgrnd = new ImageBrush();
            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/EasyButton.png", UriKind.Absolute));
            EasyLevelButton.Background = backgrnd;
        }

        private void HardLevelButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageBrush backgrnd = new ImageBrush();
            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HardButtonOn.png", UriKind.Absolute));
            HardLevelButton.Background = backgrnd;
        }

        private void HardLevelButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageBrush backgrnd = new ImageBrush();
            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/HardButton.png", UriKind.Absolute));
            HardLevelButton.Background = backgrnd;
        }
    }
}
