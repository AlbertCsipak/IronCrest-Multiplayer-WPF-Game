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
        public MediaPlayer button_click = new MediaPlayer();
        public LobbyUC()
        {
            InitializeComponent();
            this.DataContext = new LobbyViewModel();
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
        }
    }
}
