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
    public partial class TradeUC : UserControl
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        public MediaPlayer button_click = new MediaPlayer();
        public TradeUC()
        {
            InitializeComponent();
            this.DataContext = new TradeViewModel();
            this.gameLogic = (this.DataContext as TradeViewModel).gameLogic;
            this.clientLogic = (this.DataContext as TradeViewModel).clientLogic;
            this.controlLogic = (this.DataContext as TradeViewModel).controlLogic;

        }

        private void check1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
