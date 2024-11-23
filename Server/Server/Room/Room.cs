using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using Server.Logger.Interface;
using Server.Network;
using Server.Physics;
using Server.Room.AbilitySystem.Example.Damage;
using Server.Room.Example;
using Server.Room.Interface;

namespace Server.Room;

public class Room(RoomState state, byte maxPlayers, ushort roomId) : IRoom, IAbilityService, IMovementService
{
    public ushort RoomId { get; set; } = roomId;
    public byte MaxPlayers { get; set; } = maxPlayers;
    public RoomState State { get; set; } = state;
    public byte CurrentPlayers { get; set; }

    private AbilityConfig _abilityConfig = new();
    private AbilityConfig[] _abilityConfigs;
    private AbilityStorage _abilityStorage;
    private AbilityCastHandler _abilityCastHandler;
    
    private readonly List<ushort> _connectionsId = new();
    private readonly List<Player> _players = new();
    private readonly List<ushort> _winPlayers = new();
    
    private List<Enemy> _enemies = new();
    private List<Unit> _units = new();
    private List<Spot> _spots = new();

    private Dictionary<string, Player> _teams = new();
    private Random _random = new();

    private readonly ILogger _logger = ServiceContainer.Provider.GetRequiredService<ILogger>();
    private readonly NetworkSend _networkSend = ServiceContainer.Provider.GetRequiredService<NetworkSend>();
    private readonly CollisionMap _collisionMap = ServiceContainer.Provider.GetRequiredService<CollisionMap>();
    private readonly Options _options = ServiceContainer.Provider.GetRequiredService<Options>();

    public void Start()
    {
        _logger.Print($"Room[{RoomId}] has been started");
        
        //Create Enemies
        //Add all units
        
        _units.AddRange(_players);
        _units.AddRange(_collisionMap.GetUnits());
        
        foreach (var player in _players)
        {
            _networkSend.CreatePlayer(player.ConnectionId, _players);

            //player.UnitKnockdown += UnitKnockdown;

            player.UnitDead += UnitDead;
            player.UnitDead += DropBoosters;
            player.UnitDead += CheckWinConditions;

            player.AbilityCastHandler.SetUnits(_units);
        }

        foreach (var player in _players)
        {
            var value = _random.Next(-10, 10);
            player.Transform.Position = new Vector2(40 + value, 40 + value);
            
            _networkSend.MovementPlayer(player.ConnectionId, player.Transform.Position, (ushort)player.Transform.Rotation.Y, _connectionsId);
        }

        var patrick = new Patrick();
        var position = new Vector2(45, 45);

        patrick.Index = (ushort)(_players[0].ConnectionId + _players.Count + 1);
        patrick.Transform.Position = position;

        _enemies.Add(patrick);

        foreach (var enemy in _enemies)
        {
            enemy.AbilityCastHandler.SetUnits(_players);
        }
        
        _spots.Add(new Spot(patrick, position, 5));
        _networkSend.CreateEnemies(_connectionsId, _enemies);

        foreach (var enemy in _enemies)
        {
            _networkSend.MovementEnemy(enemy.Index, enemy.Transform.Position, (ushort)enemy.Transform.Rotation.Y, _connectionsId);
        }

        foreach (var spot in _spots)
        {
            spot.SetUnits(_players);
        }
        
        _units.AddRange(_enemies);
        
        _abilityConfigs = new AbilityConfig[1];
        
        _abilityConfigs[0] = new PoisonCloudConfig();
        _abilityStorage = new AbilityStorage(_abilityConfigs);
        
        _abilityCastHandler = new AbilityCastHandler(null!, _abilityStorage);
        _abilityCastHandler.SetUnits(_units);
        _abilityCastHandler.Initialize();

        _networkSend.StartMatch(_connectionsId);
        
        TeamDistribution(_players.Count);
        Update();
    }

    public async void Update()
    {
        while (true)
        {
            SendUnitHealth((ushort)ETypeUnit.Player);
            SendUnitHealth((ushort)ETypeUnit.Enemy);
            
            CastPoisonCloud();
            CheckTargetSpots();
            
            await Task.Delay(_options.UpdateTime / 2);
        }
    }

    public void End()
    {
        
    }

    public void Connect(ushort connectionId, ushort character)
    {
        var unitSelection = ServiceContainer.Provider.GetRequiredService<UnitSelection>();
        var unit = unitSelection.SelectPlayer(character);
            unit.ConnectionId = connectionId;
            unit.Character = character;
            unit.Index = (byte)connectionId;

        _players.Add(unit);
        _connectionsId.Add(connectionId);
        
        CheckStartConditions();
    }

    public void ReConnect(ushort connectionId)
    {
        
    }

    public void Leave(ushort connectionId)
    {
        
    }

    public void LeaveAll()
    {
        foreach (var player in _players.ToList())
        {
            RemovePlayer(player.ConnectionId);
        }
    }

    public void CheckMapLoading()
    {
        foreach (var player in _players)
        {
            if (player.IsReady == false) return;
        }
        
        Start();
    }

    private void CheckStartConditions()
    {
        if (_players.Count == MaxPlayers)
        {
            CurrentPlayers = MaxPlayers;
            
            foreach (var player in _players)
            {
                _networkSend.LoadMatch(player.ConnectionId, "Room", _connectionsId);
            }
        }
    }

    public virtual void CheckWinConditions(ushort connectionId)
    {
        if (CurrentPlayers == 1)
        {
            _logger.Print($"Room [{RoomId}] WIN");
            _winPlayers.Add(connectionId);
            _networkSend.WinPlayer(_winPlayers);
            
            End();
        }
    }

