namespace GUI_20212022_Z6O9JF.Models
{
    //public enum UnitType { Viking, Crusader, Mongolian, Arabian }
    public class Unit : IGameItem
    {
        public int OwnerId { get; set; }
        public static int ID { get; set; }
        public string Name { get; set; }
        public int[] Position { get; set; }
        public bool CanMove { get; set; }
        public Faction FactionType { get; set; }
        public int Level { get; set; }
        public bool IsAttackerUnit { get; set; }

        public Unit()
        {
            Position = new int[2];
            ID++;
            CanMove = true;
            Level = 1;
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
