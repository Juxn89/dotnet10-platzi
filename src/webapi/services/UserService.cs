using Microsoft.EntityFrameworkCore;

using webapi.data;
using webapi.models;

namespace webapi.services;

public class UserService : IUserService
{
  private readonly AppDbContext _context;

  public UserService(AppDbContext context)
  {
    _context = context;
  }

  public async Task<User> CreateAsync(User user)
  {
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();
    return user;
  }

  public async Task<IEnumerable<User>> GetAllAsync()
  {
    return await _context.Users.AsNoTracking().ToListAsync();
  }

  public async Task<User?> GetByIdAsync(int id)
  {
    return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
  }

  public async Task<User?> UpdateAsync(int id, User user)
  {
    User? existingUser = await _context.Users.FindAsync(id);
    if (existingUser == null)
    {
      return null;
    }

    existingUser.Name = user.Name;
    existingUser.Email = user.Email;

    await _context.SaveChangesAsync();
    return existingUser;
  }

  public async Task<bool> DeleteAsync(int id)
  {
    User? user = await _context.Users.FindAsync(id);
    if (user == null)
    {
      return false;
    }

    _context.Users.Remove(user);
    await _context.SaveChangesAsync();
    return true;
  }
}
