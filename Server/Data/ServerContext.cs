using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApertureScience;

namespace ApertureScience
{
    public class ServerContext : DbContext
    {
        public ServerContext (DbContextOptions<ServerContext> options)
            : base(options)
        {
            if (Database.EnsureCreated())
            {
                Add(admin);
                SaveChanges();
            }
        }

        public DbSet<Employee> Employee { get; set; }
        private static readonly Employee admin = new Employee("xxx@yyy.com", "drowssap", "Admin", "");
    }
}
