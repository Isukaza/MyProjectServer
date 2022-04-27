namespace MyProjectServer.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } // название компании
        public List<Staff>? Staffs { get; set; }

    }
}
