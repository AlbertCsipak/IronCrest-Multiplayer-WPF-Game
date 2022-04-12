namespace GUI_20212022_Z6O9JF.Models
{
    public interface IGameItem
    {
        public int OwnerId { get; set; }
        Faction FactionType { get; set; }
        public int[] Position { get; set; }
        public bool CanMove { get; set; }
        public int Level { get; set; }
        public void Move(int[] pos);
    }
}
