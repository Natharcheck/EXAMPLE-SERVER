using Server.Client.Interface;
using Server.Managers.Interface;
using Server.Reward.Interface;

namespace Server.Reward;

public class EverydayReward(IManager<IAppClient> clientManager, RewardSystem rewardSystem)
{
    private readonly IManager<IAppClient> _clientManager = clientManager;
    private readonly RewardSystem _rewardSystem = rewardSystem;
    
    public void CheckReward(IAppClient client)
    {
        var connectionId = client.ConnectionId;
        var rewards = new List<IReward>();

        if (client.InputTime == DateTime.Now.Day)
        {
            switch (client.EverydayReward)
            {
                case 0 :
                    client.EverydayReward += 1;
                    CheckReward(client);
                    break;
                case 1 : 
                    rewards.Add(_rewardSystem.RewardContainer.Money);
                    _rewardSystem.Give(connectionId, rewards);
                
                    client.EverydayReward += 1;
                    client.InputTime = DateTime.Now.Day;
                    break;
                case 2 : break;
                case 3 : break;
                case 4 : break;
                case 5 : break;
                case 6 : break;
                case 7 : break;
            }   
        }
    }
}