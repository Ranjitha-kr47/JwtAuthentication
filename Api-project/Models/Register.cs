using System.ComponentModel.DataAnnotations;

namespace Api_project.Models
{
    public class Register
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email{get ;set;}
        public string Password{get;set;}
        public int Age { get; set; }
        public string DateOfJoining { get; set; }
        public int DepartmentId { get; set; }
        public Department department { get; set; }
        public int DesignationId { get; set; }
        public Designation designation { get; set; }
    }
}