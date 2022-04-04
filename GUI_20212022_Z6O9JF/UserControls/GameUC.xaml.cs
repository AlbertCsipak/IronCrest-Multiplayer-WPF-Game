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
    /// Interaction logic for GameUC.xaml
    /// </summary>
    public partial class GameUC : UserControl
    {
        IGameLogic gameLogic;
        public GameUC()
        {
            InitializeComponent();
            this.DataContext = new GameViewModel();
            this.gameLogic = (this.DataContext as GameViewModel).gameLogic;
            display.LogicSetup(gameLogic, grid);
            InvalidateVisual();
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
            display.InvalidateVisual();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
            display.InvalidateVisual();
        }
    }
}
