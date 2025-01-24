using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
        public Group group{ get; set; }
    }
}