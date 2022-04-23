using GUI_20212022_Z6O9JF.Models;
using System.Collections.Generic;

namespace GUI_20212022_Z6O9JF.Logic
{
    public interface IGameLogic
    {
        List<Faction> AvailableFactions { get; set; }
        int ClientID { get; set; }
        Battle CurrentBattle { get; set; }
        MysteryEvent CurrentMystery { get; set; }
        Trade CurrentTrade { get; set; }
        Hero FirstHero { get; set; }
        Game Game { get; set; }
        HexagonTile[,] GameMap { get; set; }
        string Map { get; set; }
        Queue<MysteryEvent> MysteryEvents { get; set; }
        Hero SecondaryHero { get; set; }
        HexagonTile SelectedHexagonTile { get; set; }
        List<Player> WinOrder { get; set; }

        void AddUnit();
        void AddVillage();
        void Battle(HexagonTile hexagonTile);
        void ClearCompass(HexagonTile hexagon);
        void DecreaseMoves();
        HexagonTile[,] GameMapSetup(string path);
        void GetResources();
        void GetResourcesFromMysteryEvent();
        bool HasSufficientResources(int offerindex);
        bool HasSufficientResources(string resource, int cost);
        bool IsQuestDone();
        Queue<MysteryEvent> LoadMysteryEvents();
        Queue<Trade> LoadTrades();
        void MakeTrade();
        void MoveUnit(HexagonTile hexagonTile);
        void MysteryBoxEvent(HexagonTile hexagonTile);
        void MysteryHeroEvent();
        List<Quest> RandomQuestSelector(int n);
        void ReadQuests();
        void ReloadHexagonObjects();
        void ResetMoves();
        void SelectableFactions();
        void SetGameEndOrder();
        void UpgradeVillage();
    }
}