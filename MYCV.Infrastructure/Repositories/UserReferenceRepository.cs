using Microsoft.EntityFrameworkCore;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
using MYCV.Infrastructure.Data;

namespace MYCV.Infrastructure.Repositories
{
    public class UserReferenceRepository: IUserReferenceRepository
    {
        private readonly MyCvDbContext _context;

        public UserReferenceRepository(MyCvDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all reference by UserId (direct, preferred)
        /// </summary>
        public async Task<List<UserReference>> GetByUserIdAsync(int userId)
        {
            return await _context.UserReferences
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<UserReference?> GetByIdAsync(int id)
        {
            return await _context.UserReferences
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task AddAsync(UserReference reference)
        {
            await _context.UserReferences.AddAsync(reference);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserReference reference)
        {
            _context.UserReferences.Update(reference);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserReference reference)
        {
            reference.IsDeleted = true;
            reference.DeletedDate = DateTime.UtcNow;
            _context.UserReferences.Update(reference);
            await _context.SaveChangesAsync();
        }
    }
}
