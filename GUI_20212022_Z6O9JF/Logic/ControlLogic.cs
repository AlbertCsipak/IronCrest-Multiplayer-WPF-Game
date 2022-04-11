using GUI_20212022_Z6O9JF.Models;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class ControlLogic : IControlLogic
    {
        IGameLogic gameLogic;
        IClientLogic clientLogic;
        Polygon SelectedPolygon;
        Brush currentColor;
        public Grid grid { get; set; }
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
                if (SelectedPolygon != null && SelectedPolygon != polygon)
                {
                    gameLogic.MoveUnit(polygon.Tag as HexagonTile);
                    ClearSelections();
                }
            }
        }
        public void Polygon_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Polygon polygon = (sender as Polygon);
            if ((polygon.Tag as HexagonTile).FieldType != FieldType.water)
            {
                ClearSelections();

                SelectedPolygon = polygon;
                gameLogic.SelectedHexagonTile = (SelectedPolygon.Tag as HexagonTile);

                PolygonBorderBrush(polygon);

                if ((polygon.Tag as HexagonTile).Objects.Where(t => t.CanMove).ToList().Count != 0)
                {
                    foreach (var item in (polygon.Tag as HexagonTile).NeighborCoords())
                    {
                        Polygon thisPoly = grid.Children[gameLogic.GameMap[item.X, item.Y].ParentId] as Polygon;
                        if (gameLogic.GameMap[item.X, item.Y].FieldType != FieldType.water)
                        {
                            PolygonBorderBrush(thisPoly);
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
                    polygon.Stroke = currentColor;
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
                    currentColor = polygon.Stroke;
                    polygon.Stroke = Brushes.White;

                }
            }
        }
        void ClearSelections()
        {
            if (SelectedPolygon != null)
            {
                SelectedPolygon.Stroke = Brushes.Transparent;
                foreach (var item in (SelectedPolygon.Tag as HexagonTile).NeighborCoords())
                {
                    Polygon thisPoly = grid.Children[gameLogic.GameMap[item.X, item.Y].ParentId] as Polygon;
                    if (gameLogic.GameMap[item.X, item.Y].FieldType != FieldType.water)
                    {
                        thisPoly.Stroke = Brushes.Transparent;
                    }
                }
                SelectedPolygon = null;
            }
        }
        void PolygonBorderBrush(Polygon polygon)
        {
            switch (gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).Select(t => t.Faction).FirstOrDefault())
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
                default:
                    break;
            }
        }
    }
}
