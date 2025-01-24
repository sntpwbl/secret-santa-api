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
        Task<GroupDTO> GetGroupByIdAsync(int groupId);
        Task<GroupDTO> CreateGroupAsync(GroupCreateDTO dto);
        Task<GroupDTO> AddPersonToGroupAsync(int personId, int groupId);
        Task<GroupDTO> RemovePersonFromGroupAsync(int personId, int groupId);
        Task<GroupDTO> UpdateGroupAsync(int groupId, GroupUpdateDTO dto);
        Task DeleteGroupAsync(int groupId);
    }
}