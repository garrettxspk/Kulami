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
        private EasyAI easyAI;
        bool player1turn = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            engine.StartGame();
            Console.WriteLine("Would you like to make the first move? (Y/N)");
            easyAI = new EasyAI(engine.CurrentGame);
            string moveFirst = Console.ReadLine();
            if (moveFirst[0] == 'Y' || moveFirst[0] == 'y')
                player1turn = true;
            else
                player1turn = false;
            engine.CurrentGame.PrintGameBoard();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!engine.CurrentGame.IsGameOver())
            {
                string move = "";
                //bool madeMoveSuccessfully = false;
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
                    string AIMove = easyAI.GetMove();
                    engine.CurrentGame.MakeMoveOnBoard(AIMove);

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
