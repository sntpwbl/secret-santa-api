using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecretSanta.Entities;

namespace SecretSanta.DTO
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsGeneratedMatches { get; set; }
        public string? Description { get; set; }
        public ICollection<Person> People { get; set; }

        public GroupDTO(int id, string name, bool isGeneratedMatches, string? description, ICollection<Person>? people)
        {
            Id = id;
            Name = name;
            IsGeneratedMatches = isGeneratedMatches;
            Description = description;
            People = people ?? new List<Person>();
        }

    }
}