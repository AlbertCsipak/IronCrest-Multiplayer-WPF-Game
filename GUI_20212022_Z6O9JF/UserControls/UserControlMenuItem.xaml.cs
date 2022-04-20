using GUI_20212022_Z6O9JF.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace GUI_20212022_Z6O9JF
{
    /// <summary>
    /// Interaction logic for UserControlMenuItem.xaml
    /// </summary>
    public partial class UserControlMenuItem : UserControl
    {
        //IClientLogic clientLogic;
        //IGameLogic gameLogic;
        //IControlLogic controlLogic;
        //Player player;
        public UserControlMenuItem(ItemMenu itemMenu)
        {
            InitializeComponent();
            //player = gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).FirstOrDefault();
            //this.gameLogic = (this.DataContext as GameViewModel).gameLogic;
            //this.clientLogic = (this.DataContext as GameViewModel).clientLogic;
            //this.controlLogic = (this.DataContext as GameViewModel).controlLogic;
            //if (player.Quests.ElementAt(0).Done)
            //{

            //}
            ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = itemMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;
            this.DataContext = itemMenu;
        }
    }
}
