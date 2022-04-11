using System.Collections.ObjectModel;
using System.Drawing;

namespace GUI_20212022_Z6O9JF.Models
{

    public enum FieldType { field, water, village, hill, forest, wheat, goldMine }
    public class HexagonTile
    {
        public int ParentId { get; set; }
        public int OwnerId { get; set; }
        public FieldType FieldType { get; set; }
        public int[] Position { get; set; }
        public ObservableCollection<IGameItem> Objects { get; set; }
        public HexagonTile()
        {
            Objects = new ObservableCollection<IGameItem>();
            Position = new int[2];
            OwnerId = 0;
        }
        public bool IsNeighbor(HexagonTile tile)
        {
            if (tile.Position[1]%2==1)
            {
                //[i,j-1], [i, j+1], [i-1,j], [i+1,j-1], [i+1,j], [i+1,j+1]
                if (tile.Position[0] == this.Position[0]) //i
                {
                    if (tile.Position[1] == (this.Position[1] - 1)) //j-1
                    {
                        return true;
                    }
                    if (tile.Position[1] == (this.Position[1] + 1)) //j+1
                    {
                        return true;
                    }
                }
                if (tile.Position[0] == (this.Position[0] + 1)) //i+1
                {
                    if (tile.Position[1] == (this.Position[1] - 1)) //j-1
                    {
                        return true;
                    }
                    if (tile.Position[1] == this.Position[1]) //j
                    {
                        return true;
                    }
                    if (tile.Position[1] == (this.Position[1] + 1)) //j+1
                    {
                        return true;
                    }
                }
                if (tile.Position[0] == (this.Position[0] - 1) && tile.Position[1] == (this.Position[1])) // [i-1,j]
                {
                    return true;
                }
                return false;
            }
            else
            {
                //[i-1,j-1], [i-1, j], [i-1,j+1], [i,j-1], [i,j+1], [i+1,j]
                if (tile.Position[0] == this.Position[0]) //i
                {
                    if (tile.Position[1] == (this.Position[1] - 1)) //j-1
                    {
                        return true;
                    }
                    if (tile.Position[1] == (this.Position[1] + 1)) //j+1
                    {
                        return true;
                    }
                }
                if (tile.Position[0] == (this.Position[0] - 1)) //i-1
                {
                    if (tile.Position[1] == (this.Position[1] - 1)) //j-1
                    {
                        return true;
                    }
                    if (tile.Position[1] == (this.Position[1] + 1)) //j+1
                    {
                        return true;
                    }
                    if(tile.Position[1] == this.Position[1]) //j
                    {
                        return true;
                    }
                }
                if (tile.Position[0] == (this.Position[0] + 1) && tile.Position[1] == this.Position[1]) //i+1, j
                {
                    return true;
                }
                return false;
            }
            
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

    }
}
