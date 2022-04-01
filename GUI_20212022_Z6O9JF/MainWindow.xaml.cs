using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Uri unmutedUri = new Uri("Resources/Images/Other/unmuted.png", UriKind.RelativeOrAbsolute);
        public Uri mutedUri = new Uri("Resources/Images/Other/muted.png", UriKind.RelativeOrAbsolute);
        Cursor c1;
        public MainWindow()
        {

            InitializeComponent();
            c1 = new Cursor("Resources/blurite_sword.cur");
            grid.Cursor = c1;
            this.DataContext = new MainViewModel();
            img_mute.Source = new BitmapImage(unmutedUri);
            music.Play();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (music.IsMuted)
            {
                music.IsMuted = false;
                music.Play();
                img_mute.Source = new BitmapImage(mutedUri);
            }
            else
            {
                music.IsMuted = true;
                music.Pause();
                img_mute.Source = new BitmapImage(unmutedUri);
            }
        }

        private void volume_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double volume = volume_slider.Value;
            music.Volume = volume / 100;
            if (volume == 0.0)
            {
                music.IsMuted = true; img_mute.Source = new BitmapImage(mutedUri);
            }
            else
            {
                music.IsMuted = false; img_mute.Source = new BitmapImage(unmutedUri);
            }
        }
    }
}
