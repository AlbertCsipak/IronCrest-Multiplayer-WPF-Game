using GUI_20212022_Z6O9JF.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.ComponentModel;
using System.Windows;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class MainViewModel : ObservableRecipient
    {
        public IGameLogic gameLogic;
        public IClientLogic clientLogic;
        public object View
        {
            get { return clientLogic.View; }
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
        public MainViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IClientLogic>(), Ioc.Default.GetService<IGameLogic>()) { }
        public MainViewModel(IClientLogic clientLogic,IGameLogic gameLogic)
        {
            this.clientLogic = clientLogic;
            this.gameLogic = gameLogic;
            clientLogic.ChangeView("menu");

            Messenger.Register<MainViewModel, string, string>(this, "Base", (recipient, msg) =>
            {
                OnPropertyChanged("View");
            });
        }
    }
}
