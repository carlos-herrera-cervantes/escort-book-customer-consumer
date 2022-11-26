using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Linq;
using EscortBookCustomer.Consumer.Contexts;
using EscortBookCustomer.Consumer.Models;

namespace EscortBookCustomer.Consumer.Repositories;

public class ProfileRepository : IProfileRepository
{
    #region snippet_Properties

    private readonly EscortBookCustomerConsumerContext _context;

    #endregion

    #region snippet_Constructors

    public ProfileRepository(EscortBookCustomerConsumerContext context)
        => _context = context;

    #endregion

    #region snippet_ActionMethods

    public async Task<int> CountAsync(Expression<Func<Profile, bool>> expression)
        => await _context.Profiles.CountAsync(expression);

    public async Task CreateAsync(Profile profile)
    {
        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<Profile, bool>> expression)
    {
        var candidatesToDelete = await _context.Profiles.Where(expression).ToListAsync();
        _context.Profiles.RemoveRange(candidatesToDelete);
        await _context.SaveChangesAsync();
    }

    #endregion
}
