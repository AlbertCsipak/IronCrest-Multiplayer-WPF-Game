using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System.Windows.Controls;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for ServerStartUC.xaml
    /// </summary>
    public partial class ServerUC : UserControl
    {
        IGameLogic gameLogic;
        public ServerUC()
        {
            InitializeComponent();
            this.DataContext = new ServerViewModel();
            this.gameLogic = (this.DataContext as ServerViewModel).gameLogic;
        }
    }
}
