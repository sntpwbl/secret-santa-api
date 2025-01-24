using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.DTO;
using SecretSanta.Exceptions;
using SecretSanta.Services;

namespace SecretSanta.Controllers
{
    [ApiController]
    [Route("group")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService _service;

        public GroupsController(IGroupsService service){
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] GroupDTO dto){
            if(dto == null)
            {
                return BadRequest("The group name is required for its creation.");
            }
            return CreatedAtAction(nameof(Create), await _service.CreateGroupAsync(dto));
        }

        [HttpPost("{personId}/{groupId}")]
        public async Task<IActionResult> Update(int personId, int groupId){
            if(personId <= 0) return BadRequest("Invalid value for the person ID.");
            if(groupId <= 0) return BadRequest("Invalid value for the person ID.");

            try
            {
                var result = await _service.AddPersonToGroupAsync(personId, groupId);
                return Ok(new { result.Id, result.Name,
                participants = result.People.Select(p => new
                {
                    p.Id,
                    p.Name
                }).ToList() });
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}