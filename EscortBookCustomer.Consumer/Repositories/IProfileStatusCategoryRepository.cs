using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EscortBookCustomer.Consumer.Models;

namespace EscortBookCustomer.Consumer.Repositories;

public interface IProfileStatusCategoryRepository
{
    Task CreateAsync(ProfileStatusCategory category);

    Task<ProfileStatusCategory> GetByName(string name);

    Task DeleteAsync(Expression<Func<ProfileStatusCategory, bool>> expression);
}
