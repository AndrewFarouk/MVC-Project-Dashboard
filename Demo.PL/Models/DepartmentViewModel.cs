using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Code Is Required")]
        public string Code { get; set; }
        
        [Required(ErrorMessage = "Name Is Required")]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }
        
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
    }
}
