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
    /// Interaction logic for OptionsPage.xaml
    /// </summary>
    public partial class OptionsPage : UserControl, ISwitchable
    {
        public OptionsPage()
        {
            InitializeComponent();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(@"images\OptionsScreen.png", UriKind.Relative));
            OptionsBackground.Background = ib;
        }
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainPage());
        }
    }
}
