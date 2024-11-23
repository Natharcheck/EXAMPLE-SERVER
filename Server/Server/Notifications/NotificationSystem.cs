using Server.Client.Interface;
using Server.Data.Interface;
using Server.Managers.Interface;
using Server.Network;
using Server.Notifications.Interface;

namespace Server.Notifications;

public enum ENoticeFrom : ushort
{
    ChatMessage = 1, 
    ShopEvent,
    BattlePass,
    Achievements,
    Quests,
}

public class NotificationSystem(NetworkSend networkSend, IManager<IAppClient> clientManager, IDatabase database)
{
    private readonly NetworkSend _networkSend = networkSend;
    private readonly IManager<IAppClient> _clientManager = clientManager;
    private readonly IDatabase _database = database;
    
    public void SendNotice(ushort connectionId, INotice notice)
    {
        var client = _clientManager.Get(connectionId);
            client.AddNotice(notice);
        
        _networkSend.Notice(connectionId, notice);
        _database.Save(connectionId);
    }
}