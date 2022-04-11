using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class Staff
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int CompanyId { get; set; }         // внешний ключ
        public int ProfileId { get; set; }         // внешний ключ
        public Company Company { get; set; }      // навигационное свойство
        public IList<Dept> Depts { get; set; } = new List<Dept>();
        public StaffProfile Profile { get; set; } // навигационное свойство
    }
}
