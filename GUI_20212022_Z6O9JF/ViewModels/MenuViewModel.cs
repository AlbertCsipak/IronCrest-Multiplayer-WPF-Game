using GUI_20212022_Z6O9JF.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class MenuViewModel : ObservableRecipient
    {
        public IClientLogic clientLogic;
        public ICommand BackCommand { get; set; }
        public ICommand JoinGameCommand { get; set; }
        public ICommand CreateGameCommand { get; set; }
        public string IP { get; set; }
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
        public MenuViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IClientLogic>()) { }
        public MenuViewModel(IClientLogic clientLogic)
        {
            this.clientLogic = clientLogic;

            IP = "127.0.0.1";

            JoinGameCommand = new RelayCommand(() => clientLogic.ClientConnect(IP));
            CreateGameCommand = new RelayCommand(() => clientLogic.ChangeView("server"));
        }
    }
}
