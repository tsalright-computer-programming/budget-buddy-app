using Budget_Buddy.Models;
using Microsoft.EntityFrameworkCore;
using System;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Category> Categories { get; set; }
}
