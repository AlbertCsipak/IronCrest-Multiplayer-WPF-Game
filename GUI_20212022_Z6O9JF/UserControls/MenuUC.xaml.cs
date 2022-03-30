using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System.Windows.Controls;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for MenuUC.xaml
    /// </summary>
    public partial class MenuUC : UserControl
    {
        ContentControl cc;
        IGameLogic gameLogic;
        public MenuUC(ContentControl cc)
        {
            InitializeComponent();
            this.DataContext = new MenuViewModel();
            this.gameLogic = (this.DataContext as MenuViewModel).gameLogic;
            this.cc = cc;
        }
        public MenuUC()
        {

        }
    }
}
