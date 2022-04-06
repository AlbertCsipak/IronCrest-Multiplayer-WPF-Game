﻿using GUI_20212022_Z6O9JF.Logic;
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

                            switch (gameLogic.GameMap[i, j].FieldType)
                            {
                                case FieldType.field:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/field.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case FieldType.water:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/water.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case FieldType.village:
                                    drawingContext.DrawRectangle(Brushes.Green, new Pen(Brushes.Black, 1), rect);
                                    break;
                                case FieldType.hill:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/mountain_water.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case FieldType.forest:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/wood.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                case FieldType.wheat:
                                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/food.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                                    break;
                                default:
                                    break;
                            }
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
                            }

                            foreach (var item in gameLogic.GameMap[i, j].Objects.ToList())
                            {
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Characters/bjorn.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), rect);
                            }
                        }
                    }
                }
                sizeChanged = false;
            }
        }
    }
}
