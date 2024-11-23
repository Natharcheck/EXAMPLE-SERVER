using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Server.Client.Interface;
using Server.Data.Interface;
using Server.Logger.Interface;
using Server.Managers.Interface;
using Server.Physics;

namespace Server.Data;

public class JsonDatabase : IDatabase
{
    public string PathData { get; set; }
    public string PathAccount { get; set; }
    public string PathCollisions { get; set; }
    public string FileExtinction { get; set; }

    private readonly ILogger _logger;
    private readonly IManager<IAppClient> _clientManager;

    public JsonDatabase(ILogger logger, IManager<IAppClient> clientManager)
    {
        _logger = logger;
        _clientManager = clientManager;
        
        PathData = "/Users/tt/Documents/Rider/Server/Server/Data";
        PathAccount = "/Accounts";
        PathCollisions = "/Collisions";
        FileExtinction = ".json";
        
        Initialize();
    }

    private void Initialize()
    {
        _logger.Print("Path:..." + PathAccount);

        if (!Directory.Exists(PathData + PathAccount))
        {
            Directory.CreateDirectory(PathData + PathAccount);
            _logger.Print($"Create directory '{PathAccount}'");
        }
    }
    
    public void Create(ushort connectionId, string username, string password)
    {
        IAppClient appClientData = new Client.AppClient(username, password, DateTime.Now.Day)
        {
            ConnectionId = connectionId,
            
            Valuable = 10,
            NotValuable = 1000,
            
            CurrentRang = 0,
            HighRang = 0,
            Character = 0,
            RoomId = 0,
        };

        _clientManager.Set(connectionId, appClientData);
        
        Save(connectionId);
    }

    public void Search(ushort connectionId)
    {
        
    }

    public void Save(ushort connectionId)
    {
        var client = _clientManager.Get(connectionId);
        var path = PathData + PathAccount + "/" + client.Username + FileExtinction;
        
			
        File.WriteAllText(path,  JsonConvert.SerializeObject(client, Formatting.Indented));
    }

    public void Load(ushort connectionId, string username)
    {
        var path = PathData + PathAccount + "/" + username + FileExtinction;
        var newPath = File.ReadAllText(path);
			
        if(IsExist(PathAccount, username))
        {
            var client = JsonConvert.DeserializeObject<Client.AppClient>(newPath);
            _clientManager.Set(connectionId, client); 
        }

        _clientManager.Get(connectionId).ConnectionId = connectionId;
    }

    public bool IsExist(string folder, string name)
    {
        return File.Exists(PathData + folder + "/" + name + FileExtinction);
    }

    public bool IsCorrect(string username, string password)
    {
        var path = PathData + PathAccount + "/" + username + FileExtinction;
			
        var newPath = File.ReadAllText(path);
        var client = JsonConvert.DeserializeObject<Client.AppClient>(newPath);

        if (client.Password == password) return true;

        return false;
    }

    public void SaveCollisionMap(Map map)
    {
        var path = PathData + PathCollisions + "/" + map.Name + FileExtinction;
			
        File.WriteAllText(path,  JsonConvert.SerializeObject(map, Formatting.Indented));
        
        _logger.Print("Collision Map Ready");
    }
    
    public void LoadCollisionMap(string name)
    {
        var path = PathData + PathCollisions + "/" + name + FileExtinction;
        var newPath = File.ReadAllText(path);
			
        if(IsExist(PathCollisions, name))
        {
            var globalMap = ServiceContainer.Provider.GetRequiredService<Map>();
            
            var map = JsonConvert.DeserializeObject<Map>(newPath);

            globalMap.Name = map!.Name;
            globalMap.BoxPositions = map.BoxPositions;
            globalMap.RockPositions = map.RockPositions;
            globalMap.SeaweedPositions = map.SeaweedPositions;
        }
    }
}