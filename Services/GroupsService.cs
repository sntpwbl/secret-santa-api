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
        public async Task<GroupDTO> CreateGroupAsync(GroupCreateDTO dto){
            
            Group group = new Group{
                Name = dto.Name,
                HashedPassword = dto.Password,
                Description = dto.Description??null,
                People = []
            };

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

        public async Task<ICollection<GroupDTO>> GetAllGroupsAsync()
        {
            var groups = await _context.Groups
            .ToListAsync();

            var groupDTOs = groups.Select(g => new GroupDTO(
                g.Id,
                g.Name,
                g.IsGeneratedMatches,
                g.Description,
                g.People
            )).ToList();

            return groupDTOs;
        }

        public async Task<GroupDTO> GetGroupByIdAsync(int groupId)
        {
            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                            ?? throw new NotFoundException($"Group not found for ID {groupId}.");
            
            GroupDTO dto = new GroupDTO(group.Id, group.Name, group.IsGeneratedMatches, group.Description, group.People);
            return dto;
        }
    }
}