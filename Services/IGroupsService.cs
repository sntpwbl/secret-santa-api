using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecretSanta.DTO;

namespace SecretSanta.Services
{
    public interface IGroupsService
    {
        Task<ICollection<GroupDTO>> GetAllGroupsAsync();
        Task<ICollection<GroupDTO>> GetGroupsByNameAsync(string name);
        Task<GroupDTO> CreateGroupAsync(GroupCreateDTO dto);
        Task<GroupDTO> AddPersonToGroupAsync(int personId, int groupId);
        Task<GroupDTO> ValidateGroupPasswordAsync(int groupId, string password);
        Task<GenerateMatchDTO> GenerateMatchAsync(int groupId);
        Task<GroupDTO> RemovePersonFromGroupAsync(int personId, int groupId);
        Task<GroupDTO> UpdateGroupAsync(int groupId, GroupUpdateDTO dto);
        Task DeleteGroupAsync(int groupId);
    }
}