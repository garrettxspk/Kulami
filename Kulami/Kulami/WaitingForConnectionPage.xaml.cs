﻿using System;
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
    /// Interaction logic for WaitingForConnectionPage.xaml
    /// </summary>
    public partial class WaitingForConnectionPage : UserControl, ISwitchable
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public WaitingForConnectionPage()
        {
            InitializeComponent();
            ImageBrush backgrnd = new ImageBrush();
            backgrnd.ImageSource = new BitmapImage(new Uri(startupPath + "/images/SelectionPage.png", UriKind.Absolute));
            Background.Background = backgrnd;

            Task.Delay(6000);
            
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
    }
}
