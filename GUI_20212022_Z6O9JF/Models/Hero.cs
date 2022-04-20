namespace GUI_20212022_Z6O9JF.Models
{
    //public enum HeroType { Viking, Crusader, Mongolian, Arabian }
    public class Hero
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public Faction FactionType { get; set; }

    }

}
