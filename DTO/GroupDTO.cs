using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecretSanta.Entities;

namespace SecretSanta.DTO
{
    /// <summary>
    /// DTO used for returning group data.
    /// </summary>
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsGeneratedMatches { get; set; }
        public string? Description { get; set; }
        // to-do: cast collection to persondto type
        public ICollection<Person> People { get; set; } = new List<Person>();


    }
}