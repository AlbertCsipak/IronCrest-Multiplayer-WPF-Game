using GUI_20212022_Z6O9JF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GUI_20212022_Z6O9JF.Logic
{
    public interface IGameLogic
    {
        List<Faction> AvailableFactions { get; set; }
        int ClientID { get; set; }
        MysteryEvent CurrentMystery { get; set; }
        Trade CurrentTrade { get; set; }
        HexagonTile[,] GameMap { get; set; }
        string Map { get; set; }
        Queue<MysteryEvent> MysteryEvents { get; set; }
        ObservableCollection<Player> Players { get; set; }
        HexagonTile SelectedHexagonTile { get; set; }

        void AddUnit();
        void AddVillage();
        void ChooseOffer();
        void DecreaseMoves();
        HexagonTile[,] GameMapSetup(string path);
        void GetResources();
        Queue<MysteryEvent> LoadMysteryEvents();
        Queue<Trade> LoadTrades();
        void MoveUnit(HexagonTile hexagonTile);
        void MysteryBoxEvent(HexagonTile hexagonTile);
        void MysteryButtonOK();
        List<Quest> RandomQuestSelector(int n);
        void ReadQuests();
        void ReloadHexagonObjects();
        void ResetMoves();
        void SelectableFactions();
        void UpgradeVillage();
    }
}