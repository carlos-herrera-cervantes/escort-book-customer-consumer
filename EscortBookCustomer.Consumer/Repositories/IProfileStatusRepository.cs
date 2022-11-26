using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EscortBookCustomer.Consumer.Models;

namespace EscortBookCustomer.Consumer.Repositories;

public interface IProfileStatusRepository
{
    Task<ProfileStatus> GetAsync(Expression<Func<ProfileStatus, bool>> expression);

    Task<int> CountAsync(Expression<Func<ProfileStatus, bool>> expression);

    Task CreateAsync(ProfileStatus profileStatus);

    Task UpdateAsync(ProfileStatus profileStatus);

    Task DeleteAsync(Expression<Func<ProfileStatus, bool>> expression);
}
