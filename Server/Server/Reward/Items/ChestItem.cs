using Server.Reward.Interface;

namespace Server.Reward.Items;

public class ChestItem(ushort id, bool isRandomRewards, List<IReward> rewards) : Item(id)
{
    public List<IReward> Rewards = rewards;
}