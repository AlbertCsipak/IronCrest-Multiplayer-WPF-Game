using GUI_20212022_Z6O9JF.Logic;
using System;
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
                Color greenishBlueColor = (Color)ColorConverter.ConvertFromString("#B3C8B7");
                drawingContext.DrawText(new FormattedText("START GAME", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("Poor Richard"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 80, new SolidColorBrush(greenishBlueColor)), new Point(100,100));
            }
        }
    }
}
