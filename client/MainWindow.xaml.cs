using System;
using System.Windows;
using System.Windows.Controls;

namespace client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, kamenServices.IkamenServiceCallback
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Play_Click(object sender, RoutedEventArgs e)
        {
            if (!isConnected)
                ConnectPlayer();
            else
                DisconnectPlayer();
        }

        void ConnectPlayer()
        {
            client = new kamenServices.IkamenServiceClient(new System.ServiceModel.InstanceContext(this));
            var gameInformation = client.Connect(myname.Text);
            _lobbyID = gameInformation.Item1;
            _myID = gameInformation.Item2;
            opname.Content = gameInformation.Item3;

            myname.IsEnabled = false;

            isConnected = true;
        }

        void DisconnectPlayer()
        {

            client.Disconnect(_lobbyID);
            client = null;

            myname.IsEnabled = true;

            isConnected = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isConnected)
                DisconnectPlayer();
        }

        private void MoveGame_Click(object sender, RoutedEventArgs e)
        {
            var button = ((Button)sender);

            int move = 0;
            switch (button.Name)
            {
                case "Rock":
                    move = 1;
                    break;
                case "Scissors":
                    move = 2;
                    break;
                case "Paper":
                    move = 3;
                    break;
                
            }
            client.GameMoveSend(move, _lobbyID, _myID);
            mySelect = move;
            GameResultAsync();

            Rock.IsEnabled = false;
            Scissors.IsEnabled = false;
            Paper.IsEnabled = false;
        }

        void GameResultAsync()
        {
            Dispatcher.BeginInvoke(new Action(() => GameResult()));
        }

        private void GameResult()
        {
            if (mySelect != 0 && opSelect != 0)
            {
                string result;
                if (mySelect == opSelect)
                    result = "Ничья";
                else if ((1 == mySelect && opSelect == 3) || (2 == mySelect && opSelect == 1) || (3 == mySelect && opSelect == 2))
                    result = "Победа";
                else
                    result = "Поражение";

                var window_GameResult = new GameResult(this, result);
                DisconnectPlayer();
                window_GameResult.ShowDialog();
            }
        }

        public void GameMoveReceive(int GameSelection)
        {
            opSelect = GameSelection;
            GameResultAsync();
            Rock.IsEnabled = true;
            Scissors.IsEnabled = true;
            Paper.IsEnabled = true;
        }
        public void StartGame(string OpponentName)
        {
            opname.Content = OpponentName;
            Rock.IsEnabled = true;
            Paper.IsEnabled = true;
            Scissors.IsEnabled = true;
        }

        private kamenServices.IkamenServiceClient client;
        public int opSelect = 0;
        public int mySelect = 0;
        private int _lobbyID;
        private int _myID;

        private bool isConnected = false;
    }
}
