﻿using System.Collections.ObjectModel;

namespace GUI_20212022_Z6O9JF.Models
{

    public enum FieldType { field, water, village, hill, forest, wheat }
    public class HexagonTile
    {
        public FieldType FieldType { get; set; }
        public ObservableCollection<object> Objects { get; set; }
        public HexagonTile()
        {

        }
    }
}
