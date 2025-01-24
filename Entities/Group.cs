using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsGeneratedMatches { get; set; } = false;
        public required ICollection<Person> People { get; set; }
    }
}