using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for ServerStartUC.xaml
    /// </summary>
    public partial class GameSubMenu : UserControl
    {
        public MediaPlayer button_click = new MediaPlayer();
        public GameSubMenu()
        {
            InitializeComponent();
            this.DataContext = new ServerViewModel();
        }

    }
}
