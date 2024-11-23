using Server.Client.Interface;
using Server.Managers.Interface;
using Server.Room.Interface;

namespace Server.Managers;

public class RoomManager(IManager<IAppClient> clientManager) : IManager<IRoom>
{
    private readonly Dictionary<ushort, IRoom> _rooms = new();
    private readonly IManager<IAppClient> _clientManager = clientManager;

    public void Add(ushort connectionId)
    {
        var roomId = (ushort)((ushort)_rooms.Count + 1);
        IRoom room = new Room.Room(RoomState.Search, 2, roomId);
        
        _rooms.Add(roomId, room);

        Connect(connectionId, roomId);
    }

    public void Add(ushort connectionId, IRoom? type)
    {
        
    }

    public void Remove(ushort roomId)
    {
        _rooms.Remove(roomId);
    }

    public void Search(ushort connectionId)
    {
        var listRooms = _rooms.ToList();

        if (_rooms.Count < 1)
        {
            Add(connectionId);
            return;
        }

        for (ushort i = 1; i <= listRooms.Count; i++)
        {
            if (_rooms.ContainsKey(i))
            {
                if (_rooms[i].State == RoomState.Search)
                {
                    Connect(connectionId, i);
                    break;
                }
            }
        }
    }

    private void Connect(ushort connectionId, ushort roomId)
    {
        var client = _clientManager.Get(connectionId);
            client.RoomId = roomId;
        
        _rooms[roomId].Connect(connectionId, client.Character);
    }
    
    public IRoom Get(ushort connectionId) => _rooms[connectionId];
    public void Set(ushort connectionId, IRoom type) => _rooms[connectionId] = type;
}