    public Player GetPlayer(ushort connectionId)
    {
        foreach (var unit in _players)
        {
            if (unit.ConnectionId == connectionId)
                return unit;
        }

        return null!;
    }
    
    public Unit GetEnemy(ushort connectionId)
    {
        foreach (var unit in _enemies)
        {
            if (unit.Index == connectionId)
                return unit;
        }

        return null!;
    }
    
    public Unit GetUnit(ushort connectionId)
    {
        foreach (var unit in _units)
        {
            if (unit.Index == connectionId)
                return unit;
        }

        return null!;
    }

    public void RemovePlayer(ushort connectionId)
    {
        _players.Remove(GetPlayer(connectionId));
        _connectionsId.Remove(connectionId);
        CurrentPlayers--;
    }

    private void UnitKnockdown(ushort connectionId)
    {
        _networkSend.BehaviorPlayer(connectionId, (ushort)EUBehavior.Dead, _connectionsId);
    }
    
    private void UnitDead(ushort connectionId)
    {
        _logger.Print($"Unit [{connectionId}] die");
        _networkSend.BehaviorPlayer(connectionId, (ushort)EUBehavior.Knockdown, _connectionsId);
    }

    public List<ushort> GetConnectionsId() => _connectionsId;

    private async void DropBoosters(ushort connectionId)
    {
        var unit = GetUnit(connectionId);
        var startPosition = unit.Transform.Position;
        var boosters = unit.GetBoosters();
        var positions = new List<Vector2>();
        
        foreach (var booster in boosters)
        {
            var randPosX = _random.Next(-5, 5);
            var randPosY = _random.Next(-5, 5);
            var position = new Vector2(startPosition.X + randPosX, startPosition.Y + randPosY);
            
            booster.Transform.Position = position;
            positions.Add(position);
            
            _collisionMap.AddBooster(booster);
        }
        
        _networkSend.DropBoosters(positions, _connectionsId);
    }
    
    private void SendUnitHealth(ushort unitType)
    {
        var healths = new List<short>();
        var maxHealths = new List<short>();

        foreach (var unit in _players)
        {
            healths.Add(unit.HealthComponent!.Health);
            maxHealths.Add(unit.HealthComponent.MaxHealth);
        }

        _networkSend.UnitHealth(_connectionsId, healths, maxHealths, unitType);
    }

    public void UseAbility(ushort connectionId, byte abilityIndex, byte abilityState, ushort angle, Vector2 targetPosition)
    {
        var unit = GetPlayer(connectionId);
            unit.UseAbility(abilityIndex, abilityState);
            unit.AngleAttack = angle;
        
        _networkSend.MovementPlayer(connectionId, unit.Transform.Position, (ushort)unit.Transform.Rotation.Y, _connectionsId);
    }

    public void Movement(ushort connectionId, ushort directionAngle)
    {
        var unit = GetPlayer(connectionId);
            
        if(unit.HealthComponent?.IsLive == false) return;
        
        unit.Transform.Rotation.Y = directionAngle;
            
        var checkDirection = unit.Transform.Position + unit.Transform.Forward * (unit.MoveSpeed / 2);
        var direction = unit.Transform.Position + unit.Transform.Forward * unit.MoveSpeed;
            
        unit.Transform.Direction = checkDirection;

        if (_collisionMap.GetCirclesCollisionDirection(unit) == false)
        {
            unit.Transform.Position = direction;
        }

        if (_collisionMap.GetCirclesCollisionBoosters(unit, out var booster))
        {
            _collisionMap.RemoveBooster(booster);
            _networkSend.GetBoosters(unit.ConnectionId, (ushort)unit.GetBoosters().Count, booster.Transform.Position, _connectionsId);
            unit.AddBooster(booster);
        }

        _networkSend.MovementPlayer(connectionId, unit.Transform.Position, (ushort)unit.Transform.Rotation.Y, _connectionsId);
    }

    private void MovementEnemies(ushort enemyId, Vector2 targetPosition)
    {
        var unit = GetEnemy(enemyId);
        
        if(unit.IsMove == false) return;
        if(unit.HealthComponent?.IsLive == false) return;
        
        unit.Transform.LookAt(targetPosition);
        var direction = unit.Transform.Position + unit.Transform.Direction * unit.MoveSpeed;
        
        if (_collisionMap.GetCirclesCollisionDirection(unit) == false)
        {
            unit.Transform.Position = direction;
        }
        
        _networkSend.MovementEnemy(unit.Index, unit.Transform.Position, (ushort)unit.Transform.Rotation.Y, _connectionsId);
    }

    private void TeamDistribution(int teamCount)
    {
        var teams = new List<List<Player>>();
        
        for (byte i = 0; i < teamCount; i++)
        {
            teams.Add(new List<Player>());
        }
        
        for (byte i = 0; i < _players.Count; i++)
        {
            var teamIndex = i % teamCount;
            var teamName = (teamIndex + 1).ToString();
            
            _players[i].Team = teamName;  
            teams[teamIndex].Add(_players[i]);
        }
    }

    private void CastPoisonCloud()
    {
        _abilityCastHandler.OnCast(0);
        var ability = _abilityCastHandler.GetAbility<PoisonCloudAbility>();
        
        _networkSend.PoisonCloud(ability.Collision.Radius, _connectionsId);
    }
    
    private void CheckTargetSpots()
    {
        foreach (var spot in _spots)
        {
            spot.SetTarget();

            var unit = spot.Owner;
            MovementEnemies(unit.Index, unit.GetTargetPosition());
        }
    }
}