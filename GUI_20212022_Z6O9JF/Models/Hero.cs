namespace GUI_20212022_Z6O9JF.Models
{
    public enum HeroType { Viking, Crusader, Mongolian, Arabian }
    public class Hero
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int[] Position { get; set; }
        public HeroType HeroType { get; set; }
        public Hero()
        {
            Position = new int[2];
        }
    }

}
