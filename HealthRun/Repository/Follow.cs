using HealthRun.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthRun.Repository
{
    public interface Follow
    {
        Task<int> GetFollowersListAsync();
        Task<int> FollowAsync();
        Task<int> UnFollowAsync();

    }
}
