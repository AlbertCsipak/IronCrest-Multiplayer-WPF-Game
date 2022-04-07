namespace GUI_20212022_Z6O9JF.Models
{
    public class Village : IGameItem
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public int[] Position { get; set; }
        public int Level { get; set; }
        public bool CanMove { get ; set; }
        public Faction FactionType { get; set; }

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
