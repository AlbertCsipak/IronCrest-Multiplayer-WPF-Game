namespace GUI_20212022_Z6O9JF.Models
{
    public enum UnitType { Viking, Crusader, Mongolian, Arabian }
    public class Unit :IGameItem
    {
        public static int ID { get; set; }
        public string Name { get; set; }
        public int[] Position { get; set; }
        public UnitType UnitType { get; set; }
        public bool CanMove { get ; set ; }

        public Unit()
        {
            Position = new int[2];
            ID++;
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
