using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System.Windows.Controls;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for JoinGameUC.xaml
    /// </summary>
    public partial class JoinGameUC : UserControl
    {
        IGameLogic gameLogic;
        public JoinGameUC()
        {
            InitializeComponent();
            this.DataContext = new JoinGameViewModel();
            this.gameLogic = (this.DataContext as JoinGameViewModel).gameLogic;
            ;
        }
    }
}
