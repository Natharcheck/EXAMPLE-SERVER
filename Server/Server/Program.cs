using Microsoft.Extensions.DependencyInjection;
using Server.Data.Interface;
using Server.Managers.Interface;
using Server.Messenger;
using Server.Network.Interface;

namespace Server;

public static class Program
{
    public static void Main()
    {
        ServiceContainer.Initialize();
        
        ServiceContainer.Provider.GetRequiredService<IDatabase>();
        ServiceContainer.Provider.GetRequiredService<IConfig>();
        ServiceContainer.Provider.GetRequiredService<IManager<Chat>>().Add(0);
        
        ServiceContainer.Provider.GetRequiredService<IDatabase>().LoadCollisionMap("CollisionMap");
    }
}