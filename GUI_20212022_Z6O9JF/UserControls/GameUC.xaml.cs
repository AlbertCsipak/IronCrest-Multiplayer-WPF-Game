using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for GameUC.xaml
    /// </summary>
    public partial class GameUC : UserControl
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        public MediaPlayer button_click = new MediaPlayer();
        private int currentIndex = 1;
        DispatcherTimer dt;
        public GameUC()
        {
            InitializeComponent();
            this.DataContext = new GameViewModel();
            this.gameLogic = (this.DataContext as GameViewModel).gameLogic;
            this.clientLogic = (this.DataContext as GameViewModel).clientLogic;
            this.controlLogic = (this.DataContext as GameViewModel).controlLogic;
            display.LogicSetup(clientLogic, gameLogic, controlLogic, grid);
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(100);
            dt.Tick += new EventHandler(this.updateImageTimer_Tick);
            dt.Tick += (sender, eventargs) =>
            {
                display.InvalidateVisual();
            };
            dt.Start();
        }

        private void updateImageTimer_Tick(object sender, EventArgs e)
        {
            if (currentIndex == 1)
            {
                skip_image.Source = new BitmapImage(new Uri("Resources/Image/Menu/skip_framed_button_pressed.png", UriKind.Relative));
                currentIndex++;
            }
            else if (currentIndex == 2)
            {
                skip_image.Source = new BitmapImage(new Uri("Resources/Image/Menu/skip_framed_button_unpressed.png", UriKind.Relative));
                currentIndex--;
            }

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
            //skip_image_pressed.Visibility = Visibility.Visible;
            //skip_image_unpressed.Visibility = Visibility.Hidden;
            //skip_image_unpressed.Visibility = Visibility.Visible;
            //skip_image_unpressed.Visibility = Visibility.Visible;
            //skip_image_unpressed.Source = new BitmapImage(new Uri("Resources/Images/Menu/skip_framed_button_pressed.png", UriKind.RelativeOrAbsolute));
            //skip_image.Source = new BitmapImage(new Uri("Resources/Images/Menu/skip_framed_button_unpressed.png", UriKind.RelativeOrAbsolute));
        }

        //private void Button_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    Thread.Sleep(5000);
        //    skip_image_unpressed.Visibility = Visibility.Visible;
        //    skip_image_pressed.Visibility = Visibility.Hidden;
        //}
    }
}
