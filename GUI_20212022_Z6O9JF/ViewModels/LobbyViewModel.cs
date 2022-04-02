﻿using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace GUI_20212022_Z6O9JF.ViewModels
{
    public class LobbyViewModel : ObservableRecipient
    {
        public IGameLogic gameLogic;
        public IClientLogic clientLogic;
        public ICommand BackCommand { get; set; }
        public ICommand GameCommand { get; set; }
        public ICommand NextFaction { get; set; }
        public ICommand PreviousFaction { get; set; }
        public string SelectedString { get; set; }
        public string Name { get; set; }
        int index = 0;
        public Faction SelectedFaction { get { return gameLogic.AvailableFactions[index]; } }
        public static bool IsInDesignMode
        {
            get
            {
                return
                    (bool)DependencyPropertyDescriptor
                    .FromProperty(DesignerProperties.
                    IsInDesignModeProperty,
                    typeof(FrameworkElement))
                    .Metadata.DefaultValue;
            }
        }
        public LobbyViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IGameLogic>(),Ioc.Default.GetService<IClientLogic>()) { }
        public LobbyViewModel(IGameLogic gameLogic,IClientLogic clientLogic)
        {
            this.gameLogic = gameLogic;
            this.clientLogic = clientLogic;
            gameLogic.GameMap = gameLogic.GameMapSetup($"Resources/Maps/map{gameLogic.Map}.txt");

            Name = "Anon";

            GameCommand = new RelayCommand(() => clientLogic.ChampSelect(SelectedFaction, Name));
            NextFaction = new RelayCommand(() =>
            {
                if (gameLogic.AvailableFactions.Count - 1 > index)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
                OnPropertyChanged("SelectedFaction");
            });
            PreviousFaction = new RelayCommand(() =>
            {
                if (index > 0)
                {
                    index--;
                }
                else
                {
                    index = gameLogic.AvailableFactions.Count - 1;
                }
                OnPropertyChanged("SelectedFaction");
            });

            Messenger.Register<LobbyViewModel, string, string>(this, "Base", (recipient, msg) =>
            {
                OnPropertyChanged("SelectedFaction");
            });
        }
    }
}