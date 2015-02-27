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
using System.IO;

namespace Kulami
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainMenu
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public MainMenu()
        {
            try
            {
                InitializeComponent();
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri(startupPath + "/images/BackgroundMain.jpg", UriKind.Absolute));
                Backgrnd.Background = ib;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.ToString());
            }
        }

        private void Button_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }
        public void UtilizeState (object state)
        {
            throw new NotImplementedException();
        }
    }
}
