using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for JoinGameUC.xaml
    /// </summary>
    public partial class LobbyUC : UserControl
    {
        public static event EventHandler StartOfGame;
        public MediaPlayer button_click = new MediaPlayer();
        public LobbyUC()
        {
            InitializeComponent();
            this.DataContext = new LobbyViewModel();

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
        }

        private void Button_LockIn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StartOfGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
