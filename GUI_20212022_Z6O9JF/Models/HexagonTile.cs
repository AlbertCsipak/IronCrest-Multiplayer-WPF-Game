using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_20212022_Z6O9JF.Models
{

    public enum FieldType { field, water, village, hill, forest, wheat }
    public class HexagonTile
    {
        public FieldType FieldType {get;set;}
        public ObservableCollection<object> Objects { get; set; }
        public HexagonTile()
        {

        }
    }
}
