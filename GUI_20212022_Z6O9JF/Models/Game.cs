using System.Collections.Generic;

namespace GUI_20212022_Z6O9JF.Models
{
    public class Game
    {
        public int QuestCount { get; set; }
        public int Timer { get; set; }
        public List<Player> Players { get; set; }
        public List<Trade> Trades { get; set; }
        public List<Player> WinOrder { get; set; }
        public Battle CurrentBattle { get; set; }
        public Game()
        {
            Players = new List<Player>();
            Trades = new List<Trade>();
            WinOrder = new List<Player>();
            CurrentBattle = new Battle();
        }
    }
}
