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
    /// Interaction logic for GetMoveFromUser.xaml
    /// </summary>
    public partial class GetMoveFromUser : Window
    {
        private string x;
        public string X
        {
            get { return xTxtBox.Text; }
            set { x = value; }
        }

        private string y;
        public string Y
        {
            get { return yTxtBox.Text; }
            set { y = value; }
        }

        public GetMoveFromUser()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }


    }
}
