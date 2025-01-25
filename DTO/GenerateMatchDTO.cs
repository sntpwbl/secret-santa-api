using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.DTO
{
    public class GenerateMatchDTO
    {
        public string GenerateStatus { get; set; }
        public bool Generated { get; set; }
        public int GroupId { get; set; }
    }
}