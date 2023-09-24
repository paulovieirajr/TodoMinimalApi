using Microsoft.EntityFrameworkCore;
using Task = TodoMinimalApi.Models.Task;

namespace TodoMinimalApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Task> Tasks => Set<Task>();
}