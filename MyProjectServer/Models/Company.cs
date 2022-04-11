using System.ComponentModel.DataAnnotations;

namespace MyProjectServer.Models
{
    public class Company
    {
        [Required]
        public int Id { get; set; } 
        public string Name { get; set; } // название компании
        public List<Staff>? Staffs { get; set; }

    }
}
