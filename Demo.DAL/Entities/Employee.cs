using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Employee : BaseEntity
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; } 
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; }
        public int? DepartmentId { get; set; } // FK
        // FK Optional => OnDelete : Restrict

        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
