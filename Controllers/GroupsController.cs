using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Context;

namespace SecretSanta.Controllers
{
    [ApiController]
    [Route("group")]
    public class GroupsController : ControllerBase
    {
        private readonly SecretSantaContext _context;

        public GroupsController(SecretSantaContext context)
        {
            _context = context;
        }
    }
}