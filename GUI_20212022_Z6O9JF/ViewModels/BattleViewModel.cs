using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class BattleViewModel : ObservableRecipient
    {
        public IGameLogic gameLogic { get; set; }
        public IClientLogic clientLogic { get; set; }
        public IControlLogic controlLogic { get; set; }
        public ICommand NextNumber { get; set; }
        public ICommand PreviousNumber { get; set; }
        public ICommand ReadyCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public int SelectedNumber { get; set; }
        public Faction Char { get { return gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Faction; } }
        public Battle CurrentBattle { get { return gameLogic.Game.CurrentBattle; } }
        public Hero Hero1 { get { return gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Heroes.Where(x => x.HeroType == HeroType.First).FirstOrDefault(); } }
        public Hero Hero2 { get { return gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Heroes.Where(x => x.HeroType == HeroType.Secondary).FirstOrDefault(); } }
        public int PlayerMaxArmyPower;
        public static bool IsInDesignMode
        {
            get
            {
                return
                    (bool)DependencyPropertyDescriptor
                    .FromProperty(DesignerProperties.
                    IsInDesignModeProperty,
                    typeof(FrameworkElement))
                    .Metadata.DefaultValue;
            }
        }
        public BattleViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>(), Ioc.Default.GetService<IClientLogic>(), Ioc.Default.GetService<IControlLogic>()) { }
        public BattleViewModel(IGameLogic gameLogic, IClientLogic clientLogic, IControlLogic controlLogic)
        {
            this.controlLogic = controlLogic;
            this.gameLogic = gameLogic;
            this.clientLogic = clientLogic;
            PlayerMaxArmyPower = Math.Min(gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().ArmyPower, 7);
            SelectedNumber = 0;
            NextNumber = new RelayCommand(() =>
            {
                if (SelectedNumber < PlayerMaxArmyPower)
                {
                    SelectedNumber++;
                }
                else
                {
                    SelectedNumber = PlayerMaxArmyPower;
                }
                OnPropertyChanged("SelectedNumber");
            });
            PreviousNumber = new RelayCommand(() =>
            {
                if (SelectedNumber > 0)
                {
                    SelectedNumber--;
                }
                else
                {
                    SelectedNumber = 0;
                }
                OnPropertyChanged("SelectedNumber");
            });
            ReadyCommand = new RelayCommand(() =>
            {
                if (CurrentBattle != null)
                {
                    CurrentBattle.IsBattleStarted = true;
                }

                //start the battle animation
                //BattleDisplay.cs
            });
            ExitCommand = new RelayCommand(() =>
            {
                clientLogic.BattleViewChange("");
            });

            Messenger.Register<BattleViewModel, string, string>(this, "Base", (recipient, msg) =>
            {
                OnPropertyChanged("SelectedNumber");
                //OnPropertyChanged("Hero1");
                //OnPropertyChanged("Hero2");
            });
        }
    }
}
