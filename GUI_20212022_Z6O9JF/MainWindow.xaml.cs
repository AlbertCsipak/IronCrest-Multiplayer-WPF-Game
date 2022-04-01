using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Uri unmutedUri = new Uri("Images/Other/unmuted.png", UriKind.RelativeOrAbsolute);
        public Uri mutedUri = new Uri("Images/Other/muted.png", UriKind.RelativeOrAbsolute);
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            
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
            music.Volume = volume/100;
            if (volume==0.0)
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
