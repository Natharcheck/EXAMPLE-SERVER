using Microsoft.Extensions.DependencyInjection;
using Riptide;
using Server.Network;

namespace Server.Messenger;

public abstract class Chat(ushort idChat)
{
    public readonly ushort Index = idChat;
    
    private List<Client.AppClient> _clients = new();
    private Stack<string> _messages = new();

    private readonly NetworkSend _networkSend = ServiceContainer.Provider.GetRequiredService<NetworkSend>();

    public virtual void SendTo(ushort connectionId, string message)
    {
        _networkSend.MessageChat(connectionId, Index, message);
    }
    
    public virtual void SendAll(string message)
    {
        foreach (var client in _clients)
        {
            _networkSend.MessageChat(client.ConnectionId, Index, message);
        }
    }

    public List<string> GetHistory() => _messages.ToList();
}