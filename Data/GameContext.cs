using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using W9temp.Models;

namespace W9temp.Data;

public class GameContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Character> Characters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

    }

    // Seed Method
    public void Seed()
    {
        if (!Rooms.Any())
        {
            var room1 = new Room { Name = "Entrance Hall", Description = "The main entry." };
            var room2 = new Room { Name = "Treasure Room", Description = "A room filled with treasures." };

            var character1 = new Character { Name = "Knight", Level = 1, Room = room1 };
            var character2 = new Character { Name = "Wizard", Level = 2, Room = room2 };

            Rooms.AddRange(room1, room2);
            Characters.AddRange(character1, character2);

            SaveChanges();
        }
    }
}