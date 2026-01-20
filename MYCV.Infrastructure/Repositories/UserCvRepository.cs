using Microsoft.EntityFrameworkCore;
using MYCV.Application.Interfaces;
using MYCV.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Infrastructure.Repositories
{
    public class UserCvRepository : IUserCvRepository
    {
        private readonly MyCvDbContext _context;

        public UserCvRepository(MyCvDbContext context)
        {
            _context = context;
        }

        public async Task<UserCv?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var normalizedEmail = email.Trim().ToLower();

            return await _context.UserCvs
                .Include(x => x.Educations)
                .Include(x => x.Experiences)
                .Include(x => x.Skills)
                .Include(x => x.Projects)
                .Include(x => x.Languages)
                .FirstOrDefaultAsync(x =>
                    x.Email.ToLower() == normalizedEmail &&
                    !x.IsDeleted &&
                    x.IsActive);
        }

        public async Task<UserCv?> GetByIdAsync(int id)
        {
            return await _context.UserCvs
                .Include(x => x.Educations)
                .Include(x => x.Experiences)
                .Include(x => x.Skills)
                .Include(x => x.Projects)
                .Include(x => x.Languages)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    !x.IsDeleted &&
                    x.IsActive);
        }

        public async Task AddAsync(UserCv userCv)
        {
            _context.UserCvs.Add(userCv);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserCv userCv)
        {
            _context.UserCvs.Update(userCv);
            await _context.SaveChangesAsync();
        }
    }
}