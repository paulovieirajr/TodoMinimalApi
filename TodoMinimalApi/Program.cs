using Microsoft.EntityFrameworkCore;
using TodoMinimalApi.Context;
using TodoMinimalApi.Dtos;
using TodoMinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseInMemoryDatabase("TasksDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/tasks", async (AppDbContext context) =>
{
    var tasks = await context.Tasks.ToListAsync();
    return tasks.Count > 0 ? Results.Ok(tasks) : Results.NotFound();
});

app.MapGet("tasks/done", async (AppDbContext context) =>
{
    var tasks = await context.Tasks.Where(task => task.IsCompleted).ToListAsync();
    return tasks.Count > 0 ? Results.Ok(tasks) : Results.NotFound();
});

app.MapGet("/tasks/{id}", async (AppDbContext context, int id) =>
{
    var task = await context.Tasks.FindAsync(id);
    return task is not null ? Results.Ok(task) : Results.NotFound();
});

app.MapPost("/tasks", async (AppDbContext context, TaskDto taskDto) =>
{
    var task = new TaskModel(taskDto.Title, false);
    await context.Tasks.AddAsync(task);
    await context.SaveChangesAsync();
    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapPut("/tasks/{id}", async (AppDbContext context, int id, TaskModel taskModel) =>
{
    if (id != taskModel.Id)
    {
        return Results.BadRequest("Id and taskModel.Id must be the same");
    }

    var task = await context.Tasks.FindAsync(id);
    if (task is null)
    {
        return Results.NotFound();
    }

    task.Title = taskModel.Title;
    task.IsCompleted = taskModel.IsCompleted;

    await context.SaveChangesAsync();
    return Results.Ok(task);
});

app.MapDelete("/tasks/{id}", async (AppDbContext context, int id) =>
{
    var taskModel = await context.Tasks.FindAsync(id);
    if (taskModel is null)
    {
        return Results.NotFound();
    }

    context.Tasks.Remove(taskModel);
    await context.SaveChangesAsync();
    return Results.NoContent();
});


app.Run();