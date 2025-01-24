using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecretSanta.Context;

namespace SecretSanta.Services
{
    public interface IGroupsService
    {
        
    }

    public class GroupsService : IGroupsService
    {
        private readonly SecretSantaContext _context;

        public GroupsService(SecretSantaContext context)
        {
            _context = context;
        }
        
    }
}