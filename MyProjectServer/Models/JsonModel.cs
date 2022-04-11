namespace MvcMovie.Models
{
    public class DataList
    {
        public DataList()
        {
            Data = new List<DataPage>();
        }
        public IList<DataPage> Data { get; set; }
    }

    public class DataPage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public IList<string> DeptL { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }

    }
}
