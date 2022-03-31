using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System.Windows.Controls;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for ChampSelectUC.xaml
    /// </summary>
    public partial class ChampSelectUC : UserControl
    {
        IGameLogic gameLogic;
        public ChampSelectUC()
        {
            InitializeComponent();
            this.DataContext = new ChampSelectViewModel();
            this.gameLogic = (this.DataContext as ChampSelectViewModel).gameLogic;
        }
    }
}
