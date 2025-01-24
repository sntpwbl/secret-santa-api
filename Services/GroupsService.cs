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
    public interface IGroupsService
    {
        Task<Group> CreateGroupAsync(GroupDTO dto);
        Task<Group> AddPersonToGroupAsync(int personId, int groupId);
    }

    public class GroupsService : IGroupsService
    {
        private readonly SecretSantaContext _context;

        public GroupsService(SecretSantaContext context)
        {
            _context = context;
        }
        public async Task<Group> CreateGroupAsync(GroupDTO dto){
            
            Group group = new Group{
                Name = dto.Name,
                People = new Collection<Person>()
            };

            var createdGroup = await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            
            group.Id = createdGroup.Entity.Id;
            group.Name = createdGroup.Entity.Name;
            group.People = createdGroup.Entity.People;

            return group;
        }

        public async Task<Group> AddPersonToGroupAsync(int personId, int groupId){
            Person person = await _context.People.FindAsync(personId)
                            ?? throw new NotFoundException($"Person not found for ID {personId}.");

            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                            ?? throw new NotFoundException($"Group not found for ID {groupId}.");

            person.GroupId = group.Id;
            group.People ??= new List<Person>();
            group.People.Add(person);

            await _context.SaveChangesAsync();
            return group;
        }
    }
}