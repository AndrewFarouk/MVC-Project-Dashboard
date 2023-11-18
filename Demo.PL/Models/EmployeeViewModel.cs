using Demo.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Range(22, 35, ErrorMessage = "Age Must be In Range From 22 To 35")]
        public int? Age { get; set; }

        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
                ErrorMessage = "Address Must be Like 123-Street-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }

        [EmailAddress(ErrorMessage ="Wrong Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage ="Wrong Phone Number")]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        //public DateTime CreationDate { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
