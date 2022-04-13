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
    /// Interaction logic for MysteryUC.xaml
    /// </summary>
    public partial class MysteryUC : UserControl
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        public MediaPlayer button_click = new MediaPlayer();
        public MysteryUC()
        {
            InitializeComponent();
            this.DataContext = new MysteryViewModel();
            this.gameLogic = (this.DataContext as MysteryViewModel).gameLogic;
            this.clientLogic = (this.DataContext as MysteryViewModel).clientLogic;
            this.controlLogic = (this.DataContext as MysteryViewModel).controlLogic;

        }

    }
}
