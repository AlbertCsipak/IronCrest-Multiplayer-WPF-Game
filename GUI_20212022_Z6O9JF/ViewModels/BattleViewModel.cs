using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
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
        public int SelectedNumber { get; set; }
        public Battle CurrentBattle { get { return gameLogic.CurrentBattle; } }
        int index = 0;
        public List<int> ArmyPower;
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
            PlayerMaxArmyPower = Math.Min(gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().ArmyPower, 7);
            ArmyPower = new List<int>(); ;
            for (int i = 0; i <= PlayerMaxArmyPower; i++)
            {
                ArmyPower.Add(i);
            }

            
            NextNumber = new RelayCommand(() =>
            {
                if (index < PlayerMaxArmyPower)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
                OnPropertyChanged("SelectedNumber");
            });
            PreviousNumber = new RelayCommand(() =>
            {
                if (index > 0)
                {
                    index--;
                }
                else
                {
                    index = PlayerMaxArmyPower;
                }
                OnPropertyChanged("SelectedNumber");
            });

            Messenger.Register<BattleViewModel, string, string>(this, "Base", (recipient, msg) =>
            {
                OnPropertyChanged("SelectedNumber");
            });
        }
    }
}
