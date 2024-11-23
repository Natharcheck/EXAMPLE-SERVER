using System.Numerics;
using Riptide;
using Server.Client.Interface;
using Server.Logger.Interface;
using Server.Managers.Interface;
using Server.Network.Interface;
using Server.Notifications.Interface;
using Server.Reward.Interface;
using Server.Room;

namespace Server.Network;

public enum ServerPackets : ushort
{
    Verification = 1,
    LoadDataClient,

    LoadScene,
    LoadMatch,
    StartMatch,

    CreatePlayer,
    AbilityPlayer,
    BehaviorPlayer,
    MovementPayer,
    WinPlayer,

    CreateEnemies,
    AbilityEnemy,
    BehaviorEnemy,
    MovementEnemy,

    UnitHealth,
    AbilityCooldown,
    
    ItemByShopValid,
    InventoryShow,
    DropBoosters,
    GetBoosters,
    PoisonCloud,
    MessageChat,
    Reward,
    Notice
}
public class NetworkSend(IManager<IAppClient> clientManager, IConfig config, ILogger logger) : ISend
{
    private readonly ILogger _logger = logger;
    private readonly IConfig _config = config;
    private readonly IManager<IAppClient> _clientManager = clientManager;

    public void Verification(ushort connectionId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.Verification);

