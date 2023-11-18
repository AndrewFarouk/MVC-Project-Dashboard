using Demo.DAL.Entities;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable<Employee> GetEmployeesByAddress(string address);
        IEnumerable<Employee> GetEmployeesByDepartmentName(string name);
        IEnumerable<Employee> GetEmployeesWithDepartment(int? id);
        IEnumerable<Employee> Search(string name);
    }
}
