using Microsoft.EntityFrameworkCore;

public sealed class PenisDbContext(DbContextOptions<PenisDbContext> options) : DbContext(options)
{
    public DbSet<Penis> Penises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // for future configuration
    }
}