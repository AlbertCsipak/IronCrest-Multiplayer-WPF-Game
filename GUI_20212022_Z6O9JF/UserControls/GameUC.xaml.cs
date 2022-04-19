using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using GUI_20212022_Z6O9JF.ViewModel;
using GUI_20212022_Z6O9JF.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        List<SubItem<Quest>> quests;
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
            HeartChange.Opacity = 0;
            HeartChangeLabel.Opacity = 0;
            ArmyPowerChange.Opacity = 0;
            ArmyPowerChangeLabel.Opacity = 0;
            WoodChange.Opacity = 0;
            WoodChangeLabel.Opacity = 0;
            StoneChange.Opacity = 0;
            StoneChangeLabel.Opacity = 0;
            FoodChange.Opacity = 0;
            FoodChangeLabel.Opacity = 0;
            GoldChange.Opacity = 0;
            GoldChangeLabel.Opacity = 0;
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);
            quests = new List<SubItem<Quest>>();
            quests.Add(new SubItem<Quest>(player.Quests.ElementAt(0)));
            quests.Add(new SubItem<Quest>(player.Quests.ElementAt(1)));
            quests.Add(new SubItem<Quest>(player.Quests.ElementAt(2)));
            var itemQuest = new ItemMenu("Quests", quests, PackIconKind.ViewDashboard);
            Menu.Children.Add(new UserControlMenuItem(itemQuest));

            dt.Tick += (sender, eventargs) =>
            {

                SetMovePictures();
                gameLogic.IsQuestDone();
                clientLogic.IsAllQuestsDone();
                ResourceChanging();
                OpacityDefault();
                if (clientLogic.Timer == 60)
                {
                    var bc = new BrushConverter();
                    btn_build.IsEnabled = true;
                    txt_build.Foreground = (Brush)bc.ConvertFrom("#B3C8B7");
                    btn_harvest.IsEnabled = true;
                    txt_harvest.Foreground = (Brush)bc.ConvertFrom("#B3C8B7");
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
        public void SetMovePictures()
        {
            if (player.DefaultNumOfMoves==2)
            {
                switch (player.RemainingMoves)
                {
                    case 0:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 1:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 2:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    default:
                        break;
                }
            }
            else if (player.DefaultNumOfMoves == 3)
            {
                switch (player.RemainingMoves)
                {
                    case 0:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 1:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 2:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 3:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    default:
                        break;
                }
            }
            else if (player.DefaultNumOfMoves == 4)
            {
                switch (player.RemainingMoves)
                {
                    case 0:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move4.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 1:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move4.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 2:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move4.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 3:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move4.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 4:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move4.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    default:
                        break;
                }
            }
        }
        private void OpacityDefault()
        {
            //ResourceChanges
            double OpacityChanging = 0.05;
            if (HeartChange.Opacity >= 0)
            {
                HeartChange.Opacity -= OpacityChanging;
                HeartChangeLabel.Opacity -= OpacityChanging;
            }
            if (ArmyPowerChange.Opacity >= 0)
            {
                ArmyPowerChange.Opacity -= OpacityChanging;
                ArmyPowerChangeLabel.Opacity -= OpacityChanging;
            }
            if (WoodChange.Opacity >= 0)
            {
                WoodChange.Opacity -= OpacityChanging;
                WoodChangeLabel.Opacity -= OpacityChanging;
            }
            if (StoneChange.Opacity >= 0)
            {
                StoneChange.Opacity -= OpacityChanging;
                StoneChangeLabel.Opacity -= OpacityChanging;
            }
            if (FoodChange.Opacity >= 0)
            {
                FoodChange.Opacity -= OpacityChanging;
                FoodChangeLabel.Opacity -= OpacityChanging;
            }
            if (GoldChange.Opacity >= 0)
            {
                GoldChange.Opacity -= OpacityChanging;
                GoldChangeLabel.Opacity -= OpacityChanging;
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
                    HeartChange.Opacity = 1;
                    HeartChangeLabel.Opacity = 1;
                    string s = player.ResourceChanges[0] > 0 ? "+" : "";
                    HeartChangeLabel.Content = s + player.ResourceChanges[0];
                    player.ResourceChanges[0] = 0;
                }
                if (player.ResourceChanges[1] != 0)
                {
                    //ArmyPower
                    ArmyPowerChange.Opacity = 1;
                    ArmyPowerChangeLabel.Opacity = 1;
                    string s = player.ResourceChanges[1] > 0 ? "+" : "";
                    ArmyPowerChangeLabel.Content = s + player.ResourceChanges[1];
                    player.ResourceChanges[1] = 0;
                }
                if (player.ResourceChanges[2] != 0)
                {
                    //Wood
                    WoodChange.Opacity = 1;
                    WoodChangeLabel.Opacity = 1;
                    string s = player.ResourceChanges[2] > 0 ? "+" : "";
                    WoodChangeLabel.Content = s + player.ResourceChanges[2];
                    player.ResourceChanges[2] = 0;
                }
                if (player.ResourceChanges[3] != 0)
                {
                    //Stone
                    StoneChange.Opacity = 1;
                    StoneChangeLabel.Opacity = 1;
                    string s = player.ResourceChanges[3] > 0 ? "+" : "";
                    StoneChangeLabel.Content = s + player.ResourceChanges[3];
                    player.ResourceChanges[3] = 0;
                }
                if (player.ResourceChanges[4] != 0)
                {
                    //Food
                    FoodChange.Opacity = 1;
                    FoodChangeLabel.Opacity = 1;
                    string s = player.ResourceChanges[4] > 0 ? "+" : "";
                    FoodChangeLabel.Content = s + player.ResourceChanges[4];
                    player.ResourceChanges[4] = 0;
                }
                if (player.ResourceChanges[5] != 0)
                {
                    //Gold
                    GoldChange.Opacity = 1;
                    GoldChangeLabel.Opacity = 1;
                    string s = player.ResourceChanges[5] > 0 ? "+" : "";
                    GoldChangeLabel.Content = s + player.ResourceChanges[5];
                    player.ResourceChanges[5] = 0;
                }

            }
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

        private void Build_Button_Click(object sender, RoutedEventArgs e)
        {
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
            txt_build.Foreground = Brushes.Gray;
            btn_build.IsEnabled = false;
        }

        private void Harvest_Button_Click(object sender, RoutedEventArgs e)
        {
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
            txt_harvest.Foreground = Brushes.Gray;
            btn_harvest.IsEnabled = false;
        }
    }
}
