using System.ComponentModel.DataAnnotations;

namespace Api_project.Models
{
    public class Login
    {
        [Key]
        public int Id{get;set;}
        public string Email { get; set; }
        public string Password { get; set; }
    }
}