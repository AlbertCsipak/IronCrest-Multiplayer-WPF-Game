using System.Collections.Generic;

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
        public int Moves { get; set; }
        public int Popularity { get; set; }//MysteryResource
        public int ArmyPower { get; set; }//MysteryResource
        public int Gold { get; set; }//MysteryResource
        public int Wood { get; set; }//MysteryResource
        public int Stone { get; set; }//MysteryResource
        public int Food { get; set; }//MysteryResource
        public int BattlesWon { get; set; }
        public List<Quest> Quests { get; set; }
        public List<Village> Villages { get; set; }
        public List<Unit> Units { get; set; }
        public Hero Hero { get; set; }
        public List<Trade> Trade { get; set; }
    }
}
