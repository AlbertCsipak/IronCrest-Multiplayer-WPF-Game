using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for BattleUC.xaml
    /// </summary>
    public partial class BattleUC : UserControl
    {
        DispatcherTimer dt;
        MediaPlayer counterSoundEffect = new MediaPlayer();
        MediaPlayer buttonSoundEffect = new MediaPlayer();
        int counter = 4;
        public BattleUC()
        {
            InitializeComponent();
            lbl_counter.Visibility = System.Windows.Visibility.Hidden;

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            buttonSoundEffect.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            buttonSoundEffect.Play();
        }
        private void Ready_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            buttonSoundEffect.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            buttonSoundEffect.Play();
            img_armypowernum.Visibility = System.Windows.Visibility.Hidden;
            img_board.Visibility = System.Windows.Visibility.Hidden;
            btn_left.Visibility = System.Windows.Visibility.Hidden;
            btn_right.Visibility = System.Windows.Visibility.Hidden;
            btn_ready.Visibility = System.Windows.Visibility.Hidden;
            lbl_counter.Visibility = System.Windows.Visibility.Visible;
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(1000);
            dt.Tick += (sender, eventargs) =>
            {
                if (counter>0)
                {
                    lbl_counter.Content = --counter;
                    buttonSoundEffect.Open(new Uri("Resources/Music/beep.mp3", UriKind.RelativeOrAbsolute));
                    buttonSoundEffect.Play();
                }
                if (counter==0)
                {
                    lbl_counter.Content = "BATTLE!";
                }
            };
            dt.Start();
            if (counter==0)
            {
                dt.Stop();
            }

        }
        
    }
}
