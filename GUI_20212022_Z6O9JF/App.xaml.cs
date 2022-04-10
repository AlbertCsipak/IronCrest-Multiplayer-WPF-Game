using GUI_20212022_Z6O9JF.Logic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace GUI_20212022_Z6O9JF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddSingleton<IGameLogic, GameLogic>()
                .AddSingleton<IClientLogic, ClientLogic>()
                .AddSingleton<IControlLogic, ControlLogic>()
                .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
                .BuildServiceProvider()
            );
        }
    }
}
