using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for GameEndUC.xaml
    /// </summary>
    public partial class GameEndUC : UserControl
    {
        public MediaPlayer button_click = new MediaPlayer();
        public GameEndUC()
        {
            InitializeComponent();
        }

        private void Button_Exit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
            System.Threading.Thread.Sleep(300);
            System.Windows.Application.Current.Shutdown();
        }
    }
}
