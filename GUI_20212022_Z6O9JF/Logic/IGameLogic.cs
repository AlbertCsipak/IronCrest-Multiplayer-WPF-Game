using System.Collections.ObjectModel;

namespace GUI_20212022_Z6O9JF.Logic
{
    public interface IGameLogic
    {
        bool CanSend { get; set; }
        int ClientId { get; set; }

        void Blue();
        void Connect(string ip = "26.99.118.45");
        void Red();
        ObservableCollection<string> Setup();
        void Skip();
    }
}