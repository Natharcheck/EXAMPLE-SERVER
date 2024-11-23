using Microsoft.Extensions.DependencyInjection;
using Server.Network;
using Server.Reward.Interface;
using Server.Reward.Items;

namespace Server.Reward;

public class RewardSystem(NetworkSend networkSend)
{
    private readonly NetworkSend _networkSend = networkSend;
    public readonly RewardsContainer RewardContainer = ServiceContainer.Provider!.GetRequiredService<RewardsContainer>();
    
    public void Give(ushort connectionId, List<IReward> rewards)
    {
        _networkSend.Reward(connectionId, rewards);
    }
}