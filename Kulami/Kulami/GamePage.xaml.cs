using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : UserControl, ISwitchable
    {
        private IEnumerable<Button> allButtons;
        private Dictionary<string, Button> buttonNames;
        private KulamiEngine engine;
        private EasyAI easyAI;
        private HardAI hardAI;
        bool player1turn = true;
        bool easyLevelAIOn = false;
        bool hardLevelAIOn = false;

        public GamePage()
        {
            InitializeComponent();

            buttonNames = new Dictionary<string,Button>();
            allButtons = GameBackground.Children.OfType<Button>();
            foreach (Button b in allButtons)
            {
                buttonNames.Add(b.Name.ToString(), b);
            }

            engine = new KulamiEngine();
            engine.StartGame();

            //replace this with boolean passed from the difficulty selection screen
            easyLevelAIOn = true;
            //

            Random rnd = new Random();
            int playFirst = rnd.Next(0, 2);

            if (playFirst == 1)
                player1turn = true;
            else
                player1turn = false;

            if (easyLevelAIOn)
                easyAI = new EasyAI(engine.CurrentGame);
            else
                hardAI = new HardAI(engine.CurrentGame);

            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(@"images\GameBoard" + engine.GameBoardNumber + ".png", UriKind.Relative));
            GameBackground.Background = ib;
            
            ImageBrush ButtonImage = new ImageBrush();
            ButtonImage.ImageSource = new BitmapImage(new Uri(@"images\GenericPlan.png", UriKind.Relative));
            ApplyBackgroundButtons(ButtonImage);

            if (!player1turn)
            {
                PlayerTurnLabel.Content = "Computer's Turn";
                MakeAIMove();
            }
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void ApplyBackgroundButtons(ImageBrush ib)
        {
            planet00.Background = planet01.Background = planet02.Background = planet03.Background = planet04.Background = planet05.Background = planet06.Background = planet07.Background =
            planet10.Background = planet11.Background = planet12.Background = planet13.Background = planet14.Background = planet15.Background = planet16.Background = planet17.Background =
            planet20.Background = planet21.Background = planet22.Background = planet23.Background = planet24.Background = planet25.Background = planet26.Background = planet27.Background =
            planet30.Background = planet31.Background = planet32.Background = planet33.Background = planet34.Background = planet35.Background = planet36.Background = planet37.Background =
            planet40.Background = planet41.Background = planet42.Background = planet43.Background = planet44.Background = planet45.Background = planet46.Background = planet47.Background =
            planet50.Background = planet51.Background = planet52.Background = planet53.Background = planet54.Background = planet55.Background = planet56.Background = planet57.Background =
            planet60.Background = planet61.Background = planet62.Background = planet63.Background = planet64.Background = planet65.Background = planet66.Background = planet67.Background =
            planet70.Background = planet71.Background = planet72.Background = planet73.Background = planet74.Background = planet75.Background = planet76.Background = planet77.Background =
            ib;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Scores("You Won!"));
        }

        private async void planetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!engine.CurrentGame.IsGameOver())
            {
                Button btn = sender as Button;
                string btnName = btn.Name.ToString();
                int row, col;
                row = Convert.ToInt32(btnName.Substring(6, 1));
                col = Convert.ToInt32(btnName.Substring(7, 1));
                Console.WriteLine(row + " " + col);
                if (engine.CurrentGame.IsValidMove(row, col) && player1turn)
                {
                    ImageBrush ButtonImage = new ImageBrush();
                    ButtonImage.ImageSource = new BitmapImage(new Uri(@"images\RedPlan1.png", UriKind.Relative));
                    btn.Background = ButtonImage;
                    engine.CurrentGame.Board.MakeMoveOnBoard("R" + row.ToString() + col.ToString());
                    engine.CurrentGame.Board.PrintGameBoard();
                    player1turn = !player1turn;
                    PlayerTurnLabel.Content = "Computer's Turn";

                    //get move from AI
                    MakeAIMove();
                }
            }
        }

        private async void MakeAIMove()
        {
            string aiMove = easyAI.GetMove();
            int aiRow = Convert.ToInt32(aiMove.Substring(1, 1));
            int aiCol = Convert.ToInt32(aiMove.Substring(2, 1));
            string aiMoveBtnName = "planet" + aiRow.ToString() + aiCol.ToString();
            Button aiMoveBtn = buttonNames[aiMoveBtnName];
            ImageBrush AIButtonImage = new ImageBrush();
            AIButtonImage.ImageSource = new BitmapImage(new Uri(@"images\WaterPlan1.png", UriKind.Relative));
            await Task.Delay(3000);
            aiMoveBtn.Background = AIButtonImage;
            engine.CurrentGame.Board.MakeMoveOnBoard(aiMove);
            PlayerTurnLabel.Content = "Your Turn";
            player1turn = !player1turn;
        }
    }
}
