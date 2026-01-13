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
    public class UserRepository: IUserRepository
    {
        private readonly MyCvDbContext _context;

        public UserRepository(MyCvDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var normalizedEmail = email.Trim().ToLower();

            return await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email.ToLower() == normalizedEmail &&
                    !x.IsDeleted &&
                    x.IsActive);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}