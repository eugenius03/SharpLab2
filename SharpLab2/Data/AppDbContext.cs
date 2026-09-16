using Microsoft.EntityFrameworkCore;
using SharpLab2.Models;

namespace SharpLab2.Data;

public class AppDbContext : DbContext
{
    private const string ConnectionString = "Data Source=tickets.db";

    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<CarriageType> CarriageTypes => Set<CarriageType>();
    public DbSet<Passenger> Passengers => Set<Passenger>();
    public DbSet<Train> Trains => Set<Train>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(ConnectionString);
    }
}
