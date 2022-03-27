using GUI_20212022_Z6O9JF.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {
        IGameLogic gameLogic;
        public ObservableCollection<string> Data { get; set; }
        public ICommand clientComm { get; set; }
        public ICommand bComm { get; set; }
        public ICommand rComm { get; set; }
        public ICommand skipCommand { get; set; }
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
        //public MainWindowViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>()){}
        public MainWindowViewModel(/*IGameLogic gameLogic*/)
        {
            gameLogic = new GameLogic(this.Messenger);

            Data = gameLogic.Setup();

            clientComm = new RelayCommand(() => gameLogic.Connect());
            rComm = new RelayCommand(() => gameLogic.Red());
            bComm = new RelayCommand(() => gameLogic.Blue());
            skipCommand = new RelayCommand(() => gameLogic.Skip());


            Messenger.Register<MainWindowViewModel, string, string>(this, "BasicChannel", (recipient, msg) =>
            {
                OnPropertyChanged("");
            });
        }
    }
}
