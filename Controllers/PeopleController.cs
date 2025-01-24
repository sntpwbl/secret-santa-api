using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.DTO;
using SecretSanta.Services;

namespace SecretSanta.Controllers
{
    [ApiController]
    [Route("person")]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService _service;

        public PeopleController(IPeopleService service){
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePerson(PersonCreateDTO dto){
            if(dto.Name == null) return BadRequest("The person name is required for its creation.");
            if(dto.Password == null) return BadRequest("The person password is required for its creation.");

            return CreatedAtAction(nameof(CreatePerson), await _service.CreatePersonAsync(dto));
        }
    }
}