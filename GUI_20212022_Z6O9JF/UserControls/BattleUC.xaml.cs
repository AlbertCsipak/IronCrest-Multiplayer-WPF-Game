using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Renderer;
using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for BattleUC.xaml
    /// </summary>
    public partial class BattleUC : UserControl
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        DispatcherTimer dt_counter;
        DispatcherTimer dt_movement;
        MediaPlayer counterSoundEffect = new MediaPlayer();
        MediaPlayer buttonSoundEffect = new MediaPlayer();
        MediaPlayer battle_result_sound = new MediaPlayer();
        int counter = 4;
        public BattleUC()
        {
            InitializeComponent();
            BattleDisplay.Explosion += ExplosionGif;
            this.DataContext = new BattleViewModel();
            this.gameLogic = (this.DataContext as BattleViewModel).gameLogic;
            this.clientLogic = (this.DataContext as BattleViewModel).clientLogic;
            this.controlLogic = (this.DataContext as BattleViewModel).controlLogic;
            battleDisplay.LogicSetup(clientLogic, gameLogic, controlLogic, grid);
            if (gameLogic.Game.CurrentBattle.Attacker.PlayerID == gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().PlayerID)
            {
                lbl_counter.Visibility = Visibility.Hidden;
                lbl_result.Visibility = Visibility.Hidden;
            }
            if (gameLogic.Game.CurrentBattle.Defender.PlayerID == gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().PlayerID)
            {
                DefenderView();
                dt_counter = new DispatcherTimer();
                dt_counter.Interval = TimeSpan.FromMilliseconds(100);
                int i = 0;
                dt_counter.Tick += (sender, eventargs) =>
                {
                    if (gameLogic.Game.CurrentBattle.IsBattleStarted)
                    {
                        battleDisplay.InvalidateVisual();
                    }
                };
                dt_counter.Start();
            }

        }

        public void ExplosionGif(object sender, EventArgs e)
        {
            if (BattleDisplay.IsExplosionCalled)
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(@"\Resources\Images\Menu\explosion.gif", UriKind.Relative);
                image.EndInit();
                ImageBehavior.SetAnimatedSource(img_explosion, image);
                var player = gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault();
                if (player.PlayerID==gameLogic.Game.CurrentBattle.Winner.PlayerID)
                {
                    btn_ready.Visibility = Visibility.Hidden;
                    lbl_result.Content = "Victory!";
                    lbl_result.Visibility = Visibility.Visible;
                    battle_result_sound.Open(new Uri("Resources/Music/victory.mp3", UriKind.RelativeOrAbsolute));
                    battle_result_sound.Play();
                }
                else if (player.PlayerID == gameLogic.Game.CurrentBattle.Loser.PlayerID)
                {
                    btn_ready.Visibility = Visibility.Hidden;
                    lbl_result.Content = "Defeat!";
                    lbl_result.Visibility = Visibility.Visible;
                    battle_result_sound.Open(new Uri("Resources/Music/defeat.mp3", UriKind.RelativeOrAbsolute));
                    battle_result_sound.Play();
                }
                
                //clientLogic.BattleViewChange("");
            }
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            buttonSoundEffect.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            buttonSoundEffect.Play();
        }

        public void DefenderView()
        {
            img_board.Visibility = Visibility.Hidden;
            btn_left.Visibility = Visibility.Hidden;
            btn_right.Visibility = Visibility.Hidden;
            btn_ready.Visibility = Visibility.Hidden;
            lbl_counter.Visibility = Visibility.Hidden;
            img_armypowernum.Visibility = Visibility.Hidden;
            lbl_result.Visibility = Visibility.Hidden;
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
            dt_counter = new DispatcherTimer();
            //dt_movement = new DispatcherTimer();
            dt_counter.Interval = TimeSpan.FromMilliseconds(100);
            int i = 0;
            dt_counter.Tick += (sender, eventargs) =>
            {
                if (counter > 0)
                {
                    if (i%10==0)
                    {
                        lbl_counter.Content = --counter;
                        buttonSoundEffect.Open(new Uri("Resources/Music/beep.mp3", UriKind.RelativeOrAbsolute));
                        buttonSoundEffect.Play();
                    }
                }
                else
                {
                    battleDisplay.InvalidateVisual();
                    if (!lbl_counter.Content.Equals("BATTLE"))
                    {
                        lbl_counter.Content = "BATTLE!";
                    }
                }
                i++;

            };
            dt_counter.Start();
            
            //if (counter == 0)
            //{
            //    dt_counter.Stop();
            //    dt_movement.Interval = TimeSpan.FromMilliseconds(33);
            //    dt_movement.Tick += (sender, eventargs) =>
            //    {
                    
            //    };
            //    dt_movement.Start();

            //}

            
        }

        private void UserControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            battleDisplay.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
        }
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            battleDisplay.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
        }

        //private void UserControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        //{
        //    battleDisplay.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
        //}
    }
}
