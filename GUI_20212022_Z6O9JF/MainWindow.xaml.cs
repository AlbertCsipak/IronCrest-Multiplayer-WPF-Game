using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Media;
using System.Windows;
using System.Windows.Media;

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
            this.DataContext = new MainWindowViewModel();
            music.Play();
        }
    }
}
