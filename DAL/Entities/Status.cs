namespace DAL.Entities
{
    public class Status
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public Project? Project { get; set; }
        public List<Task> Tasks { get; set; } = null!;
    }
}