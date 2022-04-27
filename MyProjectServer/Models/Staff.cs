namespace MyProjectServer.Models
{
    public class Staff
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int CompanyForeignKey { get; set; }  // внешний ключ
        public Company Company { get; set; }        // навигационное свойство
        public ICollection<Dept>? Depts { get; set; }
        public StaffProfile Profile { get; set; }   // навигационное свойство
    }
}
