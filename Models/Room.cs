﻿namespace W9temp.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    // Navigation property to Characters
    public virtual ICollection<Character> Characters { get; set; }
}