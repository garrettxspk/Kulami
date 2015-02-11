using System;
using System.Collections.Generic;
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
        public DifficultySelectionPage()
        {
            InitializeComponent();
            ImageBrush backgrnd = new ImageBrush();
            ImageBrush easyBtnBackgrnd = new ImageBrush();
            ImageBrush hardBtnBackgrnd = new ImageBrush();

            backgrnd.ImageSource = new BitmapImage(new Uri(@"images\SelectionPage.png", UriKind.Relative));
            easyBtnBackgrnd.ImageSource = new BitmapImage(new Uri(@"images\EasyLevelButton.png", UriKind.Relative));
            hardBtnBackgrnd.ImageSource = new BitmapImage(new Uri(@"images\HardLevelButton.png", UriKind.Relative));

            SelectionBackground.Background = backgrnd;
            EasyLevelButton.Background = easyBtnBackgrnd;
            HardLevelButton.Background = hardBtnBackgrnd;

        }

        private void EasyLevelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new GamePage(true));
        }

        private void HardLevelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new GamePage(false));
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
    }
}
