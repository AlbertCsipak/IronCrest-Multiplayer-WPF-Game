using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GUI_20212022_Z6O9JF.Models
{
    public enum Faction { Viking, Crusader, Mongolian, Arabian }
    public enum Activity { Move, Build, Upgrade, Farm }
    public class Player
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public Faction Faction { get; set; }
        public Activity Activity { get; set; }
        public bool GoldMine { get; set; }
        public int DefaultNumOfMoves { get; set; }
        public int RemainingMoves { get; set; }

        private int popularity;
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
        public int Wood
        {
            get { return wood; }
            set
            {
                if (wood > value) ResourceChanges[2] = (wood - value) * (-1);
                else ResourceChanges[2] = value - wood;
                wood = value;
            }
        }//MysteryResource

        private int stone;
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

        private int food;
        public int Wheat { 
            get{return food;}
            set {if (food > value) ResourceChanges[4] = (food - value)*(-1);
                else ResourceChanges[4] = value - food;
                food = value;} 
        }//MysteryResource

        public ObservableCollection<int> ResourceChanges { get; set; }
        public int BattlesWon { get; set; }
        public List<Quest> Quests { get; set; }
        public List<Village> Villages { get; set; }
        public List<Unit> Units { get; set; }
        public Hero Hero { get; set; }
        public Trade Trade { get; set; }

        public Player()
        {
            ResourceChanges = new ObservableCollection<int>();
            ResourceChanges.Add(0);
            ResourceChanges.Add(0);
            ResourceChanges.Add(0);
            ResourceChanges.Add(0);
            ResourceChanges.Add(0);
            ResourceChanges.Add(0);

        }
    }
}
