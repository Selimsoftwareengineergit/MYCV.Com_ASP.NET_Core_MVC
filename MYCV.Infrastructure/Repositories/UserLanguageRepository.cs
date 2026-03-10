using Microsoft.EntityFrameworkCore;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
using MYCV.Infrastructure.Data;

namespace MYCV.Infrastructure.Repositories
{
    public class UserLanguageRepository: IUserLanguageRepository
    {
        private readonly MyCvDbContext _context;

        public UserLanguageRepository(MyCvDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all language by UserId (direct, preferred)
        /// </summary>
        public async Task<List<UserLanguage>> GetByUserIdAsync(int userId)
        {
            return await _context.UserLanguages
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<UserLanguage?> GetByIdAsync(int id)
        {
            return await _context.UserLanguages
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task AddAsync(UserLanguage language)
        {
            await _context.UserLanguages.AddAsync(language);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserLanguage language)
        {
            _context.UserLanguages.Update(language);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserLanguage language)
        {
            language.IsDeleted = true;
            language.DeletedDate = DateTime.UtcNow;
            _context.UserLanguages.Update(language);
            await _context.SaveChangesAsync();
        }
    }
}
