using webapi.models;

namespace webapi.services;

public interface ITaskService
{
  Task<IEnumerable<TaskItem>> GetAllAsync();
  Task<TaskItem?> GetByIdAsync(int id);
  Task<TaskItem> CreateAsync(TaskItem taskItem);
  Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem);
  Task<bool> DeleteAsync(int id);
}
