using System.ComponentModel.DataAnnotations;

namespace MyProjectServer.Models
{
    public class Dept
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public ICollection<Staff> Staffs { get; set; } = new List<Staff>();
    }
}
