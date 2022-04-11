namespace MvcMovie.Models
{
    public class Dept
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public IList<Staff> Staffs { get; set; } = new List<Staff>();
    }
}
