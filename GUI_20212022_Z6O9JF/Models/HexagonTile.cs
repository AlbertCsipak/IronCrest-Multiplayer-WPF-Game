using System.Collections.ObjectModel;
using System.Drawing;

namespace GUI_20212022_Z6O9JF.Models
{

    public enum FieldType { grass, lake, mountain, forest, wheat, goldMine, ocean, compassField }
    public class HexagonTile
    {
        public int ParentId { get; set; }
        public int OwnerId { get; set; }
        public FieldType FieldType { get; set; }
        public int[] Position { get; set; }
        public ObservableCollection<IGameItem> Objects { get; set; }
        public Trade Compass { get; set; }
        public HexagonTile()
        {
            Objects = new ObservableCollection<IGameItem>();
            Position = new int[2];
            OwnerId = 0;
        }
        public Point[] NeighborCoords()
        {
            Point[] coords = new Point[6];
            if (this.Position[1] % 2 == 1)
            {
                coords[0] = new Point(this.Position[0], this.Position[1] - 1);
                coords[1] = new Point(this.Position[0], this.Position[1] + 1);
                coords[2] = new Point(this.Position[0] - 1, this.Position[1]);
                coords[3] = new Point(this.Position[0] + 1, this.Position[1] - 1);
                coords[4] = new Point(this.Position[0] + 1, this.Position[1]);
                coords[5] = new Point(this.Position[0] + 1, this.Position[1] + 1);
            }
            else
            {
                coords[0] = new Point(this.Position[0], this.Position[1] - 1);
                coords[1] = new Point(this.Position[0], this.Position[1] + 1);
                coords[2] = new Point(this.Position[0] - 1, this.Position[1]);
                coords[3] = new Point(this.Position[0] - 1, this.Position[1] - 1);
                coords[4] = new Point(this.Position[0] - 1, this.Position[1] + 1);
                coords[5] = new Point(this.Position[0] + 1, this.Position[1]);
            }

            return coords;
        }
        public void GiveResources(Player player)
        {
            switch (FieldType)
            {
                case FieldType.mountain:
                    player.Stone++;
                    break;
                case FieldType.forest:
                    player.Wood++;
                    break;
                case FieldType.wheat:
                    player.Wheat++;
                    break;
                case FieldType.goldMine:
                    if (player.Faction == Faction.Mongolian)
                    {
                        player.Gold += 3;
                    }
                    else
                    {
                        player.Gold += 2;
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
