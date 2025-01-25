using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SecretSanta.Entities
{
    public class Person
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }
        private string _hashedPassword;

        private PasswordHasher<Person> _passwordHasher = new PasswordHasher<Person>();
        public string HashedPassword
        {
            get => _hashedPassword;
            set => _hashedPassword = value;
        }
        public string? GiftDescription { get; set; }

        public int? GroupId { get; set; }
        public int? SelectedPersonId { get; set; }
        public Group? Group { get; set; }
        public bool ValidatePassword(string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(this, _hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
        public string HashPassword(string password) => _passwordHasher.HashPassword(this, password);
    }
}