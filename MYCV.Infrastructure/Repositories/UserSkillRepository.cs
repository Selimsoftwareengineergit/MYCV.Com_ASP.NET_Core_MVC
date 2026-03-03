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
    public class UserSkillRepository: IUserSkillRepository
    {
        private readonly MyCvDbContext _context;

        public UserSkillRepository(MyCvDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all skill by UserId (direct, preferred)
        /// </summary>
        public async Task<List<UserSkill>> GetByUserIdAsync(int userId)
        {
            return await _context.UserSkills
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<UserSkill?> GetByIdAsync(int id)
        {
            return await _context.UserSkills
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task AddAsync(UserSkill skill)
        {
            await _context.UserSkills.AddAsync(skill);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserSkill skill)
        {
            _context.UserSkills.Update(skill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserSkill skill)
        {
            skill.IsDeleted = true;
            skill.DeletedDate = DateTime.UtcNow;
            _context.UserSkills.Update(skill);
            await _context.SaveChangesAsync();
        }
    }
}
