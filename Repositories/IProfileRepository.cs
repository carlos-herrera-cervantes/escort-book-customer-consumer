using System.Threading.Tasks;
using EscortBookCustomerConsumer.Models;

namespace EscortBookCustomerConsumer.Repositories;

public interface IProfileRepository
{
    Task CreateAsync(Profile profile);
}
