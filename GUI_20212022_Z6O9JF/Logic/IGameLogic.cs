using GUI_20212022_Z6O9JF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace GUI_20212022_Z6O9JF.Logic
{
    public interface IGameLogic
    {
        List<Faction> AvailableFactions { get; set; }
        GameLogic.FieldType[,] GameMap { get; set; }
        string Map { get; set; }
        ObservableCollection<Player> Players { get; set; }

        GameLogic.FieldType[,] GameMapSetup(string path);
        void Move(double[,][] HexagonPoints, Point point, double width, double height);
    }
}