namespace GUI_20212022_Z6O9JF.Models
{
    //public enum HeroType { Viking, Crusader, Mongolian, Arabian }
    public class Hero : IGameItem
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public Faction FactionType { get; set; }
        public int[] Position { get; set; }
        public bool CanMove { get; set; }

        public Hero()
        {
            Position = new int[2];
            CanMove = true;
        }

        public void Move(int[] pos)
        {
            if (CanMove)
            {
                Position = pos;
            }
        }
    }

}
