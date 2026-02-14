using Microsoft.EntityFrameworkCore;
using MYCV.Application.Interfaces;
using MYCV.Domain.Entities;
using MYCV.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Infrastructure.Repositories
{
    public class UserExperienceRepository: IUserExperienceRepository
    {
        private readonly MyCvDbContext _context;

        public UserExperienceRepository(MyCvDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all experience by UserId (direct, preferred)
        /// </summary>
        public async Task<List<UserExperience>> GetByUserIdAsync(int userId)
        {
            return await _context.UserExperiences
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<UserExperience?> GetByIdAsync(int id)
        {
            return await _context.UserExperiences
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task AddAsync(UserExperience experience)
        {
            await _context.UserExperiences.AddAsync(experience);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserExperience experience)
        {
            _context.UserExperiences.Update(experience);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserExperience experience)
        {
            experience.IsDeleted = true;
            experience.DeletedDate = DateTime.UtcNow;
            _context.UserExperiences.Update(experience);
            await _context.SaveChangesAsync();
        }
    }
}
