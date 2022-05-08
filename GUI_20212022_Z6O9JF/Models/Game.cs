using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI_20212022_Z6O9JF.Models
{
    public class Game
    {
        public int Turns { get; set; }
        public int Timer { get; set; }
        public int QuestCount { get; set; }
        public string Map { get; set; }
        public List<Player> Players { get; set; }
        public List<Trade> Trades { get; set; }
        public List<Player> WinOrder { get; set; }
        public Battle CurrentBattle { get; set; }
        public Player CurrentGoldMineOwner = null;
        public int PlayerID = 1;
        public int PlayerCount { get; set; }
        public Game()
        {
            Players = new List<Player>();
            Trades = new List<Trade>();
            WinOrder = new List<Player>();
        }
        public void NextPlayer() {
            PlayerID++;
            //System.Threading.Thread.Sleep(300);
            if (PlayerID>PlayerCount)
            {
                PlayerID = 1;
            }
        }
    }
}
