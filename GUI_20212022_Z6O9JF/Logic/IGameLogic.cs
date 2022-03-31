using GUI_20212022_Z6O9JF.Models;
using System.Collections.ObjectModel;

namespace GUI_20212022_Z6O9JF.Logic
{
    public interface IGameLogic
    {
        GameLogic.FieldType[,] GameMap { get; set; }
        string Map { get; set; }
        ObservableCollection<Player> Players { get; set; }
        object View { get; set; }

        void ChangeView(string view);
        void ClientConnect();
        GameLogic.FieldType[,] GameMapSetup(string path);
        void StartServer();
    }
}