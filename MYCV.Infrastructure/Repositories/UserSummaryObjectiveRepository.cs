using Microsoft.EntityFrameworkCore;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
using MYCV.Infrastructure.Data;

namespace MYCV.Infrastructure.Repositories
{
    public class UserSummaryObjectiveRepository : IUserSummaryObjectiveRepository
    {
        private readonly MyCvDbContext _context;

        public UserSummaryObjectiveRepository(MyCvDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Summary & Objective by UserId (direct, preferred)
        /// </summary>
        public async Task<List<UserSummaryObjective>> GetByUserIdAsync(int userId)
        {
            return await _context.UserSummaryObjectives
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<UserSummaryObjective?> GetByIdAsync(int id)
        {
            return await _context.UserSummaryObjectives
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task AddAsync(UserSummaryObjective summaryObjective)
        {
            await _context.UserSummaryObjectives.AddAsync(summaryObjective);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserSummaryObjective summaryObjective)
        {
            _context.UserSummaryObjectives.Update(summaryObjective);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserSummaryObjective summaryObjective)
        {
            summaryObjective.IsDeleted = true;
            summaryObjective.DeletedDate = DateTime.UtcNow;
            _context.UserSummaryObjectives.Update(summaryObjective);
            await _context.SaveChangesAsync();
        }
    }
}
