using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
        public MainWindowViewModel()
        {
            gameLogic = new GameLogic(this.Messenger);

            Data = gameLogic.Setup();

            clientComm = new RelayCommand(() => gameLogic.Connect());
            rComm = new RelayCommand(() => gameLogic.Red());
            bComm = new RelayCommand(() => gameLogic.Blue());
            skipCommand = new RelayCommand(() => gameLogic.Skip());
        }
    }
}
