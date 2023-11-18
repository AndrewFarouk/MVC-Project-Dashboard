using Demo.DAL.Entities;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable<Employee> GetEmployeesByAddress(string address);
        IEnumerable<Employee> GetEmployeesByDepartmentName(string name);
        IEnumerable<Employee> GetEmployeesWithDepartment(int? id);
        IEnumerable<Employee> Search(string name);

        //Employee GetEmployeeById(int? id);
        //IEnumerable<Employee> GetAllEmployees();
        //int Add(Employee employee);
        //int Update(Employee employee);
        //int Delete(Employee employee);
    }
}
