using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using W9temp.Data;
using W9temp.Models;

namespace W9temp.Services;

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
        if (rooms == null)
        {
            Console.Write("No rooms found, 'enter' to continue.");
            Console.ReadLine();
        }
        else
        {
            Console.Clear();
            Console.WriteLine($"Current list of rooms include: \n");
            foreach (var room in rooms)
            {
                if (room.Name.Length != 0)
                {
                    Console.WriteLine($"Room: {room.Name} - {room.Description}");
                    if (room.Characters.Any())
                    {
                        foreach (var character in room.Characters)
                        {
                            Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("    Empty Room");
                    }

                }
            }
            Console.Write("\nPress 'enter' to continue.");
            Console.ReadLine();
        }
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters == null)
        {
            Console.Write("No characters found, 'enter' to continue.");
            Console.ReadLine();
        }
        else
        {
            Console.Clear();
            Console.WriteLine($"Current list of characters include: \n");

            if (characters.Any())
            {
                Console.WriteLine("\nCharacters:\n");
                foreach (var character in characters)
                {
                    Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
                }
                Console.Write("\nPress 'enter' to continue.");
                Console.ReadLine();
            }
            else
            {
                Console.Write("No characters found, 'enter' to continue.");
                Console.ReadLine(); 
            }
        }
    }
    public void AddRoom()
    {
        Console.Write("Enter room name: ");
        var name = Console.ReadLine();

        Console.Write("Enter room description: ");
        var description = Console.ReadLine();

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
        // TODO Add character to the room
        // Find the room by ID
        // If the room doesn't exist, return
        // Otherwise, create a new character and add it to the room
        // Save the changes to the database

        bool continueInput = true;
        while (continueInput == true)
        {
            Console.Clear();
            Console.Write("Enter character name or 'exit' to return to the menu: ");
            var name = Console.ReadLine();

            if (name == null)
            {
                Console.Clear();
                Console.Write("Invalid entry. Please try again, 'enter' to continue.");
                Console.ReadLine();
            }
            else
            {
                if (name == "exit")
                {
                    continueInput = false;
                }
                else
                {
                    Console.Write("Enter character level or '-1' to return to the menu: ");
                    int level;
                    bool lvl = int.TryParse(Console.ReadLine(), out level);
                    if (!lvl)
                    {
                       // Harsh but it works
                       Console.Write("Invalid input. Please try again, 'enter' to continue.");
                       Console.ReadLine();
                    }
                    else
                    {
                       if (level == -1)
                        {
                            continueInput = false;
                        }
                        else
                        {
                            int room_cnt = _context.Rooms.Count() - 1;
                            Console.WriteLine("\nAvailable rooms are as follows: \n");
                            foreach (var room in _context.Rooms)
                            {
                                if (room.Name.Length > 0)
                                {
                                    Console.WriteLine($"Room: {room.Name} Id: {room.Id}");
                                }
                            }
                 
                            Console.Write("\nEnter room ID for the character or '-1' to return to the menu: ");
                            //var roomId = int.Parse(Console.ReadLine());
                            int rmId;
                            bool rm = int.TryParse(Console.ReadLine(), out rmId);
                            if (!rm)
                            {
                               // Harsh but it works
                               Console.Write("Invalid input. Please try again, 'enter' to continue.");
                               Console.ReadLine();
                            }
                            else
                            {
                                if (rmId == -1)
                                {
                                    continueInput = false;
                                }
                                else
                                {
                                    var room2 = _context.Rooms.Where(r => r.Id == rmId).FirstOrDefault();
                                    if (room2 == null)
                                    {
                                        // Harsh but it works
                                        Console.Write("Invalid input. Please try again, 'enter' to continue.");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        // Add character here
                                        var character = new Character
                                        {
                                            Name = name,
                                            Level = level,
                                            RoomId = rmId
                                        };
                                        _context.Characters.Add(character);
                                        _context.SaveChanges();

                                        Console.Write($"\nCharacter '{name}' added to the game, 'enter' to continue.");
                                        Console.ReadLine();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public void FindCharacter()
    {
        bool continueInput = true;
        while (continueInput == true)
        {
            Console.Clear();
            Console.Write("Enter character name to search or 'exit' to return to the menu: ");
            var name = Console.ReadLine();

            if (name != null)
            {
                // TODO Find the character by name
                // Use LINQ to query the database for the character
                // If the character exists, display the character's information
                // Otherwise, display a message indicating the character was not found
                if (name.ToLower() == "exit")
                {
                    continueInput = false;
                }
                else
                {
                    var pc = _context.Characters.Where(c => c.Name == name).FirstOrDefault();

                    if (pc == null)
                    {
                        Console.Clear();
                        Console.Write($"Character {name} not found, 'enter' to continue.");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write($"Character {pc.Name}, level {pc.Level} was found, 'enter' to continue.");
                        Console.ReadLine();
                    }
                }
            }
            else
            {
                Console.Write("Please enter a valid name or 'exit' to return to the menu, 'enter' to continue.");
                Console.ReadLine();
            }
        }
    }
    public void UpdateCharacterLevel()
    {
        bool continueInput = true;

        while (continueInput == true)
        {
            Console.Clear();
            Console.Write("Here are the characters to chose from: \n\n");
            foreach (var character in _context.Characters)
            {
                if (character.Name.Length > 0)
                {
                    Console.WriteLine($"Character Name: {character.Name} Id: {character.Id} Current Level: {character.Level}");
                }
            }
            if (_context.Characters != null)
            {
                Console.WriteLine("\nSelect an id from the list above, '-1' to exit.");
                int characterId;
                bool id = int.TryParse(Console.ReadLine(), out characterId);
                if (!id)
                {
                    // Harsh but it works
                    Console.Write("Invalid input. Please try again, 'enter' to continue.");
                    Console.ReadLine();
                }
                else
                {
                    if (characterId == -1)
                    {
                        continueInput = false;
                    }
                    else
                    {
                        var pc = _context.Characters.Where(c => c.Id == characterId).FirstOrDefault();
                        if (pc == null)
                        {
                            // Harsh but it works
                            Console.Write("Invalid input. Please try again, 'enter' to continue.");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.Write($"Enter a postive number to increase the character {pc.Name}'s level or a ");
                            Console.Write($"neative number to decrease {pc.Name}'s level? ");
                            int val;
                            bool v = int.TryParse(Console.ReadLine(), out val);
                            if (!v)
                            {
                                Console.Write("Invalid input. Please try again, 'enter' to continue.");
                                Console.ReadLine();
                            }
                            else
                            {
                                if ((pc.Level + val) <= 0)
                                {
                                    val = 0;  // This will force the value to become just zero
                                }
                                else
                                {
                                    val = pc.Level + val;
                                }
                                Console.Write($"Update {pc.Name}'s level from {pc.Level} to {val} Yes or No? (Y or N) ");
                                var input = Console.ReadLine();
                                if (input.ToLower()[0] == 'y')
                                {
                                    pc.Level = val;
                                    _context.SaveChanges();
                                    Console.Write($"\nCharacter {pc.Name}'s level was updated, 'enter' to continue.");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    Console.Write($"\nCharacter {pc.Name} was NOT updated, 'enter' to continue.");
                                    Console.ReadLine();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}