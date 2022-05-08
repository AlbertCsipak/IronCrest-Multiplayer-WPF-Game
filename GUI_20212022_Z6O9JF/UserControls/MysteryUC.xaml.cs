using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for MysteryUC.xaml
    /// </summary>
    public partial class MysteryUC : UserControl
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        public MediaPlayer button_click = new MediaPlayer();
        public MysteryUC()
        {
            InitializeComponent();
            this.DataContext = new MysteryViewModel();
            this.gameLogic = (this.DataContext as MysteryViewModel).gameLogic;
            this.clientLogic = (this.DataContext as MysteryViewModel).clientLogic;
            this.controlLogic = (this.DataContext as MysteryViewModel).controlLogic;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Closing += window_Closing;
            ;
        }

        void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {
            //GameUC.IsInSubWindow = false;
            //ReleaseMouseCapture();
        }
    }
}
