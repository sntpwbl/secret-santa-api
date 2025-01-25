using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SecretSanta.Context;
using SecretSanta.DTO;
using SecretSanta.Entities;
using SecretSanta.Exceptions;

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
        public async Task<PersonDTO> GetSelectedPersonAsync(int personId)
        {
            Person person = await _context.People.FirstOrDefaultAsync(p => p.Id == personId)
                ?? throw new NotFoundException($"Person not found for ID {personId}.");
                
            if(person.SelectedPersonId == null) throw new NotFoundException("Selected person not found due to no generated matches in this group.");

            Person selectedPerson = await _context.People.FirstOrDefaultAsync(p => p.Id == person.SelectedPersonId)
                ?? throw new NotFoundException($"Selected person not found for ID {person.SelectedPersonId}.");
            
            return new PersonDTO(selectedPerson.Id, selectedPerson.Name, selectedPerson.GiftDescription, selectedPerson.GroupId);
        }
        public async Task<PersonDTO> LoginAsync(PersonCreateDTO dto)
        {
            Person person = await _context.People.FirstOrDefaultAsync(p => p.Name == dto.Name)
                ?? throw new NotFoundException($"Person not found for Name {dto.Name}.");

            bool isPasswordValid = person.ValidatePassword(dto.Password);

            if(!isPasswordValid) throw new InvalidPasswordException($"Invalid password for person {dto.Name}.");
            else return new PersonDTO(person.Id, person.Name, person.GiftDescription, person.GroupId);
        }

        public async Task<PersonDTO> CreatePersonAsync(PersonCreateDTO dto)
        {
            
            Person person = new Person{
                Name = dto.Name
            };
            person.HashedPassword = person.HashPassword(dto.Password);
            
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