using System.Globalization;
using Server.Client.Interface;
using Server.Managers.Interface;
using Server.Notifications;

namespace Server.Managers;

public class ClientManager : IManager<IAppClient>, IManagerClients
{
    private readonly Dictionary<ushort, IAppClient> _clients = new();

    public void Add(ushort connectionId)
    {
        Random random = new();
        _clients[connectionId] = new Client.AppClient("Guest" + random.Next(0,1000) + connectionId, "1111", DateTime.Now.Day);
    }

    public void Add(ushort connectionId, IAppClient type)
    {
        
    }

    public void Remove(ushort connectionId)
    {                        
        _clients.Remove(connectionId);
    }

    public void Search(ushort connectionId)
    {
        
    }

    public IAppClient Get(ushort connectionId) => _clients[connectionId];
    public void Set(ushort connectionId, IAppClient type) => _clients[connectionId] = type;
}