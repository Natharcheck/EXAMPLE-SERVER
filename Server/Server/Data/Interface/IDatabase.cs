using Server.Physics;

namespace Server.Data.Interface;

public interface IDatabase
{
    public string PathData { get; set; }
    public string PathAccount { get; set; }
    public string PathCollisions { get; set; }
    public string FileExtinction { get; set; }
    
    public void Create(ushort connectionId, string username, string password);
    public void Search(ushort connectionId);
    
    public void Save(ushort connectionId);
    public void Load(ushort connectionId, string username);
    
    public bool IsExist(string folder, string username);
    public bool IsCorrect(string username, string password);

    public void SaveCollisionMap(Map map);
    public void LoadCollisionMap(string name);
}