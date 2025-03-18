using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnePrivateNavigation.Data.Entities;
using OnePrivateNavigation.Data.Helpers;

namespace OnePrivateNavigation.Data
{
    public class OnePrivateNavigationDbContext : DbContext
    {
        public OnePrivateNavigationDbContext(DbContextOptions<OnePrivateNavigationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
