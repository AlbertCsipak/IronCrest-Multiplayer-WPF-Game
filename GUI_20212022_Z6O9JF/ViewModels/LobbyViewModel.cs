using GUI_20212022_Z6O9JF.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.ComponentModel;
using System.Windows;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class LobbyViewModel : ObservableRecipient
    {
        public IGameLogic gameLogic;
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
        public LobbyViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>()) { }
        public LobbyViewModel(IGameLogic gameLogic)
        {
            this.gameLogic = gameLogic;

            //gameLogic.ClientConnect();
            //gameLogic.GameMap = gameLogic.GameMapSetup($"Maps/map{gameLogic.Map}.txt");
        }
    }
}
