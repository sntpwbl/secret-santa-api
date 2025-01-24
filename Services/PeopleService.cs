using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SecretSanta.Context;
using SecretSanta.DTO;
using SecretSanta.Entities;

namespace SecretSanta.Services
{
    
    public class PeopleService : IPeopleService
    {
        private readonly SecretSantaContext _context;
        public PeopleService(SecretSantaContext context){
            _context = context;
        }
        public async Task<ICollection<PersonDTO>> GetAllPeopleAsync()
        {
            var people = await _context.People.ToListAsync();

            var peopleDTO = people.Select(p => new PersonDTO(
                p.Id,
                p.Name,
                p.GiftDescription,
                p.GroupId
            )).ToList();

            return peopleDTO;
        }

        public async Task<PersonDTO> CreatePersonAsync(PersonCreateDTO dto)
        {
            Person person = new Person{
                Name = dto.Name,
                HashedPassword = dto.Password
            };
            EntityEntry<Person> createdPerson = await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
            
            PersonDTO result = new PersonDTO(
                createdPerson.Entity.Id,
                createdPerson.Entity.Name,
                createdPerson.Entity.GiftDescription,
                createdPerson.Entity.GroupId
            );

            return result;

        }

    }
}