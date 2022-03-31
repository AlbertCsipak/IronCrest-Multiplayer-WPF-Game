using GUI_20212022_Z6O9JF.Logic;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
using System.Windows.Shapes;

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
                //Color greenishBlueColor = (Color)ColorConverter.ConvertFromString("#B3C8B7");
                //FormattedText start_game_text = new FormattedText("START GAME", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("Poor Richard"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 80, new SolidColorBrush(greenishBlueColor));
                //Point point = new Point((size.Width / 2) - (start_game_text.Width / 2), (size.Height / 4) * 3.5);
                //drawingContext.DrawText(start_game_text, point);
                //Rectangle rec = new Rectangle();
                //drawingContext.DrawGeometry(Brushes.White, new Pen(Brushes.Black,1 ), rec.);
                //rec.MouseLeftButtonDown += Rec_MouseLeftButtonDown;
            }
        }
    }
}
