using Microsoft.EntityFrameworkCore;

public class UTEDbContext : DbContext
{
  public DbSet<MonHoc> MonHocs { get; set; }

  private static bool isCreated = false;

  public UTEDbContext()
  {
    if (!isCreated)
    {
      this.Database.EnsureCreated();
      isCreated = true;
    }
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer("Server=.; Database=MonHocUTE; Integrated Security=True;");
    base.OnConfiguring(optionsBuilder);
  }
}