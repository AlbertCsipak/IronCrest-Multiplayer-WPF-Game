using System.Collections.ObjectModel;

namespace GUI_20212022_Z6O9JF.Logic
{
    public interface IGameLogic
    {
        void ClientSetup();
        void Red();
        ObservableCollection<string> Setup();
        void Skip();
    }
}