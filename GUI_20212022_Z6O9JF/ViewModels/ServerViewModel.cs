using GUI_20212022_Z6O9JF.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class ServerViewModel : ObservableRecipient
    {
        public IClientLogic clientLogic;
        public ICommand BackCommand { get; set; }
        public ICommand StartCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public List<int> TurnLengths { get; set; }
        public List<int> Clients { get; set; }
        public List<string> Maps { get; set; }
        public List<string> SaveGames { get; set; }
        public string IP { get; set; }
        public int ClientNumber { get; set; }
        public int TurnLength { get; set; }
        public string Map { get; set; }
        public string SaveGame { get; set; }
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
        public ServerViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IClientLogic>()) { }
        public ServerViewModel(IClientLogic clientLogic)
        {
            this.clientLogic = clientLogic;

            TurnLengths = new List<int>();
            Clients = new List<int>();
            Maps = new List<string>();
            SaveGames = new List<string>();

            foreach (var item in Directory.GetFiles("Resources/Saves"))
            {
                SaveGames.Add(item.Split('\\')[1].Split('.')[0]);
            }

            TurnLengths.Add(30);
            TurnLengths.Add(45);
            TurnLengths.Add(60);
            TurnLengths.Add(90);

            Clients.Add(1);
            Clients.Add(2);
            Clients.Add(3);
            Clients.Add(4);

            Maps.Add("1");
            Maps.Add("2");

            IP = "127.0.0.1";
            SaveGame = "NewGame";
            ClientNumber = Clients[1];
            TurnLength = TurnLengths[2];
            Map = Maps[0];

            BackCommand = new RelayCommand(() => clientLogic.ChangeView("menu"));
            StartCommand = new RelayCommand(() =>
            {

                if (SaveGame.Equals("NewGame"))
                {
                    clientLogic.StartServer(turnLength: TurnLength, clients: ClientNumber, map: Map, ip: IP);
                }
                else
                {
                    string save = File.ReadAllText($"Resources/Saves/{SaveGame}.txt");
                    clientLogic.LoadGame(save: save, turnLength: TurnLength, clients: ClientNumber, map: Map, ip: IP);
                }
            });
        }
    }
}
