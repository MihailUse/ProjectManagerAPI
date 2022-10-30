namespace DAL.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Project? Project { get; set; }
        public List<Task> Tasks { get; set; }
    }
}