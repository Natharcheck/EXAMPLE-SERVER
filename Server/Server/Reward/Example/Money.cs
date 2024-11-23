using Server.Reward.Interface;

namespace Server.Reward.Example;

public class Money(ushort id, ushort value) : IReward
{
    public ushort Id { get; set; } = id;
    public ushort Value { get; set; } = value;
}