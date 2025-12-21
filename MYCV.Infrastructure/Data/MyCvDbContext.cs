using Microsoft.EntityFrameworkCore;
using MYCV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Infrastructure.Data
{
    public class MyCvDbContext:DbContext
    {
        public MyCvDbContext(DbContextOptions<MyCvDbContext> options): base(options) { }
        public DbSet<User> Users { get; set; }
    }
}