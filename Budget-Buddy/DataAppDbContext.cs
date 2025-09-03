using system;

public class Class1
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        "ConnectionStrings": { "Default": "Data Source=app.db" }
    }
}
