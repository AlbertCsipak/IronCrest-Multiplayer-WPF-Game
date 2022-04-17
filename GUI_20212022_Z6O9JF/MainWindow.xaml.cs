using GUI_20212022_Z6O9JF.Logic;
using GUI_20212022_Z6O9JF.ViewModels;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212022_Z6O9JF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IGameLogic gameLogic;
        IClientLogic clientLogic;
        MediaPlayer background_music = new MediaPlayer();
        MediaPlayer background_ambient = new MediaPlayer();
        Cursor c1;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            gameLogic = (this.DataContext as MainViewModel).gameLogic;
            clientLogic = (this.DataContext as MainViewModel).clientLogic;

            c1 = new Cursor("Resources/blurite_sword.cur");
            grid.Cursor = c1;

            background_ambient.Open(new Uri("Resources/Music/ambient.mp3", UriKind.RelativeOrAbsolute));
            background_ambient.Volume = 0.025;
            //background_ambient.Play();
            background_ambient.MediaEnded += Background_ambient_MediaEnded;

            background_music.Open(new Uri("Resources/Music/Lannister_music.mp3", UriKind.RelativeOrAbsolute));
            //background_music.Play();
            background_music.MediaEnded += Background_music_MediaEnded;

        }

        private void Background_ambient_MediaEnded(object sender, EventArgs e)
        {
            background_ambient.Play();
        }

        private void Background_music_MediaEnded(object sender, EventArgs e)
        {
            background_music.Play();
        }

        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string players = JsonConvert.SerializeObject(gameLogic.Players);
            string map = JsonConvert.SerializeObject(gameLogic.Map);

            if (players.Contains("Name"))
            {
                string save = players + "@" + map;
                File.AppendAllText($"Resources/Saves/{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Hour}-{DateTime.Now.Minute}_players-{gameLogic.Players.Count}_map-{gameLogic.Map}.txt", save);
            }

        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                clientLogic.ChangeView("ESC");
            }
        }
    }
}
