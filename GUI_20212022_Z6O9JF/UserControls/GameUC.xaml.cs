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
        Window window;

        public MediaPlayer button_click = new MediaPlayer();
        public MediaPlayer bell_sound = new MediaPlayer();
        public MediaPlayer quest_sound = new MediaPlayer();
        public MediaPlayer placement = new MediaPlayer();
        public MediaPlayer upgrade_sound = new MediaPlayer();
        public MediaPlayer move_sound = new MediaPlayer();
        DispatcherTimer dt;
        //bool IsResourceChanged;
        List<SubItem<Quest>> quests;
        Player player;
        bool bell = false;
        ItemMenu itemQuest;
        public GameUC()
        {
            InitializeComponent();
            Player.ResourceChanged += ResourceChanging;
            ClientLogic.StartOfTurnEvent += StartOfTurn;
            ClientLogic.EndOfTurnEvent += EndOfTurn;
            GameLogic.Move += Move;
            this.DataContext = new GameViewModel();
            this.gameLogic = (this.DataContext as GameViewModel).gameLogic;
            this.clientLogic = (this.DataContext as GameViewModel).clientLogic;
            this.controlLogic = (this.DataContext as GameViewModel).controlLogic;
            player = gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault();
            display.LogicSetup(clientLogic, gameLogic, controlLogic, grid);
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
            dt.Interval = TimeSpan.FromMilliseconds(33.33);
            quests = new List<SubItem<Quest>>();
            quests.Add(new SubItem<Quest>(player.Quests.ElementAt(0)));
            quests.Add(new SubItem<Quest>(player.Quests.ElementAt(1)));
            quests.Add(new SubItem<Quest>(player.Quests.ElementAt(2)));
            itemQuest = new ItemMenu("Quests", quests, PackIconKind.ViewDashboard);

            Menu.Children.Add(new UserControlMenuItem(itemQuest));
            dt.Tick += (sender, eventargs) =>
            {
                SetMovePictures();
                clientLogic.IsAllQuestsDone();
                //ResourceChanging();
                OpacityDefault();
                if (clientLogic.Timer == 60.0)
                {
                    SetTurnActivities();

                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(@"\Resources\Images\Menu\hourglassgif.gif", UriKind.Relative);
                    image.EndInit();
                    ImageBehavior.SetAnimatedSource(hourglass_gif, image);
                    ImageBehavior.SetRepeatBehavior(hourglass_gif, new RepeatBehavior(1));
                }
                //if (clientLogic.Timer != 60)
                //{
                //    bell = false;
                //}
                display.InvalidateVisual();
            };
            dt.Start();


        }
        public void Move(object sender, EventArgs e)
        {
            move_sound.Open(new Uri("Resources/Music/move_sound.mp3", UriKind.RelativeOrAbsolute));
            move_sound.Play();
        }

        public void StartOfTurn(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                bell_sound.Open(new Uri("Resources/Music/bell.mp3", UriKind.RelativeOrAbsolute));
                bell_sound.Volume = 0.05;
                bell_sound.Play();
            }));
        }
        public void EndOfTurn(object sender, EventArgs e)
        {
            if (gameLogic.IsQuestDone((e as LastClientEventArgs).lastClientId))
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Menu.Children.Clear();
                    Menu.Children.Add(new UserControlMenuItem(itemQuest));
                    quest_sound.Open(new Uri("Resources/Music/quest_completed_sound.mp3", UriKind.RelativeOrAbsolute));
                    quest_sound.Volume = 0.05;
                    quest_sound.Play();
                }));
            }
        }

        public void EnableAllActivities()
        {
            img_build.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\build_background.png", UriKind.RelativeOrAbsolute));
            img_move.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\move_background.png", UriKind.RelativeOrAbsolute));
            img_upgrade.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\upgrade_background.png", UriKind.RelativeOrAbsolute));
            img_harvest.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\produce_background.png", UriKind.RelativeOrAbsolute));
            btn_move.IsEnabled = true;
            btn_build.IsEnabled = true;
            btn_harvest.IsEnabled = true;
            btn_upgrade.IsEnabled = true;
            txt_build.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
            txt_move.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
            txt_upgrade.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
            txt_harvest.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
        }
        public void DisableAllActivities()
        {
            btn_move.IsEnabled = false;
            btn_build.IsEnabled = false;
            btn_harvest.IsEnabled = false;
            btn_upgrade.IsEnabled = false;
        }
        public void SetTurnActivities()
        {
            player = gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault();
            if (player.Faction != Faction.Crusader)
            {
                switch (player.TurnActivity)
                {
                    case TurnActivity.Init:
                        img_build.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\build_background.png", UriKind.RelativeOrAbsolute));
                        img_move.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\move_background.png", UriKind.RelativeOrAbsolute));
                        img_upgrade.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\upgrade_background.png", UriKind.RelativeOrAbsolute));
                        img_harvest.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\produce_background.png", UriKind.RelativeOrAbsolute));
                        btn_move.IsEnabled = true;
                        btn_build.IsEnabled = true;
                        btn_harvest.IsEnabled = true;
                        btn_upgrade.IsEnabled = true;
                        break;
                    case TurnActivity.Move:
                        img_build.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\build_background.png", UriKind.RelativeOrAbsolute));
                        img_move.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\move_background_disabled.png", UriKind.RelativeOrAbsolute));
                        img_upgrade.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\upgrade_background.png", UriKind.RelativeOrAbsolute));
                        img_harvest.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\produce_background.png", UriKind.RelativeOrAbsolute));
                        btn_move.IsEnabled = false;
                        btn_build.IsEnabled = true;
                        btn_harvest.IsEnabled = true;
                        btn_upgrade.IsEnabled = true;
                        txt_move.Foreground = Brushes.Gray;
                        txt_build.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        txt_upgrade.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        txt_harvest.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        break;
                    case TurnActivity.Build:
                        img_build.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\build_background_disabled.png", UriKind.RelativeOrAbsolute));
                        img_move.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\move_background.png", UriKind.RelativeOrAbsolute));
                        img_upgrade.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\upgrade_background.png", UriKind.RelativeOrAbsolute));
                        img_harvest.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\produce_background.png", UriKind.RelativeOrAbsolute));
                        btn_build.IsEnabled = false;
                        btn_move.IsEnabled = true;
                        btn_harvest.IsEnabled = true;
                        btn_upgrade.IsEnabled = true;
                        txt_build.Foreground = Brushes.Gray;
                        txt_move.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        txt_upgrade.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        txt_harvest.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        break;
                    case TurnActivity.Upgrade:
                        img_build.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\build_background.png", UriKind.RelativeOrAbsolute));
                        img_move.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\move_background.png", UriKind.RelativeOrAbsolute));
                        img_upgrade.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\upgrade_background_disabled.png", UriKind.RelativeOrAbsolute));
                        img_harvest.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\produce_background.png", UriKind.RelativeOrAbsolute));
                        btn_upgrade.IsEnabled = false;
                        btn_move.IsEnabled = true;
                        btn_build.IsEnabled = true;
                        btn_harvest.IsEnabled = true;
                        txt_upgrade.Foreground = Brushes.Gray;
                        txt_build.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        txt_move.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        txt_harvest.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        break;
                    case TurnActivity.Harvest:
                        img_build.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\build_background.png", UriKind.RelativeOrAbsolute));
                        img_move.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\move_background.png", UriKind.RelativeOrAbsolute));
                        img_upgrade.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\upgrade_background.png", UriKind.RelativeOrAbsolute));
                        img_harvest.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\produce_background_disabled.png", UriKind.RelativeOrAbsolute));
                        btn_harvest.IsEnabled = false;
                        btn_move.IsEnabled = true;
                        btn_build.IsEnabled = true;
                        btn_upgrade.IsEnabled = true;
                        txt_harvest.Foreground = Brushes.Gray;
                        txt_build.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        txt_upgrade.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        txt_move.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#B3C8B7");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                EnableAllActivities();
            }
        }
        public void SetMovePictures()
        {
            var player = gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault();
            if (player.DefaultNumOfMoves == 2)
            {
                switch (player.RemainingMoves)
                {
                    case 0:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 1:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 2:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
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
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 2:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 3:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
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
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move4.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 2:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        move4.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 3:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move4.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\disabled_{player.Faction}.png"), UriKind.RelativeOrAbsolute));
                        break;
                    case 4:
                        move1.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move2.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move3.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        move4.Source = new BitmapImage(new Uri(Path.Combine(@$"\Resources\Images\Characters\standing_{player.Faction}_lvl1.png"), UriKind.RelativeOrAbsolute));
                        break;
                    default:
                        break;
                }
            }
        }
        private void OpacityDefault()
        {
            //ResourceChanges
            double OpacityChanging = 0.02;
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

        private void ResourceChanging(object sender, EventArgs e)
        {
            var eventData = (e as ResourceChangedEventArgs);
            player = gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault();
            
            if (eventData.Resource != "" && eventData.Number!=0 )
            {
                player = gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault();
                if (eventData.Resource == "popularity")
                {
                    //POP
                    this.Dispatcher.Invoke(() =>
                    {
                        HeartChange.Opacity = 1;
                        HeartChangeLabel.Opacity = 1;
                        string s = eventData.Number > 0 ? "+" : "";
                        HeartChangeLabel.Content = s + eventData.Number;
                    });
                    
                }
                if (eventData.Resource == "armyPower")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //ArmyPower
                        ArmyPowerChange.Opacity = 1;
                        ArmyPowerChangeLabel.Opacity = 1;
                        string s = eventData.Number > 0 ? "+" : "";
                        ArmyPowerChangeLabel.Content = s + eventData.Number;
                    });
                }
                if (eventData.Resource == "wood")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //Wood
                        WoodChange.Opacity = 1;
                        WoodChangeLabel.Opacity = 1;
                        string s = eventData.Number > 0 ? "+" : "";
                        WoodChangeLabel.Content = s + eventData.Number;
                    });
                }
                if (eventData.Resource == "stone")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //Stone
                        StoneChange.Opacity = 1;
                        StoneChangeLabel.Opacity = 1;
                        string s = eventData.Number > 0 ? "+" : "";
                        StoneChangeLabel.Content = s + eventData.Number;
                    });
                }
                if (eventData.Resource == "wheat")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //Food
                        FoodChange.Opacity = 1;
                        FoodChangeLabel.Opacity = 1;
                        string s = eventData.Number > 0 ? "+" : "";
                        FoodChangeLabel.Content = s + eventData.Number;
                    });
                }
                if (eventData.Resource == "gold")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //Gold
                        GoldChange.Opacity = 1;
                        GoldChangeLabel.Opacity = 1;
                        string s = eventData.Number > 0 ? "+" : "";
                        GoldChangeLabel.Content = s + eventData.Number;
                    });
                }

            }
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

        private void Build_Button_Click(object sender, RoutedEventArgs e)
        {
            player = gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault();
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
            gameLogic.AddVillage();
            if (player.TurnActivity == TurnActivity.Build)
            {
                placement.Open(new Uri("Resources/Music/placement.mp3", UriKind.RelativeOrAbsolute));
                placement.Play();
                SetTurnActivities();
                DisableAllActivities();
            }
        }

        private void Harvest_Button_Click(object sender, RoutedEventArgs e)
        {
            player = gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault();
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
            gameLogic.GetResources();
            if (player.TurnActivity == TurnActivity.Harvest)
            {
                SetTurnActivities();
                DisableAllActivities();
            }
        }

        private void Move_Button_Click(object sender, RoutedEventArgs e)
        {
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
            SetTurnActivities();
            DisableAllActivities();
            if (player.TurnActivity == TurnActivity.Build)
            {
                SetTurnActivities();
                DisableAllActivities();
            }
        }

        private void Upgrade_Button_Click(object sender, RoutedEventArgs e)
        {
            player = gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault();
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
            gameLogic.UpgradeVillage();
            if (player.TurnActivity == TurnActivity.Upgrade)
            {
                upgrade_sound.Open(new Uri("Resources/Music/upgrade.mp3", UriKind.RelativeOrAbsolute));
                upgrade_sound.Play();
                SetTurnActivities();
                DisableAllActivities();
            }
        }

    }
}
