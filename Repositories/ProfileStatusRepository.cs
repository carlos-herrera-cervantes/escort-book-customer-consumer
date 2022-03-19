using System.Threading.Tasks;
using EscortBookCustomerConsumer.Contexts;
using EscortBookCustomerConsumer.Models;

namespace EscortBookCustomerConsumer.Repositories
{
    public class ProfileStatusRepository : IProfileStatusRepository
    {
        #region snippet_Properties

        private readonly EscortBookCustomerConsumerContext _context;

        #endregion

        #region snippet_Constructors
        
        public ProfileStatusRepository(EscortBookCustomerConsumerContext context)
            => _context = context;

        #endregion

        #region snippet_ActionMethods

        public async Task CreateAsync(ProfileStatus profileStatus)
        {
            await _context.ProfileStatus.AddAsync(profileStatus);
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}