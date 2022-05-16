namespace GUI_20212022_Z6O9JF.Models
{
    public class Battle
    {
        public Player Attacker { get; set; }
        public Player Defender { get; set; }
        public Player Winner { get; set; }
        public Player Loser { get; set; }
        public bool IsBattleStarted { get; set; }
        public HexagonTile BattleLocation { get; set; }
        public Battle()
        {
            Attacker = null;
            Defender = null;
            Winner = null;
            Loser = null;
            IsBattleStarted = false;
            BattleLocation = null;
        }
    }
}
