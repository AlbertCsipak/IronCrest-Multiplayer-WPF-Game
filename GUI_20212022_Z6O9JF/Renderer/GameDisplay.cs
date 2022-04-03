using GUI_20212022_Z6O9JF.Logic;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF.Renderer
{
    public class GameDisplay : FrameworkElement
    {
        IGameLogic gameLogic;
        Size size;
        double[,][] HexagonPoints;
        public GameDisplay()
        {
            
        }
        public void LogicSetup(IGameLogic gameLogic) { 
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
                            StreamGeometry geometry = new StreamGeometry();
                            using (StreamGeometryContext ctx = geometry.Open())
                            {
                                ctx.BeginFigure(new Point(HexagonPoints[i, j][0] - width / 1.5, HexagonPoints[i, j][1]), true, true);
                                ctx.LineTo(new Point(HexagonPoints[i, j][0] - width / 3, HexagonPoints[i, j][1] - height), true, false);
                                ctx.LineTo(new Point(HexagonPoints[i, j][0] + width / 3, HexagonPoints[i, j][1] - height), true, false);
                                ctx.LineTo(new Point(HexagonPoints[i, j][0] + width / 1.5, HexagonPoints[i, j][1]), true, false);
                                ctx.LineTo(new Point(HexagonPoints[i, j][0] + width / 3, HexagonPoints[i, j][1] + height), true, false);
                                ctx.LineTo(new Point(HexagonPoints[i, j][0] - width / 3, HexagonPoints[i, j][1] + height), true, false);
                            }
                            geometry.Freeze();
                            Rect rect = geometry.Bounds;

                            //a kövi 3 csak visual bug fix
                            rect.Height = rect.Height + height / 2;
                            rect.Width = rect.Width + width / 16;
                            rect.Y = rect.Y - height / 4;

                            switch (gameLogic.GameMap[i,j])
                            {
                                case GameLogic.FieldType.field:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/field.png", UriKind.RelativeOrAbsolute))), null, rect);
                                    break;
                                case GameLogic.FieldType.water:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/water.png", UriKind.RelativeOrAbsolute))), null, rect);
                                    break;
                                case GameLogic.FieldType.village:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/field.png", UriKind.RelativeOrAbsolute))), null, rect);
                                    break;
                                case GameLogic.FieldType.hill:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/mountain.png", UriKind.RelativeOrAbsolute))), null, rect);
                                    break;
                                case GameLogic.FieldType.forest:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/wood.png", UriKind.RelativeOrAbsolute))), null, rect);
                                    break;
                                case GameLogic.FieldType.wheat:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/food.png", UriKind.RelativeOrAbsolute))), null, rect);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
