using Microsoft.EntityFrameworkCore;
using RegRoma.Models;

namespace RegRoma.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
    
  }

  public DbSet<User> Users => Set<User>();
}