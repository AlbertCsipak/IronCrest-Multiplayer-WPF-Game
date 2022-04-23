using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF.UserControls
{
    public partial class TradeUC : UserControl
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        public MediaPlayer button_click = new MediaPlayer();
        public int firstlySelectedOffer;
        public int offerCount = 0;
        Uri uriSourceflag;
        Uri uriSourcedarkened_flag;
        public TradeUC()
        {
            InitializeComponent();
            this.DataContext = new TradeViewModel();
            this.gameLogic = (this.DataContext as TradeViewModel).gameLogic;
            this.clientLogic = (this.DataContext as TradeViewModel).clientLogic;
            this.controlLogic = (this.DataContext as TradeViewModel).controlLogic;
            var trade = gameLogic.Game.Players.Where(t => t.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade;
            uriSourceflag = new Uri(@"\Resources\Images\Menu\flag.png", UriKind.Relative);
            uriSourcedarkened_flag = new Uri(@"\Resources\Images\Menu\flag_darken.png", UriKind.Relative);
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
                flag3_img.Source = new BitmapImage(new Uri(@"\Resources\Images\Menu\flag_disabled.png", UriKind.RelativeOrAbsolute));
            }
        }

        private void check1_Click(object sender, RoutedEventArgs e)
        {

            if (gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Faction != Models.Faction.Arabian)
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
            else //arabian 
            {
                ;
                if ((bool)(sender as CheckBox).IsChecked)
                {
                    switch ((sender as CheckBox).Name)
                    {
                        case "check1":
                            flag1_img.Source = new BitmapImage(uriSourceflag);

                            if (offerCount < 2)
                            {
                                offerCount++;
                            }


                            break;
                        case "check2":
                            flag2_img.Source = new BitmapImage(uriSourceflag);

                            if (offerCount < 2)
                            {
                                offerCount++;
                            }

                            break;
                        case "check3":
                            flag3_img.Source = new BitmapImage(uriSourceflag);

                            if (offerCount < 2)
                            {
                                offerCount++;
                            }

                            break;
                    }
                    if (offerCount == 2)
                    {
                        DisableThirdOffer();
                    }
                }
                else if (!(bool)(sender as CheckBox).IsChecked)
                {
                    var uriSource = new Uri(@"\Resources\Images\Menu\flag_darken.png", UriKind.Relative);
                    switch ((sender as CheckBox).Name)
                    {
                        case "check1":
                            flag1_img.Source = new BitmapImage(uriSource);
                            offerCount--;
                            switch (firstlySelectedOffer)
                            {
                                case 1:
                                    flag1_img.Source = new BitmapImage(uriSource);
                                    check1.IsChecked = false;
                                    break;
                                case 2:
                                    flag1_img.Source = new BitmapImage(uriSource);
                                    check1.IsChecked = false;
                                    break;
                                case 3:
                                    flag1_img.Source = new BitmapImage(uriSource);
                                    check1.IsChecked = false;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "check2":
                            flag2_img.Source = new BitmapImage(uriSource);
                            offerCount--;
                            switch (firstlySelectedOffer)
                            {
                                case 1:
                                    flag1_img.Source = new BitmapImage(uriSource);
                                    check1.IsChecked = false;
                                    break;
                                case 2:
                                    flag1_img.Source = new BitmapImage(uriSource);
                                    check1.IsChecked = false;
                                    break;
                                case 3:
                                    flag1_img.Source = new BitmapImage(uriSource);
                                    check1.IsChecked = false;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "check3":
                            flag3_img.Source = new BitmapImage(uriSource);
                            offerCount--;
                            switch (firstlySelectedOffer)
                            {
                                case 1:
                                    flag1_img.Source = new BitmapImage(uriSource);
                                    check1.IsChecked = false;
                                    break;
                                case 2:
                                    flag1_img.Source = new BitmapImage(uriSource);
                                    check1.IsChecked = false;
                                    break;
                                case 3:
                                    flag1_img.Source = new BitmapImage(uriSource);
                                    check1.IsChecked = false;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    if (offerCount < 2)
                    {
                        if (gameLogic.HasSufficientResources(0))
                        {
                            check1.IsEnabled = true;
                        }
                        if (gameLogic.HasSufficientResources(1))
                        {
                            check2.IsEnabled = true;
                        }
                        if (gameLogic.HasSufficientResources(2))
                        {
                            check3.IsEnabled = true;
                        }
                    }
                }
            }
            button_click.Open(new Uri("Resources/Music/button.mp3", UriKind.RelativeOrAbsolute));
            button_click.Play();
        }
        private void DisableThirdOffer()
        {
            if ((bool)check1.IsChecked && (bool)check2.IsChecked)
            {
                flag3_img.Source = new BitmapImage(uriSourcedarkened_flag);
                check3.IsEnabled = false;
            }
            else if ((bool)check1.IsChecked && (bool)check3.IsChecked)
            {
                flag2_img.Source = new BitmapImage(uriSourcedarkened_flag);
                check2.IsEnabled = false;
            }
            else if ((bool)check2.IsChecked && (bool)check3.IsChecked)
            {
                flag1_img.Source = new BitmapImage(uriSourcedarkened_flag);
                check1.IsEnabled = false;
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Faction == Models.Faction.Arabian)//arab faction 2 offert fogadhat el
            {

                if ((bool)check1.IsChecked && (bool)check2.IsChecked)
                {
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(0);
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(1);
                }
                else if ((bool)check1.IsChecked && (bool)check3.IsChecked)
                {
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(0);
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(2);
                }
                else if ((bool)check2.IsChecked && (bool)check3.IsChecked)
                {
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(1);
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(2);
                }
                else if ((bool)check1.IsChecked)
                {
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(0);
                }
                else if ((bool)check2.IsChecked)
                {
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(1);
                }
                else if ((bool)check3.IsChecked)
                {
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(2);
                }
            }
            else
            {
                if ((bool)check1.IsChecked)
                {
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(0);
                }
                else if ((bool)check2.IsChecked)
                {
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(1);
                }
                else if ((bool)check3.IsChecked)
                {
                    gameLogic.Game.Players.Where(x => x.PlayerID == clientLogic.ClientId).FirstOrDefault().Trade.SelectedOfferIndexes.Add(2);
                }
            }
        }
    }
}
