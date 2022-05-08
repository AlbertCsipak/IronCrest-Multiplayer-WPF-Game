using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GUI_20212022_Z6O9JF.Models
{
    public enum Faction { Viking, Crusader, Mongolian, Arabian }
    public enum TurnActivity { Init, Move, Build, Upgrade, Harvest }
    public class Player
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public Faction Faction { get; set; }
        public TurnActivity TurnActivity { get; set; }
        public bool GoldMine { get; set; }
        public bool HasEnteredGoldMine { get; set; }
        public int DefaultNumOfMoves { get; set; }
        public int RemainingMoves { get; set; }
        private int popularity;
        public ObservableCollection<int> ResourceChanges { get; set; }
        public int BattlesWon { get; set; }
        public List<Quest> Quests { get; set; }
        //public int QuestsDone { get; set; }
        public List<Village> Villages { get; set; }
        public List<Unit> Units { get; set; }
        public List<Hero> Heroes { get; set; }
        public Trade Trade { get; set; }
        public int NumOfTradesMade { get; set; }
        public bool HisTurn { get; set; }
        public void SetupPopulatiry(int num)
        {
            popularity = num;
        }
        public int Popularity
        {
            get { return popularity; }
            set
            {
                if (popularity > value) ResourceChanges[0] = (popularity - value) * (-1);
                else ResourceChanges[0] = value - popularity;
                popularity = value;
            }
        }//MysteryResource

        private int armyPower;
        public void SetupArmyPower(int num)
        {
            armyPower = num;
        }
        public int ArmyPower
        {
            get { return armyPower; }
            set
            {
                if (armyPower > value) ResourceChanges[1] = (armyPower - value) * (-1);
                else ResourceChanges[1] = value - armyPower;
                armyPower = value;
            }
        }//MysteryResource

        private int gold;
        public void SetupGold(int num)
        {
            gold = num;
        }
        public int Gold
        {
            get { return gold; }
            set
            {
                if (gold > value) ResourceChanges[5] = (gold - value) * (-1);
                else ResourceChanges[5] = value - gold;
                gold = value;
            }
        }//MysteryResource
        private int wood;

        public void SetupWood(int num)
        {
            wood = num;
        }
        public int Wood
        {
            get { return wood; }
            set
            {
                ;
                if (wood > value) ResourceChanges[2] = (wood - value) * (-1);
                else ResourceChanges[2] = value - wood;
                wood = value;
            }
        }//MysteryResource

        private int stone;

        public void SetupStone(int num)
        {
            stone = num;
        }
        public int Stone
        {
            get { return stone; }
            set
            {
                if (stone > value) ResourceChanges[3] = (stone - value) * (-1);
                else ResourceChanges[3] = value - stone;
                stone = value;
            }
        }//MysteryResource

        private int wheat;
        public void SetupWheat(int num)
        {
            wheat = num;
        }
        public int Wheat
        {
            get { return wheat; }
            set
            {
                if (wheat > value) ResourceChanges[4] = (wheat - value) * (-1);
                else ResourceChanges[4] = value - wheat;
                wheat = value;
            }
        }//MysteryResource



        public Player()
        {
            NumOfTradesMade = 0;
            Units = new List<Unit>();
            Heroes = new List<Hero>();
            Quests = new List<Quest>();
            //QuestsDone = 0;
            Villages = new List<Village>();
            TurnActivity = TurnActivity.Init;
            ResourceChanges = new ObservableCollection<int>() {0,0,0,0,0,0 };
        }
    }
}
