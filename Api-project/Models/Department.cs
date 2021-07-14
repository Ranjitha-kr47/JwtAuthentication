using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api_project.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public int DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public List<Designation> designation { get; set; }
    }
}