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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly MVCAppDbContext _context;

        public DepartmentRepository(MVCAppDbContext context) : base (context)
        {
            _context = context;
        }

        public Department GetDepartmentNameByIdOfEmployee(int? id)
        {
            return _context.Departments.FirstOrDefault(d => d.Id == id);
        }
    }
}
