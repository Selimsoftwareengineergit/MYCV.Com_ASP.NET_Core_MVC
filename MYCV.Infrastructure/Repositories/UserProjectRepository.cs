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
    public class UserProjectRepository: IUserProjectRepository
    {
        private readonly MyCvDbContext _context;

        public UserProjectRepository(MyCvDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all project by UserId (direct, preferred)
        /// </summary>
        public async Task<List<UserProject>> GetByUserIdAsync(int userId)
        {
            return await _context.UserProjects
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<UserProject?> GetByIdAsync(int id)
        {
            return await _context.UserProjects
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task AddAsync(UserProject project)
        {
            await _context.UserProjects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserProject project)
        {
            _context.UserProjects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserProject project)
        {
            project.IsDeleted = true;
            project.DeletedDate = DateTime.UtcNow;
            _context.UserProjects.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}
