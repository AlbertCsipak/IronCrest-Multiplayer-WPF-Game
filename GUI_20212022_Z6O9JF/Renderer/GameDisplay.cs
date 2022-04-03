using GUI_20212022_Z6O9JF.Logic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI_20212022_Z6O9JF.Renderer
{
    public class GameDisplay : FrameworkElement
    {
        IGameLogic gameLogic;
        Size size;
        Grid grid;
        public double[,][] HexagonPoints { get; set; }
        public GameDisplay()
        {

        }
        public void LogicSetup(IGameLogic gameLogic,Grid grid)
        {
            this.grid = grid;
            this.gameLogic = gameLogic;
            HexagonPoints = new double[gameLogic.GameMap.GetLength(0), gameLogic.GameMap.GetLength(1)][];
        }
        public void Resize(Size size) { this.size = size; }
        public void GetCoordinates()
        {

            double rectHeight = size.Height / HexagonPoints.GetLength(0);
            double rectWidth = size.Width / HexagonPoints.GetLength(1);

            if (size.Width > 0)
            {
                for (int i = 0; i < HexagonPoints.GetLength(0); i++)
                {
                    for (int j = 0; j < HexagonPoints.GetLength(1); j++)
                    {
                        HexagonPoints[i, j] = new double[2];

                        int x = i % 2 == 0 ? 2 : 1;
                        if (i == HexagonPoints.GetLength(0) - 1)
                        {
                            if (i % 2 == 0)
                            {
                                HexagonPoints[i, j][0] = 0;
                                HexagonPoints[i, j][1] = 0;
                            }
                        }
                        else if (j == HexagonPoints.GetLength(1) - 1)
                        {
                            if (i % 2 == 0)
                            {
                                HexagonPoints[i, j][0] = j * rectWidth + rectWidth / x;
                                HexagonPoints[i, j][1] = i * rectHeight + rectHeight;
                            }
                        }
                        else
                        {
                            HexagonPoints[i, j][0] = j * rectWidth + rectWidth / x;
                            HexagonPoints[i, j][1] = i * rectHeight + rectHeight;
                        }
                    }
                }
            }
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            GetCoordinates();
            if (HexagonPoints[0, 0] != null)
            {
                double width = HexagonPoints[0, 0][0];
                double height = HexagonPoints[0, 0][1];
                for (int i = 0; i < HexagonPoints.GetLength(0); i++)
                {
                    for (int j = 0; j < HexagonPoints.GetLength(1); j++)
                    {
                        if (HexagonPoints[i, j][0] != 0 && HexagonPoints[i, j][1] != 0)
                        {

                            Polygon polygon = new Polygon();
                            PointCollection points = new PointCollection();
                            points.Add(new Point(HexagonPoints[i, j][0] - width / 1.5, HexagonPoints[i, j][1]));
                            points.Add(new Point(HexagonPoints[i, j][0] -width / 3, HexagonPoints[i, j][1] - height));
                            points.Add(new Point(HexagonPoints[i, j][0] + width / 3, HexagonPoints[i, j][1] - height));
                            points.Add(new Point(HexagonPoints[i, j][0] + width / 1.5, HexagonPoints[i, j][1]));
                            points.Add(new Point(HexagonPoints[i, j][0] + width / 3, HexagonPoints[i, j][1] + height));
                            points.Add(new Point(HexagonPoints[i, j][0] - width / 3, HexagonPoints[i, j][1] + height));
                            polygon.Points = points;
                            polygon.AllowDrop = true;
                            polygon.Stroke = Brushes.Transparent;
                            polygon.StrokeThickness = 5;
                            polygon.ClipToBounds = false;
                            polygon.FillRule = FillRule.Nonzero;
                            
                            polygon.IsManipulationEnabled = true;
                            polygon.MouseLeftButtonDown += Polygon_MouseLeftButtonDown;
                            polygon.MouseEnter += Polygon_MouseEnter;
                            polygon.MouseLeave += Polygon_MouseLeave;

                            switch (gameLogic.GameMap[i,j])
                            {
                                case GameLogic.FieldType.field:
                                    polygon.Fill = new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/field.png", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.FieldType.water:
                                    polygon.Fill = new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/water.png", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.FieldType.village:
                                    break;
                                case GameLogic.FieldType.hill:
                                    polygon.Fill = new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/mountain.png", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.FieldType.forest:
                                    polygon.Fill = new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/wood.png", UriKind.RelativeOrAbsolute)));
                                    break;
                                case GameLogic.FieldType.wheat:
                                    polygon.Fill = new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/food.png", UriKind.RelativeOrAbsolute)));
                                    break;
                                default:
                                    break;
                            }
                            grid.Children.Add(polygon);
                        }
                    }
                }
            }
        }

        private void Polygon_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Polygon).Stroke = Brushes.Transparent;
        }


        private void Polygon_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Polygon).Stroke = Brushes.Turquoise;
        }

        private void Polygon_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (sender as Polygon).Fill = Brushes.Black;
        }
    }
}
