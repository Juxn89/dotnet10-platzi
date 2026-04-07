using Microsoft.EntityFrameworkCore;

namespace webapi.data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }
}
