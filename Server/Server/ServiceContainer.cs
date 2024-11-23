using Microsoft.Extensions.DependencyInjection;
using Server.Client.Interface;
using Server.Data;
using Server.Data.Interface;
using Server.Logger;
using Server.Logger.Interface;
using Server.Managers;
using Server.Managers.Interface;
using Server.Messenger;
using Server.Network;
using Server.Network.Interface;
using Server.Notifications;
using Server.Physics;
using Server.Reward;
using Server.Room.Example;
using Server.Room.Interface;

namespace Server;

public static class ServiceContainer
{
    private static IServiceCollection? _serviceCollection;
    public static IServiceProvider? Provider;
    
    public static void Initialize()
    {
        _serviceCollection = new ServiceCollection();

        _serviceCollection.AddSingleton<IConfig, NetworkConfig>();
        _serviceCollection.AddSingleton<ILogger, ConsoleLogger>();
        _serviceCollection.AddSingleton<IDatabase, JsonDatabase>();

        _serviceCollection.AddSingleton<IManager<IAppClient>, ClientManager>();
        _serviceCollection.AddSingleton<IManager<IRoom>, RoomManager>();
        _serviceCollection.AddSingleton<IManager<Chat>, ChatManager>();

        _serviceCollection.AddSingleton<NetworkSend>();
        _serviceCollection.AddSingleton<NetworkReceive>();
        _serviceCollection.AddSingleton<RewardSystem>();
        _serviceCollection.AddSingleton<NotificationSystem>();

        _serviceCollection.AddTransient<CollisionMap>();
        _serviceCollection.AddSingleton<UnitSelection>();
        _serviceCollection.AddSingleton<Options>();
        _serviceCollection.AddSingleton<Map>();

        Provider = _serviceCollection.BuildServiceProvider();
    }
}