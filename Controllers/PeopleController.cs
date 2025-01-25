using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.DTO;
using SecretSanta.Exceptions;
using SecretSanta.Services;
using Swashbuckle.AspNetCore.Annotations;

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
        [HttpGet]
        [SwaggerOperation(Summary = "Find all signed up people.", Description = "Returns every person in the database.")]
        [SwaggerResponse(200, "Successfully returned all people.", typeof(PersonDTO))]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetAllPeople(){
            return Ok(await _service.GetAllPeopleAsync());
        }

        [HttpGet("{personId}/selected")]
        [SwaggerOperation(Summary = "Returns a match based on person ID.", Description = "Returns who the person is matched with.")]
        [SwaggerResponse(200, "Successfully logged in.", typeof(PersonDTO))]
        [SwaggerResponse(400, "Invalid person ID.")]
        [SwaggerResponse(404, "Not found person.")]
        [SwaggerResponse(404, "Not found person due to lack of match generation.")]
        [SwaggerResponse(404, "Not found selected person.")]
        public async Task<IActionResult> GetSelectedPeople(int personId){
            return Ok(await _service.GetSelectedPersonAsync(personId));
        }
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Completes login operation.", Description = "Authenticates a person using its name and password.")]
        [SwaggerResponse(200, "Successfully logged in.", typeof(PersonDTO))]
        [SwaggerResponse(400, "Invalid data. Name or password are missing.")]
        [SwaggerResponse(401, "Invalid password.")]
        [SwaggerResponse(404, "Not found person.")]
        public async Task<IActionResult> Login(PersonCreateDTO dto){
            try
            {
                if(dto.Name == null) return BadRequest("The person name is required for login.");
                if(dto.Password == null) return BadRequest("The person password is required for login.");
                return Ok(await _service.LoginAsync(dto));
                
            }
            catch(NotFoundException ex){
                return NotFound(ex.Message);
            }
            catch(InvalidPasswordException ex){
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("create")]
        [SwaggerOperation(Summary = "Creates a person.", Description = "Uses a name and a password to create a person.")]
        [SwaggerResponse(200, "Successfully created.", typeof(PersonDTO))]
        [SwaggerResponse(400, "Invalid data. Name or password are missing.")]
        public async Task<IActionResult> CreatePerson(PersonCreateDTO dto){
            if(dto.Name == null) return BadRequest("The person name is required for its creation.");
            if(dto.Password == null) return BadRequest("The person password is required for its creation.");

            return CreatedAtAction(nameof(CreatePerson), await _service.CreatePersonAsync(dto));
        }
    }
}