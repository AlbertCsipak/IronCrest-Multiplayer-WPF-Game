using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF.Renderer
{
    public class BattleDisplay : FrameworkElement
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        Size size;
        Grid grid;
        Battle battle { get { return gameLogic.Game.CurrentBattle; } }
        bool sizeChanged;
        public int AttackerXPos;
        public int DefenderXPos;


        public void LogicSetup(IClientLogic clientLogic, IGameLogic gameLogic, IControlLogic controlLogic, Grid grid)
        {
            this.grid = grid;
            this.clientLogic = clientLogic;
            this.gameLogic = gameLogic;
            this.controlLogic = controlLogic;
            sizeChanged = true;
            controlLogic.grid = grid;
            AttackerXPos = 0;
            DefenderXPos = (int)size.Width;
        }
        public void Resize(Size size)
        {
            this.size = size;
            sizeChanged = true;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (size.Width != 0 && size.Height != 0)
            {
                double attackernum = size.Width / 100 * AttackerXPos;
                double defendernum = size.Width / 100 * DefenderXPos;
                switch (battle.Defender.Faction)
                {
                    case Faction.Viking:
                        switch (DefenderXPos % 3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_viking1_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_viking2_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_viking3_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                        }
                        break;
                    case Faction.Crusader:
                        switch (DefenderXPos % 3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_crusader1_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_crusader2_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_crusader3_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                        }
                        break;
                    case Faction.Mongolian:
                        switch (DefenderXPos % 3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_mongolian1_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.6), new Size(size.Width / 16, size.Height / 6)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_mongolian2_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.6), new Size(size.Width / 16, size.Height / 6)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_mongolian3_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.6), new Size(size.Width / 16, size.Height / 6)));
                                break;
                        }
                        break;
                    case Faction.Arabian:
                        switch (DefenderXPos % 3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_arabian1_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_arabian2_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_arabian3_reversed.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(defendernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                        }
                        break;
                }


                switch (battle.Attacker.Faction)
                {
                    case Faction.Viking:
                        switch (AttackerXPos % 3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_viking1.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_viking2.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_viking3.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                        }
                        break;
                    case Faction.Crusader:
                        switch (AttackerXPos % 3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_crusader1.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_crusader2.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_crusader3.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                        }
                        break;
                    case Faction.Mongolian:
                        switch (AttackerXPos % 3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_mongolian1.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.6), new Size(size.Width / 16, size.Height / 6)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_mongolian2.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.6), new Size(size.Width / 16, size.Height / 6)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_mongolian3.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.6), new Size(size.Width / 16, size.Height / 6)));
                                break;
                        }
                        break;
                    case Faction.Arabian:
                        switch (AttackerXPos % 3)
                        {
                            case 0:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_arabian1.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 1:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_arabian2.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                            case 2:
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_arabian3.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
                                break;
                        }
                        break;
                }
                if (attackernum <= size.Width / 2)
                {
                    AttackerXPos++;
                }
                if (defendernum >= size.Width / 2)
                {
                    DefenderXPos--;
                }

                //robbanó kép
                //drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Resources", "Images", "Characters", "walking_arabian1.png"), UriKind.RelativeOrAbsolute))), new Pen(Brushes.Black, 0), new Rect(new Point(attackernum, size.Height / 10 * 5.8), new Size(size.Width / 16, size.Height / 7)));
            }

            sizeChanged = false;
        }

    }
}
