using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MediaPlayer background_music = new MediaPlayer();
        public MediaPlayer background_ambient = new MediaPlayer();
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

            background_ambient.Open(new Uri("Resources/Music/ambient.mp3", UriKind.RelativeOrAbsolute));
            background_ambient.Volume = 0.025;
            background_ambient.Play();
            background_ambient.MediaEnded += Background_ambient_MediaEnded;

            background_music.Open(new Uri("Resources/Music/standard.mp3", UriKind.RelativeOrAbsolute));
            background_music.Play();
            background_music.MediaEnded += Background_music_MediaEnded;

        }

        private void Background_ambient_MediaEnded(object sender, EventArgs e)
        {
            background_ambient.Play();
        }

        private void Background_music_MediaEnded(object sender, EventArgs e)
        {
            background_music.Play();
        }

        private void volume_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double volume = volume_slider.Value;
            background_music.Volume = volume / 1000;
            if (volume == 0.0)
            {
                background_music.IsMuted = true; 
                img_mute.Source = new BitmapImage(mutedUri);
            }
            else
            {
                background_music.IsMuted = false; 
                img_mute.Source = new BitmapImage(unmutedUri);
            }
        }
    }
}
