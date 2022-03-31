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
        public IGameLogic gameLogic;
        public ICommand ChangeViewCommand { get; set; }
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
        public MenuViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>()) { }
        public MenuViewModel(IGameLogic gameLogic)
        {
            this.gameLogic = gameLogic;

            ChangeViewCommand = new RelayCommand(() => gameLogic.ChangeView("game"));
        }
    }
}
