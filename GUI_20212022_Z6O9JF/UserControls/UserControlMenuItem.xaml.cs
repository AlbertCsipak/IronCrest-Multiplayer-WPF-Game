using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using GUI_20212022_Z6O9JF.ViewModel;
using GUI_20212022_Z6O9JF.ViewModels;
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
