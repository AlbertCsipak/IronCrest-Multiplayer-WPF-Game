using GUI_20212022_Z6O9JF.UserControls;
using System.Windows;

namespace GUI_20212022_Z6O9JF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cc.Content = new MenuUC();
        }
    }
}
