using Microsoft.EntityFrameworkCore;
using W9_assignment_template.Models;

namespace W9_assignment_template.Data;

public class GameContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Character> Characters { get; set; }

    // Override OnConfiguring to set up the connection string for the SQL Server
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Replace with your actual connection string from https://www.connectionstrings.com/sql-server/
        optionsBuilder.UseSqlServer("Server=bitsql.wctc.edu;Database=W9_Assignment;User Id=jdelfosse;Password=000420533;");
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