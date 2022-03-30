using System.Collections.Generic;

namespace GUI_20212022_Z6O9JF.Models
{
    public class Player
    {
        public string Name { get; set; }
        public int PlayerID { get; set; }
        public List<string> Villages { get; set; }
        public List<string> Units { get; set; }
        public List<string> Heroes { get; set; }
    }
}
