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
        private HardAI hardAI;
        bool player1turn = true;
        bool easyLevelAIOn = false;
        bool hardLevelAIOn = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            engine.StartGame();
            Console.WriteLine("Would you like to make the first move? (Y/N)");
            string moveFirst = Console.ReadLine();
            if (moveFirst[0] == 'Y' || moveFirst[0] == 'y')
                player1turn = true;
            else
                player1turn = false;
            Console.WriteLine("Play against easy (e) or hard (h) computer?");
            string aiLevel = Console.ReadLine();
            if (aiLevel[0] == 'e' || aiLevel[0] == 'E')
            {
                easyLevelAIOn = true;
                easyAI = new EasyAI(engine.CurrentGame);
            }
            else if (aiLevel[0] == 'h' || aiLevel[0] == 'H')
            {
                hardLevelAIOn = true;
                hardAI = new HardAI(engine.CurrentGame);
            }
            engine.CurrentGame.Board.PrintGameBoard();
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
                        move += "R" + getMove.Row + getMove.Col;
                    engine.CurrentGame.Board.MakeMoveOnBoard(move);
                    Gameboard copy = engine.CurrentGame.GetCopyOfGameBoard();
                }
                else
                {
                    if (easyLevelAIOn)
                    {
                        string AIMove = easyAI.GetMove();
                        engine.CurrentGame.Board.MakeMoveOnBoard(AIMove);
                    }
                    else
                    {
                        string blahblahblah = hardAI.GetMove();
                    }

                }
                engine.CurrentGame.Board.PrintGameBoard();
                player1turn = !player1turn;
            }
            else
            {
                engine.CurrentGame.GetPoint();
            }
        }
    }
}
