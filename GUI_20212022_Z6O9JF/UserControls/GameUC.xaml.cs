using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

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
        DispatcherTimer dt;
        bool IsResourceChanged;
        Player player;
        public GameUC()
        {
            InitializeComponent();
            this.DataContext = new GameViewModel();
            this.gameLogic = (this.DataContext as GameViewModel).gameLogic;
            this.clientLogic = (this.DataContext as GameViewModel).clientLogic;
            this.controlLogic = (this.DataContext as GameViewModel).controlLogic;
            display.LogicSetup(clientLogic, gameLogic, controlLogic, grid);
            player = gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).FirstOrDefault();
            player.ResourceChanges.CollectionChanged += ResourceChanges_CollectionChanged;
            PopChange.Opacity = 0;
            ArmyPowerChange.Opacity = 0;
            WoodChange.Opacity = 0;
            StoneChange.Opacity = 0;
            FoodChange.Opacity = 0;
            GoldChange.Opacity = 0;
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);


            dt.Tick += (sender, eventargs) =>
            {
                ResourceChanging();
                OpacityDefault();
                if (clientLogic.Timer==60)
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(@"\Resources\Images\Menu\hourglassgif.gif", UriKind.Relative);
                    image.EndInit();
                    ImageBehavior.SetAnimatedSource(hourglass_gif, image);
                    ImageBehavior.SetRepeatBehavior(hourglass_gif, new RepeatBehavior(1));
                }
                display.InvalidateVisual();
            };
            dt.Start();
            
        }


        private void OpacityDefault()
        {
            //ResourceChanges
            double OpacityChanging = 0.05;
            if (PopChange.Opacity >= 0)
            {
                PopChange.Opacity -= OpacityChanging;
            }
            if (ArmyPowerChange.Opacity >= 0)
            {
                ArmyPowerChange.Opacity -= OpacityChanging;
            }
            if (WoodChange.Opacity >= 0)
            {
                WoodChange.Opacity -= OpacityChanging;
            }
            if (StoneChange.Opacity >= 0)
            {
                StoneChange.Opacity -= OpacityChanging;
            }
            if (FoodChange.Opacity >= 0)
            {
                FoodChange.Opacity -= OpacityChanging;
            }
            if (GoldChange.Opacity >= 0)
            {
                GoldChange.Opacity -= OpacityChanging;
            }
            //MissingResource Changes

        }

        private void ResourceChanging()
        {
            if (IsResourceChanged)
            {
                if (player.ResourceChanges[0] != 0)
                {
                    //POP
                    ;
                    PopChange.Opacity = 1;
                    string s = player.ResourceChanges[0] > 0 ? "+" : "";
                    PopChangeLabel.Content = s + player.ResourceChanges[0];
                    player.ResourceChanges[0] = 0;
                }
                if (player.ResourceChanges[1] != 0)
                {
                    //ArmyPower
                    ArmyPowerChange.Opacity = 1;
                    string s = player.ResourceChanges[1] > 0 ? "+" : "";
                    ArmyPowerChangeLabel.Content = s + player.ResourceChanges[1];
                    player.ResourceChanges[1] = 0;
                }
                if (player.ResourceChanges[2] != 0)
                {
                    //Wood
                    WoodChange.Opacity = 1;
                    string s = player.ResourceChanges[2] > 0 ? "+" : "";
                    WoodChangeLabel.Content = s + player.ResourceChanges[2];
                    player.ResourceChanges[2] = 0;
                }
                if (player.ResourceChanges[3] != 0)
                {
                    //Stone
                    StoneChange.Opacity = 1;
                    string s = player.ResourceChanges[3] > 0 ? "+" : "";
                    StoneChangeLabel.Content = s + player.ResourceChanges[3];
                    player.ResourceChanges[3] = 0;
                }
                if (player.ResourceChanges[4] != 0)
                {
                    //Food
                    FoodChange.Opacity = 1;
                    string s = player.ResourceChanges[4] > 0 ? "+" : "";
                    FoodChangeLabel.Content = s + player.ResourceChanges[4];
                    player.ResourceChanges[4] = 0;
                }
                if (player.ResourceChanges[5] != 0)
                {
                    //Gold
                    GoldChange.Opacity = 1;
                    string s = player.ResourceChanges[5] > 0 ? "+" : "";
                    GoldChangeLabel.Content = s + player.ResourceChanges[5];
                    player.ResourceChanges[5] = 0;
                }

            }

            
            //if (PopChange.Opacity <= 0)
            //{
            //    IsResourceChanged = false;
            //    PopChange.Opacity = 1;
            //}



            //if (!clientLogic.CanSend)
            //{
            //    skip_image.Visibility = Visibility.Hidden;
            //}
        }

        private void ResourceChanges_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsResourceChanged = true;
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
        }

        private void UserControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                clientLogic.ChangeView("ESC");
            }
        }
    }
}
