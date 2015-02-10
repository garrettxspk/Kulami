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
    /// Interaction logic for Scores.xaml
    /// </summary>
    public partial class Scores : UserControl
    {
        private String gameResult;
        public Scores(String result)
        {
            InitializeComponent();
            gameResult = result;
            resultLabel.Content = gameResult;
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(@"images\ScoresBackground.png", UriKind.Relative));
            ScoresBackground.Background = ib;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainPage());
        }
    }
}
