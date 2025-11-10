namespace Budget_Buddy.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public DateOnly PostedDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int AmountCents { get; set; } // positive
        public Guid? CategoryId { get; set; } // null when splits exist later
        public Category? Category { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}