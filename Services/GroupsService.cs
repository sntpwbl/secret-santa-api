using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using SecretSanta.Context;
using SecretSanta.Entities;

namespace SecretSanta.Services
{
    public interface IGroupsService
    {
        Task<Group> CreateGroupAsync(string groupName);
    }

    public class GroupsService : IGroupsService
    {
        private readonly SecretSantaContext _context;

        public GroupsService(SecretSantaContext context)
        {
            _context = context;
        }
        public async Task<Group> CreateGroupAsync(string groupName){
            
            Group group = new Group{
                Name = groupName,
                People = new Collection<Person>()
            };

            var createdGroup = await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            
            group.Id = createdGroup.Entity.Id;
            group.Name = createdGroup.Entity.Name;
            group.People = createdGroup.Entity.People;

            return group;
        }
    }
}