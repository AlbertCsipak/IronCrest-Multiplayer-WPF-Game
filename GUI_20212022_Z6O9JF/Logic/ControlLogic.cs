using GUI_20212022_Z6O9JF.Models;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class ControlLogic : IControlLogic
    {
        IGameLogic gameLogic;
        IClientLogic clientLogic;
        Polygon SelectedPolygon;
        public ControlLogic(IGameLogic gameLogic, IClientLogic clientLogic)
        {
            this.gameLogic = gameLogic;
            this.clientLogic = clientLogic;
        }
        public void Polygon_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Polygon polygon = (sender as Polygon);
            if ((polygon.Tag as HexagonTile).FieldType != FieldType.water)
            {
                if (SelectedPolygon != null && SelectedPolygon != polygon && gameLogic.SelectedHexagonTile.Objects.Where(t=>t.CanMove).ToList().Count>0)
                {
                    if ((polygon.Tag as HexagonTile).Objects.Where(t=>t.CanMove).ToList().Count == 0)
                    {
                        foreach (var item in gameLogic.SelectedHexagonTile.Objects.Where(t => t.CanMove).ToList())
                        {
                            gameLogic.SelectedHexagonTile.Objects.Remove(item);
                            gameLogic.SelectedHexagonTile.OwnerId = 0;
                            item.Move((polygon.Tag as HexagonTile).Position);
                            (polygon.Tag as HexagonTile).Objects.Add(item);
                            SelectedPolygon.Stroke = Brushes.Transparent;
                            SelectedPolygon = null;
                            gameLogic.SelectedHexagonTile = null;
                        }
                    }
                }
            }
        }

        public void Polygon_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Polygon polygon = (sender as Polygon);
            if ((polygon.Tag as HexagonTile).FieldType != FieldType.water)
            {
                if (SelectedPolygon != polygon)
                {
                    polygon.Stroke = Brushes.Transparent;
                }
            }
        }


        public void Polygon_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Polygon polygon = (sender as Polygon);
            if ((polygon.Tag as HexagonTile).FieldType != FieldType.water)
            {
                if (SelectedPolygon != polygon)
                {
                    polygon.Stroke = Brushes.White;
                    
                }
            }
        }
        public void PolygonBorderBrush(ref Polygon polygon)
        {
            switch ((polygon.Tag as HexagonTile).Objects.Select(x => x.FactionType).FirstOrDefault())
            {
                case Faction.Arabian:
                    polygon.Stroke = Brushes.Red;
                    break;
                case Faction.Crusader:
                    polygon.Stroke = Brushes.Black;
                    break;
                case Faction.Mongolian:
                    polygon.Stroke = Brushes.Yellow;
                    break;
                case Faction.Viking:
                    polygon.Stroke = Brushes.Blue;
                    break;
            }
        }

        public void Polygon_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Polygon polygon = (sender as Polygon);
            if ((polygon.Tag as HexagonTile).FieldType != FieldType.water)
            {
                if ((polygon.Tag as HexagonTile).OwnerId==gameLogic.ClientID || (polygon.Tag as HexagonTile).OwnerId == 0)
                {
                    if (SelectedPolygon != null)
                    {
                        PolygonBorderBrush(ref polygon);
                        if ((polygon.Tag as HexagonTile).Objects.Count!=0)
                        {
                            foreach (var item in (polygon.Tag as HexagonTile).NeighborCoords())
                            {
                                //TERV: HIGHLIGHTOLJA EGY HEXAGON 6 SZOMSZÉDJÁT 
                                Polygon curPol = new Polygon();
                                if (gameLogic.GameMap[item.X, item.Y].FieldType!=FieldType.water)
                                {
                                    PolygonBorderBrush(ref polygon);
                                }
                            }
                        }
                        SelectedPolygon.Stroke = Brushes.Transparent;
                    }
                    
                    SelectedPolygon = polygon;
                    gameLogic.SelectedHexagonTile = (SelectedPolygon.Tag as HexagonTile);
                } 
            }
        }
    }
}
