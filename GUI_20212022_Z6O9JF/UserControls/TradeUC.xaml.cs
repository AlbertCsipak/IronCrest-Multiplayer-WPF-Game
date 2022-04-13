using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI_20212022_Z6O9JF.UserControls
{
    public partial class TradeUC : UserControl
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        public MediaPlayer button_click = new MediaPlayer();
        int count;
        public TradeUC()
        {
            InitializeComponent();
            this.DataContext = new TradeViewModel();
            this.gameLogic = (this.DataContext as TradeViewModel).gameLogic;
            this.clientLogic = (this.DataContext as TradeViewModel).clientLogic;
            this.controlLogic = (this.DataContext as TradeViewModel).controlLogic;
            var trade = gameLogic.Players.Where(t => t.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade;
            count = 0;
        }

        private void check1_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)(sender as CheckBox).IsChecked)
            {
                var uriSourceflag = new Uri(@"\Resources\Images\Menu\flag.png", UriKind.Relative);
                var uriSourcedarkened_flag = new Uri(@"\Resources\Images\Menu\flag_darken.png", UriKind.Relative);
                switch ((sender as CheckBox).Name)
                {
                    case "check1":
                        flag1_img.Source = new BitmapImage(uriSourceflag);
                        check2.IsChecked = false;
                        check3.IsChecked = false;
                        flag2_img.Source = new BitmapImage(uriSourcedarkened_flag);
                        flag3_img.Source = new BitmapImage(uriSourcedarkened_flag);
                        break;
                    case "check2":
                        flag2_img.Source = new BitmapImage(uriSourceflag);
                        check1.IsChecked = false;
                        check3.IsChecked = false;
                        flag1_img.Source = new BitmapImage(uriSourcedarkened_flag);
                        flag3_img.Source = new BitmapImage(uriSourcedarkened_flag);
                        break;
                    case "check3":
                        flag3_img.Source = new BitmapImage(uriSourceflag);
                        check1.IsChecked = false;
                        check2.IsChecked = false;
                        flag1_img.Source = new BitmapImage(uriSourcedarkened_flag);
                        flag2_img.Source = new BitmapImage(uriSourcedarkened_flag);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                var uriSource = new Uri(@"\Resources\Images\Menu\flag_darken.png", UriKind.Relative);
                switch ((sender as CheckBox).Name)
                {
                    case "check1":
                        flag1_img.Source = new BitmapImage(uriSource);
                        break;
                    case "check2":
                        flag2_img.Source = new BitmapImage(uriSource);
                        break;
                    case "check3":
                        flag3_img.Source = new BitmapImage(uriSource);
                        break;
                    default:
                        break;
                }
            }
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
        }
    }
}
