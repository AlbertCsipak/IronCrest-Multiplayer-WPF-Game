using GUI_20212022_Z6O9JF.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GUI_20212022_Z6O9JF.ViewModel
{
    public class ItemMenu
    {
        public ItemMenu(string header, List<SubItem<Quest>> subItems, PackIconKind icon)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
        }

        public ItemMenu(string header, UserControl screen, PackIconKind icon)
        {
            Header = header;
            Screen = screen;
            Icon = icon;
        }

        public string Header { get; private set; }
        public PackIconKind Icon { get; private set; }
        public List<SubItem<Quest>> SubItems { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
