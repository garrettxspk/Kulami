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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KulamiEngine engine = new KulamiEngine();
        bool player1turn = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            engine.StartGame();
            engine.CurrentGame.PrintGameBoard();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!engine.CurrentGame.IsGameOver())
            {
                string move = "";
                bool madeMoveSuccessfully = false;
                if (player1turn)
                {
                    GetMoveFromUser getMove = new GetMoveFromUser();
                    if (getMove.ShowDialog() == true)
                        move += "R" + getMove.X + getMove.Y;
                    engine.CurrentGame.MakeMoveOnBoard(move);
                    Gameboard copy = engine.CurrentGame.GetCopyOfGameBoard();
                }
                else
                {
                    while (!madeMoveSuccessfully)
                    {
                        move = "";
                        Random rnd = new Random();

                        move += "B";

                        int x = rnd.Next(0, 8);
                        int y = rnd.Next(0, 8);

                        move += x.ToString() + y.ToString();

                        madeMoveSuccessfully = engine.CurrentGame.MakeMoveOnBoard(move);
                    }
                }
                engine.CurrentGame.PrintGameBoard();
                player1turn = !player1turn;
            }
            else
            {
                engine.CurrentGame.GetPoint();
            }
        }
    }
}
