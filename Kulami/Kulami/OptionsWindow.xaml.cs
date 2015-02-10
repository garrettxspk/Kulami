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
using System.Windows.Shapes;

namespace Kulami
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow
    {
        public OptionsWindow()
        {
            InitializeComponent();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(@"images\OptionsScreen.png", UriKind.Relative));
            OptionsBackgrnd.Background = ib;
        }
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
    }
}
