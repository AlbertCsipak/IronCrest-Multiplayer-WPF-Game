using System.Collections.Generic;

namespace GUI_20212022_Z6O9JF.Models
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public bool GoldMine { get; set; }
        public int Popularity { get; set; }
        public int ArmyPower { get; set; }
        public int Gold { get; set; }
        public int Wood { get; set; }
        public int Stone { get; set; }
        public int Food { get; set; }
        public int BattlesWon { get; set; }
        public List<Quest> Quests { get; set; }
        public List<string> Villages { get; set; }
        public List<string> Units { get; set; }
        public List<string> Heroes { get; set; }
    }
}
