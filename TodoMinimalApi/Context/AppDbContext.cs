using Microsoft.EntityFrameworkCore;
using TaskModel = TodoMinimalApi.Models.TaskModel;

namespace TodoMinimalApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TaskModel> Tasks => Set<TaskModel>();
}