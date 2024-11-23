namespace Server.Room.Interface;

public enum RoomState
{
    Closed,
    Search
}

public interface IRoom
{
    public ushort RoomId { get; set; }
    public byte MaxPlayers { get; set; }
    public RoomState State { get; set; }
    public byte CurrentPlayers { get; set; }
    public void Start();
    public void Update();
    public void End();
    public void Connect(ushort connectionId, ushort character);
    public void ReConnect(ushort connectionId);
    public void Leave(ushort connectionId);
    public void LeaveAll();
    public void CheckMapLoading();
    public void CheckWinConditions(ushort connectionId);
    public Player GetPlayer(ushort index);
    public void RemovePlayer(ushort index);
    public List<ushort> GetConnectionsId();
}