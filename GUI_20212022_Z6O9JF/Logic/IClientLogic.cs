using GUI_20212022_Z6O9JF.Models;

namespace GUI_20212022_Z6O9JF.Logic
{
    public interface IClientLogic
    {
        bool CanSend { get; set; }
        int ClientId { get; set; }
        object MysteryView { get; set; }
        int Timer { get; set; }
        object TradeView { get; set; }
        object View { get; set; }

        void ChampSelect(Faction faction, string name);
        void ChangeView(string view);
        void ChooseOffer();
        void ClientConnect(string ip);
        void LoadGame(string save, int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1");
        void MysteryButtonOK();
        void MysteryViewChange(string view);
        void SkipTurn();
        void StartServer(int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1", int port = 10000, int bufferSize = 8192);
        void TradeViewChange(string view);
    }
}