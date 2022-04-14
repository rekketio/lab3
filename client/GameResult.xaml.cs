using System.Windows;

namespace client
{
    /// <summary>
    /// Логика взаимодействия для GameResult.xaml
    /// </summary>
    public partial class GameResult : Window
    {
        public GameResult(MainWindow window_MainWindow, string result)
        {
            InitializeComponent();
            this.Result.Content = result;
            this.window_MainWindow = window_MainWindow;
        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
            window_MainWindow.Play_Click(sender, e);
            window_MainWindow.mySelect = 0;
            window_MainWindow.opSelect = 0;

            window_MainWindow.Rock.IsEnabled = false;
            window_MainWindow.Scissors.IsEnabled = false;
            window_MainWindow.Paper.IsEnabled = false;
            this.Close();
        }
        MainWindow window_MainWindow;
    }
}
