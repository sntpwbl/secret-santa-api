using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SecretSanta.Context;
using SecretSanta.DTO;
using SecretSanta.Entities;

namespace SecretSanta.Services
{
    public interface IPeopleService
    {
        Task<Person> CreatePersonAsync(PersonDTO dto);
    }
    public class PeopleService : IPeopleService
    {
        private readonly SecretSantaContext _context;
        public PeopleService(SecretSantaContext context){
            _context = context;
        }

        public async Task<Person> CreatePersonAsync(PersonDTO dto)
        {
            Person person = new Person{
                Name = dto.Name
            };
            EntityEntry<Person> createdPerson = await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
            
            person = new Person{
                Id = createdPerson.Entity.Id,
                Name = createdPerson.Entity.Name,
                GroupId = createdPerson.Entity.GroupId,
            };

            return person;

        }
        
    }
}