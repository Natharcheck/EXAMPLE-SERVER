namespace Server.Reward.Items;

public abstract class Item(ushort id)
{
    public readonly ushort Id = id;
    public ushort Value;
}