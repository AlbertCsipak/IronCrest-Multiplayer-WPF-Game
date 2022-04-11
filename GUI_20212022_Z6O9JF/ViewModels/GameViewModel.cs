using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class GameViewModel : ObservableRecipient
    {
        public IGameLogic gameLogic { get; set; }
        public IClientLogic clientLogic { get; set; }
        public IControlLogic controlLogic { get; set; }
        public ICommand AddUnitCommand { get; set; }
        public ICommand AddVillageCommand { get; set; }
        public ICommand UpgradeVillageCommand { get; set; }
        public ICommand SkipTurnCommand { get; set; }

        public int Timer { get { return clientLogic.Timer; } }
        public bool CanSend { get { return clientLogic.CanSend; } }
        public int Wood { get { return gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Wood).FirstOrDefault(); } }
        public int Stone { get { return gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Stone).FirstOrDefault(); } }
        public int Food { get { return gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Food).FirstOrDefault(); } }
        public int Gold { get { return gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Gold).FirstOrDefault(); } }
        public int Popularity { get { return gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Popularity).FirstOrDefault(); } }
        public int ArmyPower { get { return gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.ArmyPower).FirstOrDefault(); } }
        public List<string> Quests { get { return gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).SelectMany(t => t.Quests).Select(x=>x.Name).ToList(); } }

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
        public GameViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>(), Ioc.Default.GetService<IClientLogic>(), Ioc.Default.GetService<IControlLogic>()) { }
        public GameViewModel(IGameLogic gameLogic, IClientLogic clientLogic, IControlLogic controlLogic)
        {
            this.controlLogic = controlLogic;
            this.gameLogic = gameLogic;
            this.clientLogic = clientLogic;

            AddUnitCommand = new RelayCommand(() => gameLogic.AddUnit());
            AddVillageCommand = new RelayCommand(() => gameLogic.AddVillage());
            UpgradeVillageCommand = new RelayCommand(() => gameLogic.UpgradeVillage());
            SkipTurnCommand = new RelayCommand(() => clientLogic.SkipTurn());

            Messenger.Register<GameViewModel, string, string>(this, "Base", (recipient, msg) =>
            {
                OnPropertyChanged("CanSend");
                OnPropertyChanged("Wood");
                OnPropertyChanged("Food");
                OnPropertyChanged("Stone");
                OnPropertyChanged("Gold");
                OnPropertyChanged("Popularity");
                OnPropertyChanged("ArmyPower");
                OnPropertyChanged("Quests");
                OnPropertyChanged("Timer");
            });

        }
    }
}
