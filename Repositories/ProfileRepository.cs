using System.Threading.Tasks;
using EscortBookCustomerConsumer.Contexts;
using EscortBookCustomerConsumer.Models;

namespace EscortBookCustomerConsumer.Repositories;

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

    public async Task CreateAsync(Profile profile)
    {
        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();
    }

    #endregion
}
