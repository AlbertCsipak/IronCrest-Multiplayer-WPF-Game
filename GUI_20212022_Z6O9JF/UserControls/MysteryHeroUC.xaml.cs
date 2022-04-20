using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for MysteryUC.xaml
    /// </summary>
    public partial class MysteryHeroUC : UserControl
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        public MediaPlayer button_click = new MediaPlayer();
        public MysteryHeroUC()
        {
            InitializeComponent();
            this.DataContext = new MysteryHeroViewModel();
            this.gameLogic = (this.DataContext as MysteryHeroViewModel).gameLogic;
            this.clientLogic = (this.DataContext as MysteryHeroViewModel).clientLogic;
            this.controlLogic = (this.DataContext as MysteryHeroViewModel).controlLogic;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().
        }
    }
}
