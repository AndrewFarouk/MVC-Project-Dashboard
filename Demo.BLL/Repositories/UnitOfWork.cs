using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        private readonly MVCAppDbContext _dbContext;
        public IEmployeeRepository employeeRepository { get; set; }
        public IDepartmentRepository departmentRepository { get; set; }

        public UnitOfWork(MVCAppDbContext dbContext)
        {
            employeeRepository = new EmployeeRepository(dbContext);
            departmentRepository = new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();

        public void Dispose()
            => _dbContext.Dispose();
    }
}
