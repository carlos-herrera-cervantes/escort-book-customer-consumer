using System.Threading.Tasks;
using EscortBookCustomerConsumer.Models;

namespace EscortBookCustomerConsumer.Repositories;

public interface IProfileStatusCategoryRepository
{
    Task<ProfileStatusCategory> GetByName(string name);
}
