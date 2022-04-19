using System.Windows.Controls;

namespace GUI_20212022_Z6O9JF.ViewModel
{
    public class SubItem<T>
    {
        public SubItem(T element, UserControl screen = null)
        {
            Element = element;
            Screen = screen;
        }

        public T Element { get; private set; }
        public UserControl Screen { get; private set; }
    }
}