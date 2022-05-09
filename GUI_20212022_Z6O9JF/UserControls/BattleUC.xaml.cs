using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
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
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        DispatcherTimer dt_counter;
        DispatcherTimer dt_movement;
        MediaPlayer counterSoundEffect = new MediaPlayer();
        MediaPlayer buttonSoundEffect = new MediaPlayer();
        int counter = 4;
        public BattleUC()
        {
            InitializeComponent();
            this.DataContext = new BattleViewModel();
            this.gameLogic = (this.DataContext as BattleViewModel).gameLogic;
            this.clientLogic = (this.DataContext as BattleViewModel).clientLogic;
            this.controlLogic = (this.DataContext as BattleViewModel).controlLogic;
            battleDisplay.LogicSetup(clientLogic, gameLogic, controlLogic, grid);
            if (gameLogic.Game.CurrentBattle.Attacker == gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault())
            {
                lbl_counter.Visibility = System.Windows.Visibility.Hidden;
            }
            if (gameLogic.Game.CurrentBattle.Defender == gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault())
            {
                DefenderView();
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
            btn_ready.Visibility = Visibility.Hidden;
            btn_ready.Visibility = Visibility.Hidden;
            lbl_counter.Visibility = Visibility.Hidden;
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
