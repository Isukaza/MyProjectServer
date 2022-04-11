using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } // название компании
        public virtual ICollection<Staff>? Staffs { get; set; }

    }
}
