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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl, ISwitchable
    {
        public MainPage()
        {
            InitializeComponent();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(@"images\BackgroundMain.jpg", UriKind.Relative));
            MainBackground.Background = ib;
        }
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new GamePage());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new OptionsPage());
        }
    }
}
