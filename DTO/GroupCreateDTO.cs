using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.DTO
{
    public class GroupCreateDTO
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public string? Description { get; set; }
    }
}