using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Context;

namespace SecretSanta.Controllers
{
    [ApiController]
    [Route("person")]
    public class PeopleController : ControllerBase
    {
        private readonly SecretSantaContext _context;

        public PeopleController(SecretSantaContext context)
        {
            _context = context;
        }
    }
}