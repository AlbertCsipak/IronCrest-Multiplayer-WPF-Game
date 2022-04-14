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
            if (!gameLogic.HasSufficientResources(0))
            {
                check1.IsEnabled = false;
                flag1_img.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\flag_disabled.png", UriKind.RelativeOrAbsolute));
            }
            if (!gameLogic.HasSufficientResources(1))
            {
                check2.IsEnabled = false;
                flag2_img.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\flag_disabled.png", UriKind.RelativeOrAbsolute));
            }
            if (!gameLogic.HasSufficientResources(2))
            {
                check3.IsEnabled = false;
                flag3_img.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\flag_disabled.png",UriKind.RelativeOrAbsolute));
            }
            count = 0;
        }

        private void check1_Click(object sender, RoutedEventArgs e)
        {
            var uriSourceflag = new Uri(@"\Resources\Images\Menu\flag.png", UriKind.Relative);
            var uriSourcedarkened_flag = new Uri(@"\Resources\Images\Menu\flag_darken.png", UriKind.Relative);
            if (gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Faction != Models.Faction.Arabian)
            {
                if ((bool)(sender as CheckBox).IsChecked)
                {
                    switch ((sender as CheckBox).Name)
                    {
                        case "check1":
                            flag1_img.Source = new BitmapImage(uriSourceflag);
                            if (check2.IsEnabled)
                            {
                                check2.IsChecked = false;
                                flag2_img.Source = new BitmapImage(uriSourcedarkened_flag);
                            }
                            if (check3.IsEnabled)
                            {
                                check3.IsChecked = false;
                                flag3_img.Source = new BitmapImage(uriSourcedarkened_flag);
                            }
                            break;
                        case "check2":
                            flag2_img.Source = new BitmapImage(uriSourceflag);
                            if (check1.IsEnabled)
                            {
                                check1.IsChecked = false;
                                flag1_img.Source = new BitmapImage(uriSourcedarkened_flag);
                            }
                            if (check3.IsEnabled)
                            {
                                check3.IsChecked = false;
                                flag3_img.Source = new BitmapImage(uriSourcedarkened_flag);
                            }
                            break;
                        case "check3":
                            flag3_img.Source = new BitmapImage(uriSourceflag);
                            if (check1.IsEnabled)
                            {
                                check1.IsChecked = false;
                                flag1_img.Source = new BitmapImage(uriSourcedarkened_flag);
                            }
                            if (check2.IsEnabled)
                            {
                                check2.IsChecked = false;
                                flag2_img.Source = new BitmapImage(uriSourcedarkened_flag);
                            }
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
            }
            else
            {
                if ((bool)(sender as CheckBox).IsChecked)
                {
                    switch ((sender as CheckBox).Name)
                    {
                        case "check1":
                            flag1_img.Source = new BitmapImage(uriSourceflag);
                            break;
                        case "check2":
                            flag2_img.Source = new BitmapImage(uriSourceflag);
                            break;
                        case "check3":
                            flag3_img.Source = new BitmapImage(uriSourceflag);
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
            }
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Faction==Models.Faction.Arabian)//arab faction 2 offert fogadhat el
            {
                
                if ((bool)check1.IsChecked && (bool)check2.IsChecked)
                {
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(0);
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(1);
                }
                else if ((bool)check1.IsChecked && (bool)check3.IsChecked)
                {
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(0);
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(2);
                }
                else if ((bool)check2.IsChecked && (bool)check3.IsChecked)
                {
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(1);
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(2);
                }
                else if ((bool)check1.IsChecked)
                {
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(0);
                }
                else if ((bool)check2.IsChecked)
                {
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(1);
                }
                else if ((bool)check3.IsChecked)
                {
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(2);
                }
            }
            else
            {
                if ((bool)check1.IsChecked)
                {
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(0);
                }
                else if ((bool)check2.IsChecked)
                {
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(1);
                }
                else if ((bool)check3.IsChecked)
                {
                    gameLogic.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(2);
                }
            }
        }
    }
}
