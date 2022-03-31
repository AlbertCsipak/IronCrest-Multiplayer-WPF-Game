using GUI_20212022_Z6O9JF.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.ComponentModel;
using System.Windows;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {
        public IGameLogic gameLogic;
        public object View
        {
            get { return gameLogic.View; }
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
        public MainWindowViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>()) { }
        public MainWindowViewModel(IGameLogic gameLogic)
        {
            this.gameLogic = gameLogic;
            gameLogic.ChangeView("menu");

            Messenger.Register<MainWindowViewModel, string, string>(this, "Base", (recipient, msg) =>
            {
                OnPropertyChanged("View");
            });
        }
    }
}
