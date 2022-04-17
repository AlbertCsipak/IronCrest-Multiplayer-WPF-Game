using GUI_20212022_Z6O9JF.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF.Renderer
{
    public class GoldMineDisplay : FrameworkElement
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        Size size;
        Grid grid;
        bool sizeChanged;
        public int XPos;
        public void LogicSetup(IClientLogic clientLogic, IGameLogic gameLogic, IControlLogic controlLogic, Grid grid)
        {
            this.grid = grid;
            this.clientLogic = clientLogic;
            this.gameLogic = gameLogic;
            this.controlLogic = controlLogic;
            sizeChanged = true;
            XPos = 0;
        }
        public void Resize(Size size)
        {
            this.size = size;
            sizeChanged = true;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            ;
            if (size.Width!=0 && size.Height!= 0)
            {
                double num = size.Width / 100 * XPos;
                switch (gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).FirstOrDefault().Faction)
                {
                    case Models.Faction.Viking:
                        switch (XPos%3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "standing_viking.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(num, size.Height / 10 * 6.2), new Size(size.Width / 15, size.Height / 9)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "standing_viking.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(num, size.Height / 10 * 6.2), new Size(size.Width / 15, size.Height / 9)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "standing_viking.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(num, size.Height / 10 * 6.2), new Size(size.Width / 15, size.Height / 9)));
                                break;
                        }
                        break;
                    case Models.Faction.Crusader:
                        switch (XPos % 3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_crusader1.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(num, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_crusader2.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(num, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_crusader3.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(num, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                        }
                        break;
                    case Models.Faction.Mongolian:
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(@"\Resources/Images/Characters/standing_mongolian.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(10, grid.Height / 5), size));
                        break;
                    case Models.Faction.Arabian:
                        switch (XPos % 3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_arabian1.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(num, size.Height / 10 * 5.8), new Size(size.Width / 15, size.Height / 9)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_arabian2.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(num, size.Height / 10 * 5.8), new Size(size.Width / 15, size.Height / 9)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_arabian3.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(num, size.Height / 10 * 5.8), new Size(size.Width / 15, size.Height / 9)));
                                break;
                        }
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(@"\Resources/Images/Characters/standing_arabian.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(10, grid.Height / 5), size));
                        break;
                    default:
                        break;
                }
                if (num <= size.Width)
                {
                    XPos++;
                }
                else
                {
                    clientLogic.ChangeView("game");
                }
            }
            
            sizeChanged = false;
        }
    }

}
