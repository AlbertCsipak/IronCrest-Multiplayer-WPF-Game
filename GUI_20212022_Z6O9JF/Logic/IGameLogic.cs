using GUI_20212022_Z6O9JF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GUI_20212022_Z6O9JF.Logic
{
    public interface IGameLogic
    {
        List<Faction> AvailableFactions { get; set; }
        bool CanSend { get; set; }
        int ClientId { get; set; }
        GameLogic.FieldType[,] GameMap { get; set; }
        string Map { get; set; }
        ObservableCollection<Player> Players { get; set; }
        object View { get; set; }

        void ChampSelect(Faction faction, string name = "Anon");
        void ChangeView(string view);
        void ClientConnect();
        GameLogic.FieldType[,] GameMapSetup(string path);
        void StartServer(int turnLength = 100, int clients = 2, int map = 1, string ip = "127.0.0.1");
    }
}