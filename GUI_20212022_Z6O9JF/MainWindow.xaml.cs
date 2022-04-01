using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MediaElement background_music;
        public MediaElement button_clink_sound;
        SoundPlayer player = new SoundPlayer("file.wav");
        public Uri unmutedUri = new Uri("Resources/Images/Other/unmuted.png", UriKind.RelativeOrAbsolute);
        public Uri mutedUri = new Uri("Resources/Images/Other/muted.png", UriKind.RelativeOrAbsolute);
        Cursor c1;

        public MainWindow()
        {
            background_music = new MediaElement();
            button_clink_sound = new MediaElement();
            InitializeComponent();
            c1 = new Cursor("Resources/blurite_sword.cur");
            grid.Cursor = c1;
            this.DataContext = new MainViewModel();
            img_mute.Source = new BitmapImage(unmutedUri);
            
            button_clink_sound.Source = new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute);
            
            background_music.Source = new Uri("Resources/Music/standard.mp3", UriKind.RelativeOrAbsolute);
            background_music.LoadedBehavior = MediaState.Manual;
            button_clink_sound.LoadedBehavior = MediaState.Manual;
            background_music.Play();

        }

        private void volume_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double volume = volume_slider.Value;
            background_music.Volume = volume / 100;
            if (volume == 0.0)
            {
                background_music.IsMuted = true; img_mute.Source = new BitmapImage(mutedUri);
            }
            else
            {
                background_music.IsMuted = false; img_mute.Source = new BitmapImage(unmutedUri);
            }
        }
    }
}
