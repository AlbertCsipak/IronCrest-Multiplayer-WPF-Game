namespace GUI_20212022_Z6O9JF.Models
{
    public class Battle
    {
        public int AttackerID { get; set; }
        public int DefenderID { get; set; }
        public int WinnerID { get; set; }
        public int LoserID { get; set; }
        public bool IsBattleStarted { get; set; }
        public int[] BattleLocation { get; set; }
        public Battle()
        {
            AttackerID = 0;
            DefenderID = 0;
            WinnerID = 0;
            LoserID = 0;
            IsBattleStarted = false;
            BattleLocation = new int[2] {0,0,};
        }
    }
}
