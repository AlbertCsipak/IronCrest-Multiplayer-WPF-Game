using GUI_20212022_Z6O9JF.Logic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI_20212022_Z6O9JF.Renderer
{
    public class Display : FrameworkElement
    {
        IGameLogic gameLogic;
        Size size;
        Grid grid;
        double[,][] HexagonPoints;
        public void LogicSetup(IGameLogic gameLogic,Grid grid)
        {
            this.grid = grid;
            this.gameLogic = gameLogic;
            HexagonPoints = new double[gameLogic.GameMap.GetLength(0), gameLogic.GameMap.GetLength(1)][];
        }
        public void Resize(Size size) {
            this.size = size;
            GetCoordinates();
        }
        public void GetCoordinates()
        {
            
            double rectHeight = size.Height / HexagonPoints.GetLength(0);
            double rectWidth = size.Width / HexagonPoints.GetLength(1);
            ;
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

                            Rect rect = new Rect();
                            rect.Location = new Point(HexagonPoints[i, j][0] - width * 0.65, HexagonPoints[i, j][1] - height*1.1);
                            rect.Width = width + width*0.3;
                            rect.Height = height + height*1.1;


                            switch (gameLogic.GameMap[i, j].FieldType)
                            {
                                case Models.FieldType.field:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/field.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case Models.FieldType.water:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/water.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case Models.FieldType.village:
                                    drawingContext.DrawRectangle(Brushes.Green, new Pen(Brushes.Black, 1), rect);
                                    break;
                                case Models.FieldType.hill:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/mountain.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case Models.FieldType.forest:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/wood.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case Models.FieldType.wheat:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/food.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                for (int i = 0; i < HexagonPoints.GetLength(0); i++)
                {
                    for (int j = 0; j < HexagonPoints.GetLength(1); j++)
                    {
                        if (HexagonPoints[i, j][0] != 0 && HexagonPoints[i, j][1] != 0)
                        {
                            Polygon polygon = new Polygon();
                            PointCollection points = new PointCollection();

                            points.Add(new Point(HexagonPoints[i, j][0] - width / 1.5, HexagonPoints[i, j][1]));
                            points.Add(new Point(HexagonPoints[i, j][0] - width / 3, HexagonPoints[i, j][1] - height));
                            points.Add(new Point(HexagonPoints[i, j][0] + width / 3, HexagonPoints[i, j][1] - height));
                            points.Add(new Point(HexagonPoints[i, j][0] + width / 1.5, HexagonPoints[i, j][1]));
                            points.Add(new Point(HexagonPoints[i, j][0] + width / 3, HexagonPoints[i, j][1] + height));
                            points.Add(new Point(HexagonPoints[i, j][0] - width / 3, HexagonPoints[i, j][1] + height));

                            polygon.Points = points;

                            polygon.AllowDrop = true;
                            polygon.Stroke = Brushes.Transparent;
                            polygon.Fill = Brushes.Transparent;
                            polygon.StrokeThickness = 2.5;
                            polygon.IsManipulationEnabled = true;
                            polygon.Tag = gameLogic.GameMap[i, j];

                            polygon.MouseLeftButtonDown += Polygon_MouseLeftButtonDown;
                            polygon.MouseEnter += Polygon_MouseEnter;
                            polygon.MouseLeave += Polygon_MouseLeave;

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
