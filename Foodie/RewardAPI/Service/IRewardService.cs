using RewardAPI.Message;

namespace RewardAPI.Service
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
