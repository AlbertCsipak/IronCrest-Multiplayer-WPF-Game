using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GUI_20212022_Z6O9JF.Logic
{
    public interface IControlLogic
    {
        Grid grid { get; set; }

        void PolygonBorderBrush(Polygon polygon);
        void Polygon_MouseEnter(object sender, MouseEventArgs e);
        void Polygon_MouseLeave(object sender, MouseEventArgs e);
        void Polygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e);
        void Polygon_MouseRightButtonDown(object sender, MouseButtonEventArgs e);
    }
}