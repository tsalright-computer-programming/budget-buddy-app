namespace Budget_Buddy.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CategoryType Type { get; set; }
        public bool IsArchived { get; set; } = false;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}