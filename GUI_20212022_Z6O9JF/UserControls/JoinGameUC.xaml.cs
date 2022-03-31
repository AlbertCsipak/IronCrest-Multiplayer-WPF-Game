using GUI_20212022_Z6O9JF.Logic;
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
        }
    }
}
