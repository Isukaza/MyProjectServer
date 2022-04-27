namespace MyProjectServer.Models
{
     public class ListStaffDTO
    {
        public ListStaffDTO()
        {
            Data = new List<StaffDTO>();
        }
        public List<StaffDTO> Data { get; set; }
    }

    public class StaffDTO
{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public IEnumerable<string>? DeptL { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
    }
}
