using Server.Reward.Items;

namespace Server.Reward.Interface;

public interface IReward
{
    public ushort Id { get; set; }
    public ushort Value { get; set; }
}