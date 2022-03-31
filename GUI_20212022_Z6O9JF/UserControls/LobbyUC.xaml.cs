using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System.Windows.Controls;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for LobbyUC.xaml
    /// </summary>
    public partial class LobbyUC : UserControl
    {
        IGameLogic gameLogic;
        public LobbyUC()
        {
            InitializeComponent();
            this.DataContext = new LobbyViewModel();
            this.gameLogic = (this.DataContext as LobbyViewModel).gameLogic;
        }
    }
}
