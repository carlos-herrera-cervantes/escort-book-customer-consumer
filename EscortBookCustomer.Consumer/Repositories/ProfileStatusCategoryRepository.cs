using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EscortBookCustomer.Consumer.Contexts;
using EscortBookCustomer.Consumer.Models;

namespace EscortBookCustomer.Consumer.Repositories;

public class ProfileStatusCategoryRepository : IProfileStatusCategoryRepository
{
    #region snippet_Properties

    private readonly EscortBookCustomerConsumerContext _context;

    #endregion

    #region snippet_Constructors

    public ProfileStatusCategoryRepository(EscortBookCustomerConsumerContext context)
        => _context = context;

    #endregion

    #region snippet_ActionMethods

    public async Task<ProfileStatusCategory> GetByName(string name)
        => await _context.ProfileStatusCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name);

    public async Task CreateAsync(ProfileStatusCategory category)
    {
        await _context.ProfileStatusCategories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<ProfileStatusCategory, bool>> expression)
    {
        var candidatesToDelete = await _context.ProfileStatusCategories.Where(expression).ToListAsync();
        _context.ProfileStatusCategories.RemoveRange(candidatesToDelete);
        await _context.SaveChangesAsync();
    }

    #endregion
}
