using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System.Windows.Controls;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for ServerStartUC.xaml
    /// </summary>
    public partial class ServerStartUC : UserControl
    {
        IGameLogic gameLogic;
        public ServerStartUC()
        {
            InitializeComponent();
            this.DataContext = new ServerStartViewModel();
            this.gameLogic = (this.DataContext as ServerStartViewModel).gameLogic;
        }

        private void btn_create_game_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            btn_create_game.IsEnabled = false;
        }
        private void btn_load_game_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void btn_stop_game_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            btn_create_game.IsEnabled = true;
                //szerver leállítása;
        }

        
    }
}
