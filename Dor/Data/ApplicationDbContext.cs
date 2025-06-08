using Dor.Models;
using Microsoft.EntityFrameworkCore;

namespace Dor.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    public DbSet<Building> Buildings { get; set; } = null!;
    public DbSet<Complex> Complexes { get; set; } = null!;
    public DbSet<Customers> Customers { get; set; } = null!;
    public DbSet<Property> Properties { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure table names
        modelBuilder.Entity<Building>().ToTable("Buildings");
        modelBuilder.Entity<Complex>().ToTable("Complexes");
        modelBuilder.Entity<Customers>().ToTable("Customers");
        modelBuilder.Entity<Property>().ToTable("Properties");
        modelBuilder.Entity<User>().ToTable("Users");


        // Seed data for the User table
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Name = "admin",
            Password = "admin", // Plain text password
            Role = "Admin",
        });
    }
}