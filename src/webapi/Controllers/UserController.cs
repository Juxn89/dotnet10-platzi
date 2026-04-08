using Microsoft.AspNetCore.Mvc;
using webapi.models;
using webapi.services;

namespace webapi.Controllers;

/// <summary>
/// Controller for managing users
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UserController : ControllerBase
{
  private readonly IUserService _userService;

  public UserController(IUserService userService)
  {
    _userService = userService;
  }

  /// <summary>
  /// Get all users
  /// </summary>
  /// <returns>List of all users</returns>
  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<User>>> GetAll()
  {
    IEnumerable<User> users = await _userService.GetAllAsync();
    return Ok(users);
  }

  /// <summary>
  /// Get a user by ID
  /// </summary>
  /// <param name="id">User ID</param>
  /// <returns>User with the specified ID</returns>
  [HttpGet("{id}")]
  [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<User>> GetById(int id)
  {
    User? user = await _userService.GetByIdAsync(id);
    if (user == null)
    {
      return NotFound(new { message = $"User with ID {id} not found" });
    }
    return Ok(user);
  }

  /// <summary>
  /// Create a new user
  /// </summary>
  /// <param name="user">User data</param>
  /// <returns>Created user</returns>
  [HttpPost]
  [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<User>> Create([FromBody] User user)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    User createdUser = await _userService.CreateAsync(user);
    return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
  }

  /// <summary>
  /// Update an existing user
  /// </summary>
  /// <param name="id">User ID</param>
  /// <param name="user">Updated user data</param>
  /// <returns>Updated user</returns>
  [HttpPut("{id}")]
  [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<User>> Update(int id, [FromBody] User user)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    User? updatedUser = await _userService.UpdateAsync(id, user);
    if (updatedUser == null)
    {
      return NotFound(new { message = $"User with ID {id} not found" });
    }

    return Ok(updatedUser);
  }

  /// <summary>
  /// Delete a user
  /// </summary>
  /// <param name="id">User ID</param>
  /// <returns>No content</returns>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Delete(int id)
  {
    bool deleted = await _userService.DeleteAsync(id);
    if (!deleted)
    {
      return NotFound(new { message = $"User with ID {id} not found" });
    }

    return NoContent();
  }
}
