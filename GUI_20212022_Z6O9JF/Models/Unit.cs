namespace GUI_20212022_Z6O9JF.Models
{
    public enum UnitType { Viking, Crusader, Mongolian, Arabian }
    public class Unit
    {
        public string Name { get; set; }
        public int[,] Position { get; set; }
        public UnitType UnitType { get; set; }

    }
}
