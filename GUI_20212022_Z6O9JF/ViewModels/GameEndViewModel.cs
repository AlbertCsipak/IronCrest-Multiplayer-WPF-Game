using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class GameEndViewModel : ObservableRecipient
    {
        public IGameLogic gameLogic { get; set; }
        public IClientLogic clientLogic { get; set; }
        public IControlLogic controlLogic { get; set; }
        public Player First { get { return gameLogic.WinOrder.ElementAt(0); } }
        public Player Second { get {
                if (gameLogic.WinOrder.Count>=2)
                {
                    return gameLogic.WinOrder.ElementAt(1); 
                }
                return null;
        } }
        public Player Third { get {
                if (gameLogic.WinOrder.Count>=3)
                {
                    return gameLogic.WinOrder.ElementAt(2); 
                }
                return null;
            } }
        public Player Fourth
        {
            get
            {
                if (gameLogic.WinOrder.Count == 4)
                {
                    return gameLogic.WinOrder.ElementAt(3);
                }
                return null;
            }
        }
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
        public GameEndViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>(), Ioc.Default.GetService<IClientLogic>(), Ioc.Default.GetService<IControlLogic>()) { }
        public GameEndViewModel(IGameLogic gameLogic, IClientLogic clientLogic, IControlLogic controlLogic)
        {
            this.controlLogic = controlLogic;
            this.gameLogic = gameLogic;
            this.clientLogic = clientLogic;

            Messenger.Register<GameEndViewModel, string, string>(this, "Base", (recipient, msg) =>
            {
                OnPropertyChanged("View");
            });
        }
    }
}
