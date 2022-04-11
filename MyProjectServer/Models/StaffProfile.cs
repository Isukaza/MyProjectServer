using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class StaffProfile
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        public string Password { get; set; }
        public Staff Staff { get; set; } // навигационное свойство
    }
}
