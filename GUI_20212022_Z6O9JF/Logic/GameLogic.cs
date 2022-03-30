using GUI_20212022_Z6O9JF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GUI_20212022_Z6O9JF.Logic
{
    public class GameLogic : IGameLogic
    {
        bool CanSend { get; set; }
        ObservableCollection<Player> Players;
        SocketClient.SocketClient socketClient;
        public FieldType[,] GameMap { get; set; }
        public enum FieldType { grass, water, village, desert, snow }
        public GameLogic()
        {
            socketClient = new SocketClient.SocketClient();
            this.Players = new ObservableCollection<Player>();
            GameMap = GameMapSetup("C:/Users/berci/source/repos/GUI_20212022_Z6O9JF/GUI_20212022_Z6O9JF/Maps/map1.txt");
        }
        public ObservableCollection<Player> Setup() { return Players; }
        public FieldType[,] GameMapSetup(string path)
        {
            string[] lines = File.ReadAllLines(path);
            ;
            FieldType[,] map = new FieldType[int.Parse(lines[0]), int.Parse(lines[1])];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (lines[i + 2][j])
                    {
                        case 'g':
                            map[i, j] = FieldType.grass;
                            break;
                        case 'w':
                            map[i, j] = FieldType.water;
                            break;
                        case 'v':
                            map[i, j] = FieldType.village;
                            break;
                        case 'd':
                            map[i, j] = FieldType.desert;
                            break;
                        case 's':
                            map[i, j] = FieldType.snow;
                            break;
                        default:
                            break;
                    }
                }
            }

            return map;
        }
        public void ClientSetup()
        {
            socketClient.Connect();
            Task Send = new Task(() =>
            {
                while (socketClient.MySocket.Connected)
                {
                    if (CanSend)
                    {
                        socketClient.DataSend(Players);
                    }
                }
            }, TaskCreationOptions.LongRunning);

            Task Receive = new Task(() =>
            {
                while (socketClient.MySocket.Connected)
                {
                    string message = socketClient.DataReceive();
                    if (message != null)
                    {
                        if (message.Equals("false") || message.Equals("true"))
                        {
                            CanSend = bool.Parse(message);
                        }
                        else
                        {
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => Players.Clear()));
                            foreach (var item in JsonConvert.DeserializeObject<ObservableCollection<Player>>(message))
                            {
                                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => Players.Add(item)));
                                //cross thread exception miatt meg kell hivni az ui main threadjet
                            }
                        }
                    }
                }
            }, TaskCreationOptions.LongRunning);

            Send.Start();
            Receive.Start();
        }
        public void Skip() { socketClient.Skip(); }
    }
}
