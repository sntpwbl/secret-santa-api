using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.DTO
{   
    /// <summary>
    /// DTO used for sinalizing a match was created.
    /// </summary>
    public class GenerateMatchDTO
    {
        public string GenerateStatus { get; set; } = "Success";
        public bool Generated { get; set; }
        public int GroupId { get; set; }
    }
}