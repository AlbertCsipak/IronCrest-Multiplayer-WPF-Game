namespace GUI_20212022_Z6O9JF.Models
{
    public enum UnitType { Viking, Crusader, Mongolian, Arabian }
    public class Unit
    {
        public static int ID { get; set; }
        public string Name { get; set; }
        public int[] Position { get; set; }
        public UnitType UnitType { get; set; }
        public Unit()
        {
            Position = new int[2];
            ID++;
        }
    }
}
