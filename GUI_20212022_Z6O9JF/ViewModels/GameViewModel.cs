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
        public object TradeView { get { return clientLogic.TradeView; } }
        public object MysteryView { get { return clientLogic.MysteryView; } }
        public object MysteryHeroView { get { return clientLogic.MysteryHeroView; } }
        public object GoldMineView { get { return clientLogic.GoldMineView; } }
        public object BattleView { get { return clientLogic.BattleView; } }
        public object GameEndView { get { return clientLogic.GameEndView; } }
        
        public IGameLogic gameLogic { get; set; }
        public IClientLogic clientLogic { get; set; }
        public IControlLogic controlLogic { get; set; }
        public ICommand MoveUnitCommand { get; set; }
        //public ICommand AddVillageCommand { get; set; }
        //public ICommand UpgradeVillageCommand { get; set; }
        public ICommand SkipTurnCommand { get; set; }

        //public ICommand SettingsCommand { get; set; }
        //public ICommand ProduceCommand { get; set; }
        public ICommand TradeCommand { get; set; }
        public Faction SelectedFaction { get { return gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Faction; } }
        public Hero Hero1
        {
            get
            {
                if (gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Heroes.Where(x => x.HeroType == HeroType.First).FirstOrDefault() != null)
                {
                    return gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Heroes.Where(x => x.HeroType == HeroType.First).First();
                }
                else
                {
                    return new Hero();
                }
            }
        }
        public Hero Hero2
        {
            get
            {
                if (gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Heroes.Where(x => x.HeroType == HeroType.Secondary).FirstOrDefault() != null)
                {
                    return gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Heroes.Where(x => x.HeroType == HeroType.Secondary).First();
                }
                else
                {
                    return new Hero();
                }
            }
        }
        public string Hero1Hover { get { return $"{Hero1.Name} : {Hero1.Damage} dmg"; } }
        public string Hero2Hover { get { return $"{Hero2.Name} : {Hero2.Damage} dmg"; } }

        public int Timer { get { return clientLogic.Timer; } }
        public bool CanSend { get { return clientLogic.CanSend; } }
        public int Wood { get { return gameLogic.Game.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Wood).FirstOrDefault(); } }
        public int Stone { get { return gameLogic.Game.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Stone).FirstOrDefault(); } }
        public int Food { get { return gameLogic.Game.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Wheat).FirstOrDefault(); } }
        public int Gold { get { return gameLogic.Game.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Gold).FirstOrDefault(); } }
        public int Popularity { get { return gameLogic.Game.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Popularity).FirstOrDefault(); } }
        public int ArmyPower { get { return gameLogic.Game.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.ArmyPower).FirstOrDefault(); } }
        public List<string> Quests { get { return gameLogic.Game.Players.Where(t => t.PlayerID == gameLogic.ClientID).SelectMany(t => t.Quests).Select(x => x.Name).ToList(); } }
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

            MoveUnitCommand = new RelayCommand(() => gameLogic.AddUnit());
            //AddVillageCommand = new RelayCommand(() => gameLogic.AddVillage());
            //UpgradeVillageCommand = new RelayCommand(() => gameLogic.UpgradeVillage());
            SkipTurnCommand = new RelayCommand(() => clientLogic.SkipTurn());

            //SettingsCommand = new RelayCommand(() => clientLogic.ESCChange("ESC"));
            //ProduceCommand = new RelayCommand(() => gameLogic.GetResources());
            TradeCommand = new RelayCommand(() => clientLogic.TradeViewChange("trade"));

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
                OnPropertyChanged("TradeView");
                OnPropertyChanged("MysteryView");
                OnPropertyChanged("BattleView");
                OnPropertyChanged("MysteryHeroView");
                
                OnPropertyChanged("Hero1");
                OnPropertyChanged("Hero2");
                OnPropertyChanged("GameEndView");
                OnPropertyChanged("GoldMineView");
            });

        }
    }
}
