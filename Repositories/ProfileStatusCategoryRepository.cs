using System.Threading.Tasks;
using EscortBookCustomerConsumer.Contexts;
using EscortBookCustomerConsumer.Models;
using Microsoft.EntityFrameworkCore;

namespace EscortBookCustomerConsumer.Repositories;

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

    #endregion
}