        _config.Server.Send(message, connectionId);
    }

    public void LoadClientData(ushort connectionId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.LoadDataClient);
        var client = _clientManager.Get(connectionId);

        message.AddString(client.Username);
        message.AddUShort(client.Valuable);
        message.AddUShort(client.NotValuable); 
        message.AddUShort(client.Character); 

        _config.Server.Send(message, connectionId);
    }

    public void LoadScene(ushort connectionId, string sceneName)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.LoadScene);
            message.AddString(sceneName);

        _config.Server.Send(message, connectionId);
    }
    
    public void LoadMatch(ushort connectionId, string sceneName, List<ushort> connectionsId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.LoadMatch);
        message.AddString(sceneName);
        message.AddUShort((ushort)connectionsId.Count);

        foreach (var connection in connectionsId)
        {
            message.AddUShort(_clientManager.Get(connection).AvatarId);
        }

        _config.Server.Send(message, connectionId);
    }

    public void StartMatch(List<ushort> connectionsId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.StartMatch);

        foreach (var connectionId in connectionsId)
        {
            _config.Server.Send(message, connectionId);
        }
    }

    public void CreatePlayer(ushort connectionId, List<Player> players)
    {
        foreach (var player in players)
        {
            _config.Server.Send(PlayerData(connectionId, player.Character), player.ConnectionId);
        }
    }

    private Message PlayerData(ushort connectionId, ushort character)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.CreatePlayer);
        message.AddUShort(connectionId);
        message.AddUShort(character);

        return message;
    }

    public void BehaviorPlayer(ushort connectionId, ushort behaviorId, List<ushort> connectionsId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.BehaviorPlayer);

        message.AddUShort(connectionId);
        message.AddUShort(behaviorId);

        foreach (var id in connectionsId)
        {
            _config.Server.Send(message, id);
        }
    }

    public void MovementPlayer(ushort connectionId, Vector2 position, ushort angle, List<ushort> connectionsId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.MovementPayer);

        message.AddUShort(connectionId);
        message.AddUShort(angle);

        message.AddFloat(position.X);
        message.AddFloat(position.Y);

        foreach (var id in connectionsId)
        {
            _config.Server.Send(message, id);
        }
    }

    public void UnitHealth(List<ushort> connectionsId, List<short> healths, List<short> maxHealths, ushort unitType)
    {
        var message = Message.Create(MessageSendMode.Unreliable, ServerPackets.UnitHealth);
        message.AddUShort(unitType);
        message.AddUShort((ushort)connectionsId.Count);

        for (var i = 0; i < connectionsId.Count; i++)
        {
            message.AddUShort(connectionsId[i]);
            message.AddShort(healths[i]);
            message.AddShort(maxHealths[i]);
        }

        foreach (var connectionId in connectionsId)
        {
            _config.Server.Send(message, connectionId);
        }
    }

    public void CreateEnemies(List<ushort> connectionsId, List<Enemy> enemies)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.CreateEnemies);
            message.AddUShort((ushort)enemies.Count);

        foreach (var enemy in enemies)
        {
            message.AddUShort(enemy.Index);
            message.AddUShort(enemy.Character);
        }

        foreach (var connectionId in connectionsId)
        {
            _config.Server.Send(message, connectionId);   
        }
    }

    public void BehaviorEnemy(ushort behaviorId, ushort connectionId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.BehaviorEnemy);
        message.AddUShort(behaviorId);

        _config.Server.Send(message, connectionId);
    }

    public void MovementEnemy(ushort enemyId, Vector2 position, ushort angle, List<ushort> connectionsId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.MovementEnemy);
        message.AddUShort(enemyId);
        message.AddUShort(angle);

        message.AddFloat(position.X);
        message.AddFloat(position.Y);

        foreach (var id in connectionsId)
        {
            _config.Server.Send(message, id);
        }
    }

    public void AbilityCooldown(ushort connectionId, ushort cooldown)
    {
        var message = Message.Create(MessageSendMode.Unreliable, ServerPackets.AbilityCooldown);
        message.AddUShort(connectionId);
        message.AddUShort(cooldown);

        _config.Server.Send(message, connectionId);
    }
    
    public void WinPlayer(List<ushort> connectionsId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.WinPlayer);
        message.AddUShort((ushort)connectionsId.Count);

        foreach (var connectionId in connectionsId)
        {
            message.AddUShort(connectionId);
        }
        //Сундук
        //Рейтинг

        foreach (var connectionId in connectionsId)
        {
            _config.Server.Send(message, connectionId);   
        }
    }
    
    public void DropBoosters(List<Vector2> positions, List<ushort> connectionsId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.DropBoosters);

        message.AddUShort((ushort)positions.Count);

        foreach (var position in positions)
        {
            message.AddFloat(position.X);
            message.AddFloat(position.Y);   
        }

        foreach (var id in connectionsId)
        {
            _config.Server.Send(message, id);
        }
        _logger.Print("Boosters Send");
    }
    
    public void GetBoosters(ushort connectionId, ushort boostersCount, Vector2 position, List<ushort> connectionsId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.GetBoosters);

        message.AddUShort(connectionId);
        message.AddUShort(boostersCount);
        
        message.AddFloat(position.X);
        message.AddFloat(position.Y);

        foreach (var id in connectionsId)
        {
            _config.Server.Send(message, id);
        }
    }
    
    public void PoisonCloud(float radius, List<ushort> connectionsId)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.PoisonCloud);

        message.AddFloat(radius * 2);

        foreach (var id in connectionsId)
        {
            _config.Server.Send(message, id);
        }
    }
    
    public void MessageChat(ushort connectionId, ushort chatIndex, string chatMessage)
    {
        var message = Message.Create(MessageSendMode.Unreliable, ServerPackets.MessageChat);
        
        message.AddUShort(connectionId);
        message.AddUShort(chatIndex);
        
        message.AddString(chatMessage);

        _config.Server.Send(message, connectionId);
    }
    
    public void Notice(ushort connectionId, INotice notice)
    {
        var message = Message.Create(MessageSendMode.Unreliable, ServerPackets.MessageChat);
        
        message.AddUShort(notice.CountNotice);
        message.AddUShort((ushort)notice.NoticeFrom);

        _config.Server.Send(message, connectionId);
    }
    
    public void Reward(ushort connectionId, List<IReward> rewards)
    {
        var message = Message.Create(MessageSendMode.Reliable, ServerPackets.Reward);

        message.AddUShort((ushort)rewards.Count);

        foreach (var reward in rewards)
        {
            message.AddUShort(reward.Id);
        }

        _config.Server.Send(message, connectionId);
    }
}