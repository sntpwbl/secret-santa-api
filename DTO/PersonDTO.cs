using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.DTO
{
    /// <summary>
    /// DTO used to return a person's data.
    /// </summary>
    public class PersonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? GiftDescription { get; set; }
        public int? GroupId { get; set; }

    }
}