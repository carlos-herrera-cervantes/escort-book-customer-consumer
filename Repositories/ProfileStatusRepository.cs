using System.Threading.Tasks;
using EscortBookCustomerConsumer.Contexts;
using EscortBookCustomerConsumer.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ProfileStatus> GetByProfileIdAsync(string profileId)
            => await _context
                .ProfileStatus
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.CustomerID == profileId);

        public async Task CreateAsync(ProfileStatus profileStatus)
        {
            await _context.ProfileStatus.AddAsync(profileStatus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProfileStatus profileStatus)
        {
            _context.Entry(profileStatus).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}