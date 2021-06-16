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

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeAsync(int id)
        {
            return await Employees.FindAsync(id);
        }

        public async Task<Employee> GetEmployeeAsyncAsNoTracking(int id)
        {
            return await Employees.AsNoTracking().FirstOrDefaultAsync(employee => employee.Id == id);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            Employees.Add(employee);
            await SaveChangesAsync();
        }

        public bool IsEmployeeExists(int id)
        {
            return Employees.Any(e => e.Id == id);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            Entry(employee).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task RemoveEmployeeAsync(Employee employee)
        {
            foreach (var photoName in employee.PhotoNamesList)
                PhotoProcessor.GetProcessor(photoName).Delete();

            Employees.Remove(employee);
            await SaveChangesAsync();
        }

        private DbSet<Employee> Employees { get; set; }
        private static readonly Employee admin = new Employee("xxx@yyy.com", "drowssap", "Admin", "");
    }
}
