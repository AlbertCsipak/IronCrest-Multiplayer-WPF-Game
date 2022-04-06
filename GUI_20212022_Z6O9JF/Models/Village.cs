namespace GUI_20212022_Z6O9JF.Models
{
    public enum VillageType { Viking, Crusader, Mongolian, Arabian }
    public class Village : IGameItem
    {
        public string Name { get; set; }
        public int[] Position { get; set; }
        public int Level { get; set; }
        public VillageType VillageType { get; set; }
        public bool CanMove { get ; set; }

        public Village()
        {
            Position = new int[2];
            CanMove = false;
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
