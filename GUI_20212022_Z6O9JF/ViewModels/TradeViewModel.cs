using GUI_20212022_Z6O9JF.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class TradeViewModel : ObservableRecipient
    {
        public IGameLogic gameLogic { get; set; }
        public IClientLogic clientLogic { get; set; }
        public IControlLogic controlLogic { get; set; }
        public ICommand ChooseOffer { get; set; }
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
        public TradeViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>(), Ioc.Default.GetService<IClientLogic>(), Ioc.Default.GetService<IControlLogic>()) { }
        public TradeViewModel(IGameLogic gameLogic, IClientLogic clientLogic, IControlLogic controlLogic)
        {
            this.controlLogic = controlLogic;
            this.gameLogic = gameLogic;
            this.clientLogic = clientLogic;

            ChooseOffer = new RelayCommand(() => gameLogic.ChooseOffer());

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
