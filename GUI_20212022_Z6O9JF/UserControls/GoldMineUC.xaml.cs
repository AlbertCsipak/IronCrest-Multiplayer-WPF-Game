﻿using GUI_20212022_Z6O9JF.Logic;
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
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.UserControls
{
    /// <summary>
    /// Interaction logic for GoldMineUC.xaml
    /// </summary>
    public partial class GoldMineUC : UserControl
    {
        IClientLogic clientLogic;
        IGameLogic gameLogic;
        IControlLogic controlLogic;
        DispatcherTimer dt;

        public GoldMineUC()
        {
            InitializeComponent();
            this.DataContext = new GoldMineViewModel();
            this.gameLogic = (this.DataContext as GoldMineViewModel).gameLogic;
            this.clientLogic = (this.DataContext as GoldMineViewModel).clientLogic;
            this.controlLogic = (this.DataContext as GoldMineViewModel).controlLogic;
            goldmineDisplay.LogicSetup(clientLogic, gameLogic, controlLogic, grid);
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);
            dt.Tick += (sender, eventargs) =>
            {
                goldmineDisplay.InvalidateVisual();
            };
            dt.Start();
            goldmineDisplay.InvalidateVisual();

        }
    }
}
