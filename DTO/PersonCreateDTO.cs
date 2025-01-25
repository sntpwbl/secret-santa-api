using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.DTO
{
    /// <summary>
    /// DTO used for people's authentication.
    /// </summary>
    public class PersonCreateDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Password { get; set; }

    }
}