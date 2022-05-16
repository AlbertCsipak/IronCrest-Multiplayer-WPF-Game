using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GUI_20212022_Z6O9JF.Models
{
    public enum Faction { Viking, Crusader, Mongolian, Arabian }
    public enum TurnActivity { Init, Move, Build, Upgrade, Harvest }

    public class ResourceChangedEventArgs : EventArgs
    {
        public ResourceChangedEventArgs(string resource, int number)
        {
            Resource = resource;
            Number = number;
        }

        public string Resource { get; set; }
        public int Number { get; set; }
    }

    public class Player
    {
        static public event EventHandler ResourceChanged;
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public Faction Faction { get; set; }
        public TurnActivity TurnActivity { get; set; }
        //public TurnActivity RecentTurnActivity { get; set; }
        public bool IsRecentTurnActivityMove;
        public bool GoldMine { get; set; }
        public bool HasEnteredGoldMine { get; set; }
        public int DefaultNumOfMoves { get; set; }
        public int RemainingMoves { get; set; }
        private int popularity;
        public int BattlesWon { get; set; }
        public List<Quest> Quests { get; set; }
        //public int QuestsDone { get; set; }
        public List<Village> Villages { get; set; }
        public List<Unit> Units { get; set; }
        public List<Hero> Heroes { get; set; }
        public Trade Trade { get; set; }
        public int NumOfTradesMade { get; set; }

        public void SetupPopulatiry(int num)
        {
            popularity = num;
        }
        public int Popularity
        {
            get { return popularity; }
            set
            {
                if (popularity!=value)
                {
                    if (popularity > value) ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("popularity", (value - popularity) * -1));
                    else ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("popularity", value - popularity));
                    popularity = value;
                }
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
                if (armyPower != value)
                {
                    if (armyPower > value) ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("armyPower", (value - armyPower) * -1));
                    else ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("armyPower", value - armyPower));
                    armyPower = value;
                }
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
                if (gold != value)
                {
                    if (gold > value) ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("gold", (value - gold) * -1));
                    else ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("gold", value - gold));
                    gold = value;
                }
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
                if (wood != value)
                {
                    if (wood > value) ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("wood", (value - wood) * -1));
                    else ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("wood", value - wood));
                    wood = value;
                }
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
                if (stone != value)
                {
                    if (stone > value) ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("stone", (value - stone) * -1));
                    else ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("stone", value - stone));
                    stone = value;
                }
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
                if (wheat != value)
                {
                    if (wheat > value) ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("wheat", (value - wheat) * -1));
                    else ResourceChanged?.Invoke(this, new ResourceChangedEventArgs("wheat", value - wheat));
                    wheat = value;
                }
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
        }
    }
}
