using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecretSanta.Context;
using SecretSanta.Entities;

namespace SecretSanta.Services
{
    public interface IPeopleService
    {

    }
    public class PeopleService : IPeopleService
    {
        private readonly SecretSantaContext _context;
        public PeopleService(SecretSantaContext context){
            _context = context;
        }
    }
}