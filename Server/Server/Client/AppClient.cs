using Server.Client.Interface;
using Server.Notifications;
using Server.Notifications.Interface;

namespace Server.Client;

public class AppClient(string username, string password, int currentDay) : IAppClient
{
    public ushort ConnectionId { get; set; }
    public bool IsConnected { get; set; }
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;

    public int InputTime { get; set; } = currentDay;
    public ushort NotValuable { get; set; }
    public ushort Valuable { get; set; }
    public ushort Character { get; set; }
    public ushort RoomId { get; set; }
    public ushort EverydayReward { get; set; }

    public ushort AvatarId { get; set; }
    public ushort KillCount { get; set; }
    public ushort KillUnitCount { get; set; }
    public ushort DeadCount { get; set; }
    public ushort HelpCount { get; set; }
    public ushort RencornationCount { get; set; }
    public ushort MatchCountSolo { get; set; }
    public ushort MatchCountDuo { get; set; }
    public ushort MatchCountTriple { get; set; }
    public ushort HighRang { get; set; }
    public ushort CurrentRang { get; set; }
    public ushort HeroesCount { get; set; }
    public List<INotice> Notices { get; set; } = new();
}