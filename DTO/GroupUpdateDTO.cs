using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.DTO
{
    /// <summary>
    /// DTO used for updating groups.
    /// </summary>
    public class GroupUpdateDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}