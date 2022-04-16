using GUI_20212022_Z6O9JF.Logic;
using System;
using System.Collections.Generic;
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
        public void LogicSetup(IClientLogic clientLogic, IGameLogic gameLogic, IControlLogic controlLogic, Grid grid)
        {
            this.grid = grid;
            this.clientLogic = clientLogic;
            this.gameLogic = gameLogic;
            this.controlLogic = controlLogic;
            size = new Size(grid.Width / 15, grid.Height / 15);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            ;
            switch (gameLogic.Players.Where(t => t.PlayerID == gameLogic.ClientID).FirstOrDefault().Faction)
            {
                case Models.Faction.Viking:
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Characters/standing_viking.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(10,grid.Height/5), size ));
                    break;
                case Models.Faction.Crusader:
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Characters/standing_crusader.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(10, grid.Height / 5), size));
                    break;
                case Models.Faction.Mongolian:
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Characters/standing_mongolian.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(10, grid.Height / 5), size));
                    break;
                case Models.Faction.Arabian:
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri("Resources/Images/Characters/standing_arabian.png", UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(10, grid.Height / 5), size));
                    break;
                default:
                    break;
            }
            
        }
    }
}
