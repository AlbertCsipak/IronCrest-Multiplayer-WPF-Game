using GUI_20212022_Z6O9JF.Models;
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
                if (SelectedPolygon != null && SelectedPolygon != polygon)
                {
                    if (gameLogic.SelectedHexagonTile.Objects.Count > 0)
                    {
                        foreach (var item in gameLogic.SelectedHexagonTile.Objects.ToList())
                        {
                            if (item.CanMove)
                            {
                                gameLogic.SelectedHexagonTile.Objects.Remove(item);
                                item.Move((polygon.Tag as HexagonTile).Position);
                                (polygon.Tag as HexagonTile).Objects.Add(item);
                                SelectedPolygon.Stroke = Brushes.Transparent;
                                SelectedPolygon = null;
                            }
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

        public void Polygon_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Polygon polygon = (sender as Polygon);
            if ((polygon.Tag as HexagonTile).FieldType != FieldType.water)
            {
                if (SelectedPolygon != null)
                {
                    SelectedPolygon.Stroke = Brushes.Transparent;
                }
                SelectedPolygon = polygon;
                polygon.Stroke = Brushes.Red;
                gameLogic.SelectedHexagonTile = (SelectedPolygon.Tag as HexagonTile);
            }
        }
    }
}
