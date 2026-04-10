using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.models;
using webapi.services;

namespace webapi.Controllers;

/// <summary>
/// Controller for managing tasks
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class TasksController : ControllerBase
{
  private readonly ITaskService _taskService;

  public TasksController(ITaskService taskService)
  {
    _taskService = taskService;
  }

  /// <summary>
  /// Get all tasks
  /// </summary>
  /// <returns>List of all tasks</returns>
  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<TaskItem>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
  {
    IEnumerable<TaskItem> tasks = await _taskService.GetAllAsync();
    return Ok(tasks);
  }

  /// <summary>
  /// Get a task by ID
  /// </summary>
  /// <param name="id">Task ID</param>
  /// <returns>Task with the specified ID</returns>
  [HttpGet("{id}")]
  [ProducesResponseType(typeof(TaskItem), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<TaskItem>> GetById(int id)
  {
    TaskItem? task = await _taskService.GetByIdAsync(id);
    if (task == null)
    {
      return NotFound(new { message = $"Task with ID {id} not found" });
    }
    return Ok(task);
  }

  /// <summary>
  /// Create a new task
  /// </summary>
  /// <param name="taskItem">Task data</param>
  /// <returns>Created task</returns>
  [HttpPost]
  [ProducesResponseType(typeof(TaskItem), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<TaskItem>> Create([FromBody] TaskItem taskItem)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    TaskItem createdTask = await _taskService.CreateAsync(taskItem);
    return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
  }

  /// <summary>
  /// Update an existing task
  /// </summary>
  /// <param name="id">Task ID</param>
  /// <param name="taskItem">Updated task data</param>
  /// <returns>Updated task</returns>
  [HttpPut("{id}")]
  [ProducesResponseType(typeof(TaskItem), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<TaskItem>> Update(int id, [FromBody] TaskItem taskItem)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    TaskItem? updatedTask = await _taskService.UpdateAsync(id, taskItem);
    if (updatedTask == null)
    {
      return NotFound(new { message = $"Task with ID {id} not found" });
    }

    return Ok(updatedTask);
  }

  /// <summary>
  /// Delete a task
  /// </summary>
  /// <param name="id">Task ID</param>
  /// <returns>No content</returns>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Delete(int id)
  {
    bool deleted = await _taskService.DeleteAsync(id);
    if (!deleted)
    {
      return NotFound(new { message = $"Task with ID {id} not found" });
    }

    return NoContent();
  }
}
