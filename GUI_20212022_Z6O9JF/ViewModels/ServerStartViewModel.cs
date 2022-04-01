using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class ServerStartViewModel : ObservableRecipient
    {
        public IGameLogic gameLogic;
        public ICommand BackCommand { get; set; }
        public ICommand StartCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand StopCommand { get; set; }
        public List<int> TurnLengths { get; set; }
        public List<int> Clients { get; set; }
        public List<int> Maps { get; set; }
        public string IP { get; set; }
        public int ClientNumber { get; set; }
        public int TurnLength { get; set; }
        public int Map { get; set; }
        ObservableCollection<Player> vs;

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
        public ServerStartViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>()) { }
        public ServerStartViewModel(IGameLogic gameLogic)
        {
            this.gameLogic = gameLogic;

            TurnLengths = new List<int>();
            Clients = new List<int>();
            Maps = new List<int>();

            TurnLengths.Add(30);
            TurnLengths.Add(45);
            TurnLengths.Add(60);
            TurnLengths.Add(90);

            Clients.Add(2);
            Clients.Add(3);
            Clients.Add(4);

            Maps.Add(1);
            Maps.Add(2);

            BackCommand = new RelayCommand(() => gameLogic.ChangeView("menu"));

            StartCommand = new RelayCommand(() => gameLogic.StartServer());
            LoadCommand = new RelayCommand(() => gameLogic.LoadGame(vs));
        }
    }
}
