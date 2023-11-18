using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IEmployeeRepository employeeRepository;

        public DepartmentController(
            //IDepartmentRepository departmentRepository,
            ILogger<DepartmentController> logger,
            IUnitOfWork unitOfWork,
            //IEmployeeRepository employeeRepository
            IMapper mapper
            )
        {
            //_departmentRepository = departmentRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //this.employeeRepository = employeeRepository;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.departmentRepository.GetAllAsync();
            var mappedDepartment = _mapper.Map<IEnumerable<DepartmentViewModel>>( departments );
            //ViewData["Message"] = "Hello From View Data";
            //ViewBag.MessageBag = "Hello From View Bag";

            //TempData.Keep("Message");
          
            return View(mappedDepartment);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if(ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(departmentVM);
               await _unitOfWork.departmentRepository.AddAsync(department);
               int Result = await _unitOfWork.CompleteAsync();
                if(Result > 0)
                    TempData["Message"] = "Department Created Successfully";
                
                return RedirectToAction("Index");
            }
            return View(departmentVM);
        }

        public async Task<IActionResult> Details(int? id , string ViewName = "Details")
        {
            if (id is null)
                return NotFound();
            //return RedirectToAction("Error", "Home");
            var department = await _unitOfWork.departmentRepository.GetByIdAsync(id);
            var mappedDepartment = _mapper.Map<DepartmentViewModel>( department);

            if (department is null)
                return RedirectToAction("Error", "Home");

            return View(ViewName, mappedDepartment);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return NotFound();

            var department = await _unitOfWork.departmentRepository.GetByIdAsync(id);
            var mappedDepartment = _mapper.Map<DepartmentViewModel>(department);

            if (mappedDepartment is null)
                return NotFound();

            return View(mappedDepartment);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                   var department = _mapper.Map<Department>(departmentVM);
                   _unitOfWork.departmentRepository.Update(department);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction("Index");

                }catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

             }
            return View(departmentVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            try
            {
                _unitOfWork.departmentRepository.Delete(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);
            }
        }

        public IActionResult GetAllEmployees(int? id)
        {
            if (id == null)
                return NotFound();

            var employees = _unitOfWork.employeeRepository.GetEmployeesWithDepartment(id);
            var mappedEmployees = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            if (mappedEmployees == null)
                return NotFound();

            return View(mappedEmployees);
        }
    }

}
