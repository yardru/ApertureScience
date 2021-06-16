using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApertureScience;

namespace ApertureScience
{
    public class EmployeesManager : DbContext
    {
        public EmployeesManager (DbContextOptions<EmployeesManager> options)
            : base(options)
        {
            if (Database.EnsureCreated())
            {
                Add(admin);
                SaveChanges();
            }
        }

        public IEnumerable<Employee> GetAll()
        {
            return Employees.AsNoTracking().AsEnumerable();
        }

        public async Task<Employee> GetAsync(int id)
        {
            return await Employees.FindAsync(id);
        }

        public Employee.Roles GetRole(int id)
        {
            var emp = Employees.AsNoTracking().FirstOrDefault(employee => employee.Id == id);
            return emp != null ? emp.Role : Employee.Roles.UNDEFINED;
        }

        public async Task AddAsync(Employee employee)
        {
            Employees.Add(employee);
            await SaveChangesAsync();
        }

        public bool IsExists(int id)
        {
            return Employees.Any(e => e.Id == id);
        }

        public async Task UpdateAsync(Employee employee)
        {
            Entry(employee).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task RemoveAsync(Employee employee)
        {
            Employees.Remove(employee);
            await SaveChangesAsync();
        }

        private DbSet<Employee> Employees { get; set; }
        private static readonly Employee admin = new Employee {
            Email = "admin@admin.ru",
            Password = "hardpass",
            Role = Employee.Roles.ADMIN,
            FirstName = "admin",
            LastName = "admin",
        };
    }
}
