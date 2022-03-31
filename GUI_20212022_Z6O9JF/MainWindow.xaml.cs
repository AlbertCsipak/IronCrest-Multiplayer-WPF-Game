using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            btn_mute.Background = new ImageBrush(new BitmapImage(new Uri("Images/Other/unmuted.png", UriKind.RelativeOrAbsolute)));
            music.Play();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (music.IsMuted)
            {
                music.IsMuted = false;
                music.Play();
                btn_mute.Background = new ImageBrush(new BitmapImage(new Uri("Images/Other/unmuted.png", UriKind.RelativeOrAbsolute)));
            }
            else
            {
                music.IsMuted = true;
                music.Pause();
                btn_mute.Background = new ImageBrush(new BitmapImage(new Uri("Images/Other/muted.png", UriKind.RelativeOrAbsolute)));
            }
        }
    }
}
