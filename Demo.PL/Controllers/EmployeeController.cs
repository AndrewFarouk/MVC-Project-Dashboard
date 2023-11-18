using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork=unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearchValue = "")
        {
            IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModel> mappedEmployees;

            if (string.IsNullOrEmpty(SearchValue))
                employees = await _unitOfWork.employeeRepository.GetAllAsync();
            
            else
                employees = _unitOfWork.employeeRepository.Search(SearchValue);
           
            mappedEmployees = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            await _unitOfWork.CompleteAsync();

            return View(mappedEmployees);
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _unitOfWork.departmentRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            ModelState["Department"].ValidationState = ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                #region Manual Mapping
                #endregion
                employeeVM.ImageUrl = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var mappedEmployee = _mapper.Map<Employee>(employeeVM);

                await _unitOfWork.employeeRepository.AddAsync(mappedEmployee);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = await _unitOfWork.departmentRepository.GetAllAsync();
            return View(employeeVM);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
                return NotFound();

            var employee = await _unitOfWork.employeeRepository.GetByIdAsync(id);
            var mappedEmployees = _mapper.Map<EmployeeViewModel>(employee);

            if(mappedEmployees == null)
                return NotFound();

            ViewBag.Departments = await _unitOfWork.employeeRepository.GetAllAsync();
            return View(mappedEmployees);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if(id  == null)
                return NotFound();

            var employee = await _unitOfWork.employeeRepository.GetByIdAsync(id);
            var mappedEmployees = _mapper.Map<EmployeeViewModel>(employee);

            if(mappedEmployees == null)
                return NotFound();

            ViewBag.Departments = await _unitOfWork.departmentRepository.GetAllAsync();
            return View(mappedEmployees);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, EmployeeViewModel employeeVM)
        {
            if(id != employeeVM.Id)
                return NotFound();

            try
            {
                ModelState["Department"].ValidationState = ModelValidationState.Valid;
                if (ModelState.IsValid)
                {
                    var mappedEmployee = _mapper.Map<Employee>(employeeVM);
                    
                    if(employeeVM.ImageUrl is not  null)
                        mappedEmployee.ImageUrl = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                    _unitOfWork.employeeRepository.Update(mappedEmployee);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }

            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            ViewBag.Departments = await _unitOfWork.departmentRepository.GetAllAsync();
            return View(employeeVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _unitOfWork.employeeRepository.GetByIdAsync(id);
            var mappedEmployees = _mapper.Map<EmployeeViewModel>(employee);

            if (mappedEmployees == null)
                return NotFound();

            ViewBag.Departments = await _unitOfWork.departmentRepository.GetAllAsync();
            return View(mappedEmployees);
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, Employee employee)
        {
            if(id != employee.Id)
                return NotFound();

            try
            {
                
                _unitOfWork.employeeRepository.Delete(employee);
                var Result = await _unitOfWork.CompleteAsync();
                if (Result > 0 && employee.ImageUrl is not null)
                    DocumentSettings.DeleteFile(employee.ImageUrl, "Images");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employee);
            }
            
        }
    }
}
