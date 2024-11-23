using Server.Client.Interface;

namespace Server.Managers.Interface;

public interface IManager<T>
{
    public void Add(ushort index);
    public void Add(ushort index, T type);
    public void Remove(ushort index);

    public void Search(ushort index);
    
    public T Get(ushort index);
    public void Set(ushort index, T type);
}