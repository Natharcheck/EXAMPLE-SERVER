using Server.Managers.Interface;
using Server.Messenger;
using Server.Messenger.Example;

namespace Server.Managers;

public class ChatManager : IManager<Chat>
{
    private Dictionary<ushort, Chat> _chats = new();

    public void Add(ushort index = 0)
    {
        index = (ushort)((ushort)_chats.Count + 1);
        
        var chat = new LobbyChat(index);
        _chats.Add(index, chat);
    }

    public void Add(ushort index, Chat chat)
    {
        _chats.Add(index, chat);
    }

    public void Remove(ushort index)
    {
        _chats.Remove(index);
    }

    public void Search(ushort index)
    {
        
    }

    public Chat Get(ushort index) => _chats[index];

    public void Set(ushort index, Chat chat)
    {
        _chats[index] = chat;
    }
}