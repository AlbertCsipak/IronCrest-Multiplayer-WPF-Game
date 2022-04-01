using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for ServerStartUC.xaml
    /// </summary>
    public partial class ServerUC : UserControl
    {
        IGameLogic gameLogic;
        public MediaPlayer button_click = new MediaPlayer();
        public ServerUC()
        {
            InitializeComponent();
            this.DataContext = new ServerViewModel();
            this.gameLogic = (this.DataContext as ServerViewModel).gameLogic;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
        }
    }
}
