﻿using GUI_20212022_Z6O9JF.Models;

namespace GUI_20212022_Z6O9JF.Logic
{
    public interface IClientLogic
    {
        object BattleView { get; set; }
        bool CanSend { get; set; }
        int ClientId { get; set; }
        object ESCView { get; set; }
        object GameEndView { get; set; }
        object GoldMineView { get; set; }
        bool inBattle { get; set; }
        object MysteryHeroView { get; set; }
        object MysteryView { get; set; }
        int Timer { get; set; }
        object TradeView { get; set; }
        object View { get; set; }

        void BattleViewChange(string view);
        void ChampSelect(Faction faction, string name);
        void ChangeView(string view);
        void ChooseOffer();
        void ClientConnect(string ip);
        void EnterGoldMine();
        void ESCChange(string view);
        void GoldMineViewChange(string view);
        void IsAllQuestsDone();
        void LoadGame(string save, int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1");
        void MysteryButtonOK();
        void MysteryHeroViewChange(string view);
        void MysteryViewChange(string view);
        void SkipTurn();
        void StartServer(int turnLength = 100, int clients = 1, string map = "1", string ip = "127.0.0.1", int port = 10000, int bufferSize = 8192);
        void TradeViewChange(string view);
        void YourTurn();
    }
}