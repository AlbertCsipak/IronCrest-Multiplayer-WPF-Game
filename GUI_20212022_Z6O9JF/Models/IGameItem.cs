namespace GUI_20212022_Z6O9JF.Models
{
    public interface IGameItem
    {
        bool CanMove { get; set; }
        Faction FactionType { get; set; }
        bool IsAttackerUnit { get; set; }
        int Level { get; set; }
        string Name { get; set; }
        int OwnerId { get; set; }
        int[] Position { get; set; }

        void Move(int[] pos);
    }
}