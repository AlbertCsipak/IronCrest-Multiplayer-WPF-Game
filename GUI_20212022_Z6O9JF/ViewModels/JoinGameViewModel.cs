using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class JoinGameViewModel : ObservableRecipient
    {
        public IGameLogic gameLogic;
        public ICommand BackCommand { get; set; }
        public ICommand GameCommand { get; set; }
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
        public JoinGameViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>()) { }
        public JoinGameViewModel(IGameLogic gameLogic)
        {
            this.gameLogic = gameLogic;


            gameLogic.ClientConnect();
            gameLogic.GameMap = gameLogic.GameMapSetup($"Maps/map{gameLogic.Map}.txt");

            string name = "bercike";
            Faction faction = new Faction();
            faction = Faction.Viking;

            GameCommand = new RelayCommand(() => gameLogic.ChampSelect(name,faction));
        }
    }
}
