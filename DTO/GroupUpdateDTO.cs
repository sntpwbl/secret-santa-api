using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.DTO
{
    public class GroupUpdateDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}