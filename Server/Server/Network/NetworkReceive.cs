using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using Riptide;
using Server.Client.Interface;
using Server.Data.Interface;
using Server.Logger.Interface;
using Server.Managers.Interface;
using Server.Messenger;
using Server.Network.Interface;
using Server.Physics;
using Server.Room.Interface;
using Server.Room;

namespace Server.Network;

public enum ClientPackets : ushort
{
    Authorization = 1,
    Registration,
		
    ConnectedOrCreatedRoom,
    MapLoadingSuccessful,
    
    ActorSelection,
    BehaviorPlayer,
    AbilityPlayer,
    MovementPayer,
    
    ItemByShop,
    InventoryShow,
    MessageChat,
    PositionObjects
}

public class NetworkReceive : IReceive
{
    private static readonly ILogger Logger = ServiceContainer.Provider.GetRequiredService<ILogger>();
    private static readonly IDatabase Database = ServiceContainer.Provider.GetRequiredService<IDatabase>();
    private static readonly IManager<IRoom> RoomManager = ServiceContainer.Provider.GetRequiredService<IManager<IRoom>>();
    private static readonly IManager<IAppClient> ClientManager = ServiceContainer.Provider.GetRequiredService<IManager<IAppClient>>();
    private static readonly IManager<Chat> ChatManager = ServiceContainer.Provider.GetRequiredService<IManager<Chat>>();
    private static readonly NetworkSend NetworkSend = ServiceContainer.Provider.GetRequiredService<NetworkSend>();

    [MessageHandler((ushort)ClientPackets.Authorization)]
    private static void Authorization(ushort connectionId, Message message)
    {
        var username = message.GetString();
        var password = message.GetString();

        if (Database.IsExist(Database.PathAccount, username))
        {
            if (Database.IsCorrect(username, password))
            {
                Database.Load(connectionId, username);

                NetworkSend.Verification(connectionId);
                NetworkSend.LoadClientData(connectionId);
            }
        }
    }

    [MessageHandler((ushort)ClientPackets.Registration)]
    private static void Registration(ushort connectionId, Message message)
    {
        var username = message.GetString();
        var password = message.GetString();

        if (Database.IsExist(Database.PathAccount ,username) == false)
        {
            Database.Create(connectionId, username, password);
            NetworkSend.Verification(connectionId);

            Logger.Print($"Client [{username}] has been Registration.");
        }
    }
    
    [MessageHandler((ushort)ClientPackets.ActorSelection)]
    private static void ActorSelection(ushort connectionId, Message message)
    {
        var character = message.GetUShort();
        ClientManager.Get(connectionId).Character = character;
        
        NetworkSend.LoadClientData(connectionId);
    }

    [MessageHandler((ushort)ClientPackets.ConnectedOrCreatedRoom)]
    private static void ConnectedOrCreatedRoom(ushort connectionId, Message message)
    {
        RoomManager.Search(connectionId);
    }

    [MessageHandler((ushort)ClientPackets.MapLoadingSuccessful)]
    private static void MapLoadingSuccessful(ushort connectionId, Message message)
    {
        var roomId= ClientManager.Get(connectionId).RoomId;

        if (roomId != 0)
        {
            var room = RoomManager.Get(roomId);
                room.GetPlayer(connectionId).IsReady = true;

            room.CheckMapLoading();
        }
    }
    
    [MessageHandler((ushort)ClientPackets.BehaviorPlayer)]
    private static void BehaviorPlayer(ushort connectionId, Message message)
    {
        var room = RoomManager.Get(ClientManager.Get(connectionId).RoomId);
        var behaviorId = message.GetUShort();
        
        NetworkSend.BehaviorPlayer(connectionId, behaviorId, room.GetConnectionsId());
    }
    
    [MessageHandler((ushort)ClientPackets.AbilityPlayer)]
    private static void AbilityPlayer(ushort connectionId, Message message)
    {
        var service = (IAbilityService)RoomManager.Get(ClientManager.Get(connectionId).RoomId);
        var abilityIndex = message.GetByte();
        var abilityState = message.GetByte();
        var angle = message.GetUShort();
        
        var targetPosition = new Vector2(message.GetFloat(), message.GetFloat());
        
        service.UseAbility(connectionId, abilityIndex, abilityState, angle, targetPosition);
    }

    [MessageHandler((ushort)ClientPackets.MovementPayer)]
    private static void MovementPlayer(ushort connectionId, Message message)
    {
        var service = (IMovementService)RoomManager.Get(ClientManager.Get(connectionId).RoomId);
        var directionAngle = message.GetUShort();
        
        service.Movement(connectionId, directionAngle);
    }
    
    [MessageHandler((ushort)ClientPackets.MessageChat)]
    private static void MessageChat(ushort connectionId, Message message)
    {
        var chatIndex = message.GetUShort();
        var data = message.GetString();

        var chat = ChatManager.Get(chatIndex);
        
        chat?.SendAll(data);
    }
    
    [MessageHandler((ushort)ClientPackets.PositionObjects)]
    private static void PositionObjects(ushort connectionId, Message message)
    {
        var positions = new List<Vector2>();
        
        var blockId = message.GetUShort();
        var blockCount = message.GetUShort();

        for (ushort i = 0; i < blockCount; i++)
        {
            positions.Add(message.GetVector2());
        }
    }
}