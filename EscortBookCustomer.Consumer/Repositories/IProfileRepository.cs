using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EscortBookCustomer.Consumer.Models;

namespace EscortBookCustomer.Consumer.Repositories;

public interface IProfileRepository
{
    Task<int> CountAsync(Expression<Func<Profile, bool>> expression);

    Task CreateAsync(Profile profile);

    Task DeleteAsync(Expression<Func<Profile, bool>> expression);
}
