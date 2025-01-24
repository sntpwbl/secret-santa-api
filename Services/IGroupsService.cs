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
        Task<GroupDTO> GetGroupByIdAsync(int id);
        Task<GroupDTO> CreateGroupAsync(GroupCreateDTO dto);
        Task<GroupDTO> AddPersonToGroupAsync(int personId, int groupId);
    }
}