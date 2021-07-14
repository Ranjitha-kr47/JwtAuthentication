using System.ComponentModel.DataAnnotations;

namespace Api_project.Models
{
    public class Designation
    {
        [Key]
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int DepartmentId { get; set; }
        public Department department { get; set; }
    }
}