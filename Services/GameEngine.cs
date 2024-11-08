using Microsoft.EntityFrameworkCore;
using W9_assignment_template.Data;
using W9_assignment_template.Models;
using System.Linq;

namespace W9_assignment_template.Services;

public class GameEngine
{
    private readonly GameContext _context;

    public GameEngine(GameContext context)
    {
        _context = context;
    }

    public void DisplayRooms()
    {
        var rooms = _context.Rooms.Include(r => r.Characters).ToList();

        foreach (var room in rooms)
        {
            //Adding room ID to know which room is being displayed
            Console.WriteLine($"Room ID: {room.Id}, Name: {room.Name} - {room.Description}");
            foreach (var character in room.Characters)
            {
                Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
            }
        }
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters.Any())
        {
            Console.WriteLine("\nCharacters:");
            foreach (var character in characters)
            {
                Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
            }
        }
        else
        {
            Console.WriteLine("No characters available.");
        }
    }

    public void AddRoom()
    {
        Console.Write("Enter room name: ");
        var name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Room name cannot be empty.");
            return;
        }

        Console.Write("Enter room description: ");
        var description = Console.ReadLine();
        if (string.IsNullOrEmpty(description))
        {
            Console.WriteLine("Room description cannot be empty.");
            return;
        }

        var room = new Room
        {
            Name = name,
            Description = description
        };

        _context.Rooms.Add(room);
        _context.SaveChanges();

        Console.WriteLine($"Room '{name}' added to the game.");
    }

    public void AddCharacter()
    {
        Console.Write("Enter character name: ");
        var name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Character name cannot be empty.");
            return;
        }

        Console.Write("Enter character level: ");
        if (!int.TryParse(Console.ReadLine(), out var level))
        {
            Console.WriteLine("Invalid level. Please enter a valid number.");
            return;
        }

        Console.Write("Enter room ID for the character: ");
        if (!int.TryParse(Console.ReadLine(), out var roomId))
        {
            Console.WriteLine("Invalid room ID. Please enter a valid number.");
            return;
        }

        // Find the room by ID
        var room = _context.Rooms.Find(roomId);
        if (room == null)
        {
            Console.WriteLine($"Room with ID {roomId} does not exist.");
            return;
        }

        // Create a new character and add it to the room
        var character = new Character
        {
            Name = name,
            Level = level,
            RoomId = roomId,
            Room = room
        };

        _context.Characters.Add(character);
        _context.SaveChanges();

        Console.WriteLine($"Character '{name}' added to room '{room.Name}'.");
    }

    public void FindCharacter()
    {
        Console.Write("Enter character name to search: ");
        var name = Console.ReadLine();

        // Use LINQ to query the database for the character
        var character = _context.Characters
                                .Include(c => c.Room)
                                .FirstOrDefault(c => c.Name.ToLower() == name.ToLower());

        if (character != null)
        {
            Console.WriteLine($"Character found: ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room: {character.Room.Name}");
        }
        else
        {
            Console.WriteLine($"Character with name '{name}' not found.");
        }
    }

    public void UpdateCharacterLevel()
    {
        Console.Write("Enter character name to update: ");
        var name = Console.ReadLine();

        // Use LINQ to query the database for the character
        var character = _context.Characters.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());

        if (character != null)
        {
            Console.Write($"Current Level: {character.Level}. Enter new level: ");
            if (int.TryParse(Console.ReadLine(), out var newLevel))
            {
                character.Level = newLevel;
                _context.SaveChanges();
                Console.WriteLine($"Character '{character.Name}' level updated to {newLevel}.");
            }
            else
            {
                Console.WriteLine("Invalid level. Please enter a valid number.");
            }
        }
        else
        {
            Console.WriteLine($"Character with name '{name}' not found.");
        }
    }
}
