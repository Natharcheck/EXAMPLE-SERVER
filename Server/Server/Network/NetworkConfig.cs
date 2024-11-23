using Microsoft.Extensions.DependencyInjection;
using Riptide;
using Riptide.Utils;
using Server.Client.Interface;
using Server.Managers.Interface;
using Server.Network.Interface;

namespace Server.Network;

public class NetworkConfig : IConfig
{
    public Riptide.Server Server { get; private set; }
    private Thread? _serverThread;
    private readonly IManager<IAppClient> _clientManager;

    public NetworkConfig(IManager<IAppClient> clientManager)
    { 
        Console.Title = "Server";
        Console.Clear();

        RiptideLogger.Initialize(Console.WriteLine, true);
        Message.MaxPayloadSize = 1024;

        _clientManager = clientManager;

        Server = new Riptide.Server();
        Server.ClientConnected += PlayerConnected;
        Server.ClientDisconnected += PlayerDisconnected;

        Server.Start(7777, 10);

        _serverThread = new Thread(UpdateServer);
        _serverThread.Start();
    }

    private void PlayerConnected(object sender, ServerConnectedEventArgs callBack)
    { 
        _clientManager.Add(callBack.Client.Id);
    }
        
    private void PlayerDisconnected(object sender, ServerDisconnectedEventArgs callBack)
    {
        _clientManager.Remove(callBack.Client.Id);
    }
    
    private void UpdateServer()
    {
        while (true)
        {
            Server.Update();
        }
    }
}