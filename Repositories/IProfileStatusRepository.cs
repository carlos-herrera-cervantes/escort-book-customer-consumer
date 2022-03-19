using System.Threading.Tasks;
using EscortBookCustomerConsumer.Models;

namespace EscortBookCustomerConsumer.Repositories
{
    public interface IProfileStatusRepository
    {
         Task CreateAsync(ProfileStatus profileStatus);
    }
}