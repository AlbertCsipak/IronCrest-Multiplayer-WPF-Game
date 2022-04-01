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
        public void LogicSetup(IGameLogic gameLogic) { this.gameLogic = gameLogic; }
        public void Resize(Size size) { this.size = size; }
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (gameLogic != null)
            {
                double rowHeight = size.Height / gameLogic.GameMap.GetLength(0);
                double colWidth = size.Width / gameLogic.GameMap.GetLength(1);

                for (int i = 0; i < gameLogic.GameMap.GetLength(0); i++)
                {
                    for (int j = 0; j < gameLogic.GameMap.GetLength(1); j++)
                    {
                        switch (gameLogic.GameMap[i, j])
                        {
                            case GameLogic.FieldType.field:
                                drawingContext.DrawRectangle(
                                 new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/field.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0.5), new Rect(j * colWidth, i * rowHeight, colWidth, rowHeight));
                                break;
                            case GameLogic.FieldType.water:
                                drawingContext.DrawRectangle(
                                 new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/water.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0.5), new Rect(j * colWidth, i * rowHeight, colWidth, rowHeight));
                                break;
                            case GameLogic.FieldType.forest:
                                drawingContext.DrawRectangle(
                                  new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/wood.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0.5), new Rect(j * colWidth, i * rowHeight, colWidth, rowHeight));
                                break;
                            case GameLogic.FieldType.hill:
                                drawingContext.DrawRectangle(
                                  new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/mountain.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0.5), new Rect(j * colWidth, i * rowHeight, colWidth, rowHeight));
                                break;
                            case GameLogic.FieldType.wheat:
                                drawingContext.DrawRectangle(
                                   new ImageBrush(new BitmapImage(new Uri("Resources/Images/Map/food.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0.5), new Rect(j * colWidth, i * rowHeight, colWidth, rowHeight));
                                break;
                            default:
                                break;
                        }
                    }
                }
                //foreach (var player in gameLogic.Players)
                //{
                //    foreach (var item in player.Units)
                //    {

                //    }
                //    foreach (var item in player.Villages)
                //    {

                //    }
                //    foreach (var item in player.Heroes)
                //    {

                //    }
                //}
            }


        }

    }
}
