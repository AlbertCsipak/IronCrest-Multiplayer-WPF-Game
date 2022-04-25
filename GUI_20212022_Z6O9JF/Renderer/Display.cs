using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI_20212022_Z6O9JF.Renderer
{
    public class Display : FrameworkElement
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        Size size;
        Grid grid;
        double[,][] HexagonPoints;
        bool sizeChanged;
        int vizgif;
        int felhogif;
        static Random random;
        public Display()
        {

        }
        public void LogicSetup(IClientLogic clientLogic, IGameLogic gameLogic, IControlLogic controlLogic, Grid grid)
        {
            this.grid = grid;
            this.clientLogic = clientLogic;
            this.gameLogic = gameLogic;
            this.controlLogic = controlLogic;
            HexagonPoints = new double[gameLogic.GameMap.GetLength(0), gameLogic.GameMap.GetLength(1)][];
            sizeChanged = true;
            controlLogic.grid = grid;
        }
        public void Resize(Size size)
        {
            this.size = size;
            GetCoordinates();
            InvalidateVisual();
            sizeChanged = true;
        }
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
                        if (j % 2 == 1)
                        {
                            HexagonPoints[i, j][0] = i * rectHeight + rectHeight * 1.5 / 2 + rectHeight / 4;
                            HexagonPoints[i, j][1] = j * rectWidth + rectWidth / 2;
                        }
                        else
                        {
                            HexagonPoints[i, j][0] = i * rectHeight + rectHeight / 4 + rectHeight / 4;
                            HexagonPoints[i, j][1] = j * rectWidth + rectWidth / 2;
                        }
                    }
                }
            }
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (HexagonPoints[0, 0] != null)
            {
                double height = size.Height / HexagonPoints.GetLength(0);
                double width = size.Width / HexagonPoints.GetLength(1);
                for (int i = 0; i < HexagonPoints.GetLength(0); i++)
                {
                    for (int j = 0; j < HexagonPoints.GetLength(1); j++)
                    {
                        if (HexagonPoints[i, j][0] != 0 && HexagonPoints[i, j][1] != 0)
                        {
                            Rect rect = new Rect();
                            rect.Location = new Point(HexagonPoints[i, j][1] - width / 2 * 1.3, HexagonPoints[i, j][0] - height / 2 * 1.1);
                            rect.Width = width * 1.3;
                            rect.Height = height * 1.1;
                            random = new Random();
                            switch (gameLogic.GameMap[i, j].FieldType)
                            {
                                case FieldType.grass:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri($"Resources/Images/Map/grass.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case FieldType.ocean:
                                    if (vizgif < 30)
                                    {
                                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/water.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    }
                                    else
                                    {
                                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/water2.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    }
                                    break;
                                case FieldType.lake:
                                    if (vizgif < 30)
                                    {
                                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/water.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    }
                                    else
                                    {
                                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/water2.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    }
                                    break;
                                case FieldType.mountain:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/mountain.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case FieldType.forest:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/forest.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case FieldType.wheat:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/wheat.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case FieldType.goldMine:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/goldMine.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case FieldType.compassField:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/grass.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                default:
                                    break;
                            }
                            //drawingContext.DrawText(new FormattedText(gameLogic.GameMap[i, j].Position[0].ToString() + "," + gameLogic.GameMap[i, j].Position[1].ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black), new Point(HexagonPoints[i, j][1] - width / 2 * 1.3, HexagonPoints[i, j][0] - height / 2 * 1.1));

                            if (sizeChanged)
                            {
                                Polygon polygon = new Polygon();
                                PointCollection points = new PointCollection();

                                points.Add(new Point(HexagonPoints[i, j][1] - width / 1.5, HexagonPoints[i, j][0]));
                                points.Add(new Point(HexagonPoints[i, j][1] - width / 3, HexagonPoints[i, j][0] - height / 2));
                                points.Add(new Point(HexagonPoints[i, j][1] + width / 3, HexagonPoints[i, j][0] - height / 2));
                                points.Add(new Point(HexagonPoints[i, j][1] + width / 1.5, HexagonPoints[i, j][0]));
                                points.Add(new Point(HexagonPoints[i, j][1] + width / 3, HexagonPoints[i, j][0] + height / 2));
                                points.Add(new Point(HexagonPoints[i, j][1] - width / 3, HexagonPoints[i, j][0] + height / 2));

                                polygon.Points = points;

                                polygon.AllowDrop = true;
                                polygon.Stroke = Brushes.Transparent;
                                polygon.Fill = Brushes.Transparent;
                                polygon.StrokeThickness = 3;
                                polygon.IsManipulationEnabled = true;
                                polygon.Tag = gameLogic.GameMap[i, j];

                                polygon.MouseLeftButtonDown += controlLogic.Polygon_MouseLeftButtonDown;
                                polygon.MouseEnter += controlLogic.Polygon_MouseEnter;
                                polygon.MouseLeave += controlLogic.Polygon_MouseLeave;
                                polygon.MouseRightButtonDown += controlLogic.Polygon_MouseRightButtonDown;

                                grid.Children.Add(polygon);
                                (polygon.Tag as HexagonTile).ParentId = grid.Children.IndexOf(polygon);

                            }
                            ;
                            foreach (var item in gameLogic.GameMap[i, j].Objects.ToList())
                            {
                                if (item is Village)
                                {
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri($"Resources/Images/Map/{item.FactionType}_village_lvl{(item as Village).Level}.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                }
                            }
                            if (gameLogic.GameMap[i, j].Compass != null)
                            {
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Other/compass.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(HexagonPoints[i, j][1] + (width / 2 * 1.3) - (width / 2 * 1.7), HexagonPoints[i, j][0] - height / 2 * 1.1 + (height * 0.5)), new Size(width * 0.4, height * 0.4)));
                            }
                            foreach (var item in gameLogic.GameMap[i, j].Objects.ToList())
                            {
                                if (item is Unit)
                                    switch (item.Level)
                                    {
                                        case 1:
                                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri($"Resources/Images/Characters/standing_{item.FactionType}_lvl{item.Level}.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(HexagonPoints[i, j][1] - width / 2 * 1.3 + (width * 0.7 / 2), HexagonPoints[i, j][0] - height / 2 * 1.1 + (height * 0.8 / 5)), new Size(width * 0.7, height * 0.8)));
                                            break;
                                        case 2:
                                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri($"Resources/Images/Characters/standing_{item.FactionType}_lvl{item.Level}.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(HexagonPoints[i, j][1] - width / 2 * 1.3 + (width * 0.7 / 2), HexagonPoints[i, j][0] - height / 2 * 1.1 + (height * 0.8 / 5)), new Size(width * 0.7, height * 0.8)));
                                            break;
                                        case 3:
                                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri($"Resources/Images/Characters/standing_{item.FactionType}_lvl{item.Level}.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(HexagonPoints[i, j][1] - width / 2 * 1.3 + (width * 0.7 / 2), HexagonPoints[i, j][0] - height / 2 * 1.1 + (height * 0.8 / 5)), new Size(width * 0.7, height * 0.8)));
                                            break;
                                        default:
                                            break;
                                    }
                                {
                                }
                            }
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri($"Resources/Images/Map/clouds.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(-250+felhogif,size.Height/6,150,150));
                        }
                    }
                }
                vizgif++;
                if (vizgif > 60)
                {
                    vizgif = 0;
                }
                felhogif++;
                if (felhogif > size.Width+200)
                {
                    felhogif = 0;
                }
                sizeChanged = false;
            }
            //drawingContext.DrawImage(new BitmapImage(new Uri("Resources/Images/Map/cloud_bottom.png", UriKind.RelativeOrAbsolute)), new Rect(0, size.Height - size.Height / 7, size.Width, size.Height / 7));
            //drawingContext.DrawImage(new BitmapImage(new Uri("Resources/Images/Map/cloud_up.png", UriKind.RelativeOrAbsolute)), new Rect(0, 0, size.Width, size.Height / 7));
            //drawingContext.DrawImage(new BitmapImage(new Uri("Resources/Images/Map/cloud_left.png", UriKind.RelativeOrAbsolute)), new Rect(0, 0, size.Width / 9, size.Height));
            //drawingContext.DrawImage(new BitmapImage(new Uri("Resources/Images/Map/cloud_right.png", UriKind.RelativeOrAbsolute)), new Rect(size.Width - size.Width / 9, 0, size.Width / 9, size.Height));

        }
    }
}
