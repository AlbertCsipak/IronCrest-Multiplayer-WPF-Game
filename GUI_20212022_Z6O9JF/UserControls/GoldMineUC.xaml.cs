using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for GoldMineUC.xaml
    /// </summary>
    public partial class GoldMineUC : UserControl
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;

        DispatcherTimer dt;
        MediaPlayer music = new MediaPlayer();
        public GoldMineUC()
        {
            InitializeComponent();
            this.DataContext = new GoldMineViewModel();
            this.gameLogic = (this.DataContext as GoldMineViewModel).gameLogic;
            this.clientLogic = (this.DataContext as GoldMineViewModel).clientLogic;
            this.controlLogic = (this.DataContext as GoldMineViewModel).controlLogic;
            goldmineDisplay.LogicSetup(clientLogic, gameLogic, controlLogic, grid);
            //music.Open(new Uri(Path.Combine("Resources", "Music", "money_money_money.mp3"), UriKind.RelativeOrAbsolute));
            //music.Volume = 0.3;
            //music.Play();
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(60);
            dt.Tick += (sender, eventargs) =>
            {
                goldmineDisplay.InvalidateVisual();
            };
            dt.Start();
            goldmineDisplay.InvalidateVisual();
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            goldmineDisplay.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            goldmineDisplay.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
        }
    }
}
