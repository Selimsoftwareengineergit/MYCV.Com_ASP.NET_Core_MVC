using Microsoft.EntityFrameworkCore;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
using MYCV.Infrastructure.Data;

namespace MYCV.Infrastructure.Repositories
{
    public class UserEducationRepository : IUserEducationRepository
    {
        private readonly MyCvDbContext _context;

        public UserEducationRepository(MyCvDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all educations by UserId (direct, preferred)
        /// </summary>
        public async Task<List<UserEducation>> GetByUserIdAsync(int userId)
        {
            return await _context.UserEducations
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<UserEducation?> GetByIdAsync(int id)
        {
            return await _context.UserEducations
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task AddAsync(UserEducation education)
        {
            await _context.UserEducations.AddAsync(education);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserEducation education)
        {
            _context.UserEducations.Update(education);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserEducation education)
        {
            education.IsDeleted = true;
            education.DeletedDate = DateTime.UtcNow;
            _context.UserEducations.Update(education);
            await _context.SaveChangesAsync();
        }
    }
}