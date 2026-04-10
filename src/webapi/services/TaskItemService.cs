using Microsoft.EntityFrameworkCore;

using webapi.data;
using webapi.models;

namespace webapi.services;

public class TaskItemService : ITaskService
{
  private readonly AppDbContext _context;
  public TaskItemService(AppDbContext context)
  {
    _context = context;
  }

  public async Task<TaskItem> CreateAsync(TaskItem taskItem)
  {
    await _context.TaskItems.AddAsync(taskItem);
    await _context.SaveChangesAsync();
    return taskItem;
  }

  public async Task<IEnumerable<TaskItem>> GetAllAsync()
  {
    return await _context.TaskItems.AsNoTracking().ToListAsync();
  }

  public Task<TaskItem?> GetByIdAsync(int id)
  {
    return _context.TaskItems.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
  }

  public async Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem)
  {
    TaskItem? existingTask = await _context.TaskItems.FindAsync(id);
    if (existingTask == null)
    {
      return null;
    }

    existingTask.Title = taskItem.Title;
    existingTask.IsCompleted = taskItem.IsCompleted;
    existingTask.UserId = taskItem.UserId;

    await _context.SaveChangesAsync();
    return existingTask;
  }

  public async Task<bool> DeleteAsync(int id)
  {
    TaskItem? taskItem = await _context.TaskItems.FindAsync(id);
    if (taskItem == null)
    {
      return false;
    }

    _context.TaskItems.Remove(taskItem);
    await _context.SaveChangesAsync();
    return true;
  }
}
