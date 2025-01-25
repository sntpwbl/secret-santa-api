using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public GroupsService(IMapper mapper, SecretSantaContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GroupDTO>> GetAllGroupsAsync()
        {
            var groups = await _context.Groups.ToListAsync();
            
            return groups.Select(_mapper.Map<GroupDTO>);
        }
        public async Task<IEnumerable<GroupDTO>> GetGroupsByNameAsync(string name){
            var groups = await _context.Groups
                .Where(g => g.Name == name)
                .ToListAsync() ?? throw new NotFoundException($"No group find with name {name}.");

            return groups.Select(_mapper.Map<GroupDTO>);
        }

        public async Task<GroupDTO> CreateGroupAsync(GroupCreateDTO dto){
            
            Group group = _mapper.Map<Group>(dto);
            group.HashedPassword = group.HashPassword(group.HashedPassword);

            var createdGroup = await _context.Groups.AddAsync(group);

            await _context.SaveChangesAsync();

            return _mapper.Map<GroupDTO>(createdGroup.Entity);
        }

        public async Task<GroupDTO> AddPersonToGroupAsync(int personId, int groupId){
            Person person = await _context.People.FindAsync(personId)
                            ?? throw new NotFoundException($"Person not found for ID {personId}.");

            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                            ?? throw new NotFoundException($"Group not found for ID {groupId}.");

            person.GroupId = group.Id;
            person.SelectedPersonId = null;
            group.People ??= new List<Person>();
            group.People.Add(person);

            await _context.SaveChangesAsync();

            return _mapper.Map<GroupDTO>(group);
        }

        public async Task<GroupDTO> RemovePersonFromGroupAsync(int personId, int groupId)
        {
            Person person = await _context.People.FindAsync(personId)
                            ?? throw new NotFoundException($"Person not found for ID {personId}.");

            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                ?? throw new NotFoundException($"Group not found for ID {groupId}.");
            
            group.People.Remove(person);

            if(group.IsGeneratedMatches){
                person.SelectedPersonId = null;
                group.People.Select(p=> p.SelectedPersonId = null);
                group.IsGeneratedMatches = false;
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<GroupDTO>(group);

        }

        public async Task<GroupDTO> UpdateGroupAsync(int groupId, GroupUpdateDTO dto)
        {
            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                ?? throw new NotFoundException($"Group not found for ID {groupId}.");
            
            group.Name = dto.Name;
            group.Description = dto.Description ?? group.Description;

            await _context.SaveChangesAsync();
            return _mapper.Map<GroupDTO>(group);

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
            else return _mapper.Map<GroupDTO>(group);
            
        }

        public async Task<GenerateMatchDTO> GenerateMatchAsync(int groupId)
        {
            Group group = await _context.Groups.Include(g => g.People).FirstOrDefaultAsync(g => g.Id == groupId)
                ?? throw new NotFoundException($"Group not found for ID {groupId}.");

            if(group.IsGeneratedMatches) throw new AlreadyGeneratedMatchException("You cannot generate matches for a group multiple times.");

            if(group.People.Count() <= 2) 
                throw new InvalidNumberOfPeopleException("Your group needs to have three or more participants to generate matches.");

            
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