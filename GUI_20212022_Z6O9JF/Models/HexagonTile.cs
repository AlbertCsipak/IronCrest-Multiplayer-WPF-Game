using System.Collections.ObjectModel;

namespace GUI_20212022_Z6O9JF.Models
{

    public enum FieldType { field, water, village, hill, forest, wheat }
    public class HexagonTile
    {
        public int OwnerId { get; set; }
        public FieldType FieldType { get; set; }
        public int[] Position { get; set; }
        public ObservableCollection<IGameItem> Objects { get; set; }
        public HexagonTile()
        {
            Objects = new ObservableCollection<IGameItem>();
            Position = new int[2];
        }
    }
}
