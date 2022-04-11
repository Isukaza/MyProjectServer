using System.ComponentModel.DataAnnotations;

namespace MyProjectServer.Models
{
    public class StaffProfile
    {
        [Required]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int StaffForeignKey { get; set; }
        public Staff Staff { get; set; } // навигационное свойство
    }
}
