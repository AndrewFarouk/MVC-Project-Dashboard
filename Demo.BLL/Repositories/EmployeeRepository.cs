using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCAppDbContext _context;

        public EmployeeRepository(MVCAppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetEmployeesByDepartmentName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> Search(string name)
            => _context.Employees.Where(emp => emp.Name.Trim().ToLower().Contains(name.Trim().ToLower()));

        public IEnumerable<Employee> GetEmployeesWithDepartment(int? departmentId)
        {
            return _context.Employees
                .Include(e => e.Department)
                .Where(e => e.DepartmentId == departmentId)
                .ToList();

        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
           => _context.Employees.Where(E => E.Address == address);
        
    }
}