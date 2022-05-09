using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for ServerStartUC.xaml
    /// </summary>
    public partial class GameSubMenuUC : UserControl
    {
        IGameLogic gameLogic;
        IClientLogic clientLogic;
        Uri unmutedUri = new Uri(@"\Resources\Images\Other\unmuted.png", UriKind.RelativeOrAbsolute);
        Uri mutedUri = new Uri(@"\Resources\Images\Other\muted.png", UriKind.RelativeOrAbsolute);
        public MediaPlayer button_click = new MediaPlayer();
        public GameSubMenuUC()
        {
            InitializeComponent();
            this.DataContext = new GameSubMenuViewModel();
            this.clientLogic = (this.DataContext as GameSubMenuViewModel).clientLogic;
            this.gameLogic = (this.DataContext as GameSubMenuViewModel).gameLogic;
            img_mute.Source = new BitmapImage(unmutedUri);
        }
        private void volume_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double volume = volume_slider.Value;
            MainWindow.game_music.Volume = volume;
            if (volume == 0.0)
            {
                MainWindow.game_music.IsMuted = true;
                img_mute.Source = new BitmapImage(mutedUri);
            }
            else
            {
                MainWindow.game_music.IsMuted = false;
                img_mute.Source = new BitmapImage(unmutedUri);
            }
        }
        //private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    var window = Window.GetWindow(this);
        //}
    }
}
