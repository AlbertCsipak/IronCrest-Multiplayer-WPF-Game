using GUI_20212022_Z6O9JF.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GUI_20212022_Z6O9JF.Renderer
{
    public class MainMenuDisplay : FrameworkElement
    {
        IGameLogic gameLogic;
        Size size;
        public void LogicSetup(IGameLogic gameLogic) { this.gameLogic = gameLogic; }
        public void Resize(Size size) { this.size = size; }
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (gameLogic != null)
            {
                MediaPlayer player = new MediaPlayer(); 
                player.Open(new Uri("Images/Menu/main_background.gif", UriKind.RelativeOrAbsolute)); 
                player.Play(); drawingContext.DrawVideo(player, new Rect(0, 0, size.Width, size.Height));
            }
        }
    }
}
