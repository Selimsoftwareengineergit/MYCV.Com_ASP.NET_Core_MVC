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
    public class UserPersonalDetailRepository : IUserPersonalDetailRepository
    {
        private readonly MyCvDbContext _context;

        public UserPersonalDetailRepository(MyCvDbContext context)
        {
            _context = context;
        }

        public async Task<UserPersonalDetail?> GetByUserIdAsync(int userId)
        {
            return await _context.UserPersonalDetails
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    !x.IsDeleted &&
                    x.IsActive);
        }

        public async Task<UserPersonalDetail?> GetByIdAsync(int id)
        {
            return await _context.UserPersonalDetails
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    !x.IsDeleted &&
                    x.IsActive);
        }

        public async Task AddAsync(UserPersonalDetail userCv)
        {
            _context.UserPersonalDetails.Add(userCv);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserPersonalDetail userCv)
        {
            _context.UserPersonalDetails.Update(userCv);
            await _context.SaveChangesAsync();
        }
    }
}