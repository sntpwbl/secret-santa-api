using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.DTO
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? GiftDescription { get; set; }
        public int? GroupId { get; set; }

        public PersonDTO(int id, string name, string? giftDescription, int? groupId)
        {
            Id = id;
            Name = name;
            GiftDescription = giftDescription;
            GroupId = groupId;
        }
    }
}