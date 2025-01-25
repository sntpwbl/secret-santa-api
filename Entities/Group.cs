using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SecretSanta.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsGeneratedMatches { get; set; } = false;
        public string? Description { get; set; }

        private string _hashedPassword;

        private PasswordHasher<Group> _passwordHasher = new PasswordHasher<Group>();
        public string HashedPassword
        {
            get => _hashedPassword;
            set => _hashedPassword = value;
        }
        public required ICollection<Person> People { get; set; }

        public bool ValidatePassword(string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(this, _hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
        public string HashPassword(string password) => _passwordHasher.HashPassword(this, password);

    }
}