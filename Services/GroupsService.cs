using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SecretSanta.Context;
using SecretSanta.DTO;
using SecretSanta.Entities;
using SecretSanta.Exceptions;

namespace SecretSanta.Services
{

    public class GroupsService : IGroupsService
    {
        private readonly SecretSantaContext _context;

        public GroupsService(SecretSantaContext context)
        {
            _context = context;
        }
        public async Task<ICollection<GroupDTO>> GetAllGroupsAsync()
        {
            var groups = await _context.Groups
            .ToListAsync();

            var groupDTOs = groups.Select(g => new GroupDTO(
                g.Id,
                g.Name,
                g.IsGeneratedMatches,
                g.Description,
                []
            )).ToList();

            return groupDTOs;
        }
        public async Task<ICollection<GroupDTO>> GetGroupsByNameAsync(string name){
            var groups = await _context.Groups
                .Where(g => g.Name == name)
                .ToListAsync() ?? throw new NotFoundException($"No group find with name {name}.");

            var groupsDTO = groups.Select(g => new GroupDTO(
                g.Id,
                g.Name,
                g.IsGeneratedMatches,
                g.Description,
                []
            )).ToList();

            return groupsDTO;

        }

        public async Task<GroupDTO> CreateGroupAsync(GroupCreateDTO dto){
            
            Group group = new Group{
                Name = dto.Name,
                Description = dto.Description??null,
                People = []
            };
            group.HashedPassword = group.HashPassword(dto.Password);
            var createdGroup = await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();

            GroupDTO result = new GroupDTO(
                createdGroup.Entity.Id,
                createdGroup.Entity.Name,
                createdGroup.Entity.IsGeneratedMatches,
                createdGroup.Entity.Description,
                createdGroup.Entity.People
            );            
            return result;
        }

        public async Task<GroupDTO> AddPersonToGroupAsync(int personId, int groupId){
            Person person = await _context.People.FindAsync(personId)
                            ?? throw new NotFoundException($"Person not found for ID {personId}.");

            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                            ?? throw new NotFoundException($"Group not found for ID {groupId}.");

            person.GroupId = group.Id;
            group.People ??= new List<Person>();
            group.People.Add(person);

            await _context.SaveChangesAsync();

            GroupDTO dto = new GroupDTO(group.Id, group.Name, group.IsGeneratedMatches, group.Description, group.People);
            return dto;
        }

        public async Task<GroupDTO> RemovePersonFromGroupAsync(int personId, int groupId)
        {
            Person person = await _context.People.FindAsync(personId)
                            ?? throw new NotFoundException($"Person not found for ID {personId}.");

            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                ?? throw new NotFoundException($"Group not found for ID {groupId}.");
            
            group.People.Remove(person);
            await _context.SaveChangesAsync();

            return new GroupDTO(group.Id, group.Name, group.IsGeneratedMatches, group.Description, group.People);
        }

        public async Task<GroupDTO> UpdateGroupAsync(int groupId, GroupUpdateDTO dto)
        {
            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                ?? throw new NotFoundException($"Group not found for ID {groupId}.");
            
            group.Name = dto.Name;
            group.Description = dto.Description ?? group.Description;

            await _context.SaveChangesAsync();
            return new GroupDTO(group.Id, group.Name, group.IsGeneratedMatches, group.Description, group.People);
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            var group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId) 
                ?? throw new NotFoundException($"Group not found for ID {groupId}.");

            foreach(var person in group.People){
                person.GroupId = null;
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        public async Task<GroupDTO> ValidateGroupPasswordAsync(int groupId, string password)
        {
            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                ?? throw new NotFoundException($"Group not found for ID {groupId}.");
            bool isPasswordValid = group.ValidatePassword(password);
            if (!isPasswordValid) throw new InvalidPasswordException($"Invalid password for group with ID {groupId}");
            else return new GroupDTO(group.Id, group.Name, group.IsGeneratedMatches, group.Description, group.People);
        }

        public async Task<GenerateMatchDTO> GenerateMatchAsync(int groupId)
        {
            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                ?? throw new NotFoundException($"Group not found for ID {groupId}.");

            if(group.IsGeneratedMatches) throw new AlreadyGeneratedMatchException("You cannot generate matches for a group multiple times.");

            if(group.People.Count() < 2 || group.People.Count() % 2 != 0) 
                throw new InvalidNumberOfPeopleException("Your group needs to have two or more participants to generate matches. The total number of participants must be pair.");

            
            var peopleList = group.People.ToList();
            var giverList = new List<Person>(peopleList); 
            var receiverList = new List<Person>(peopleList);

            var random = new Random();

            receiverList = receiverList.OrderBy(_ => random.Next()).ToList();

            foreach (var giver in giverList)
            {
                while (true)
                {
                    int index = random.Next(receiverList.Count);
                    var potentialReceiver = receiverList[index];

                    if (potentialReceiver.Id != giver.Id && giver.SelectedPersonId != potentialReceiver.Id)
                    {
                        giver.SelectedPersonId = potentialReceiver.Id;

                        receiverList.RemoveAt(index);
                        break;
                    }
                }
            }
            group.IsGeneratedMatches = true;
            await _context.SaveChangesAsync();

            return new GenerateMatchDTO{GenerateStatus = "Success", Generated = true, GroupId = group.Id};
        }
    }
}