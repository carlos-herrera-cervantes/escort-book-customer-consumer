using System.Threading.Tasks;
using EscortBookCustomerConsumer.Models;

namespace EscortBookCustomerConsumer.Repositories
{
    public interface IProfileStatusRepository
    {
        Task<ProfileStatus> GetByProfileIdAsync(string profileId);

        Task CreateAsync(ProfileStatus profileStatus);

        Task UpdateAsync(ProfileStatus profileStatus);
    }
}