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
        public async Task<IActionResult> Create([FromBody] GroupCreateDTO dto){
            if(dto.Name == null) return BadRequest("The group name is required for its creation.");
            if(dto.Password == null) return BadRequest("The group password is required for its creation.");
            return CreatedAtAction(nameof(Create), await _service.CreateGroupAsync(dto));
        }

        [HttpPost("{personId}/{groupId}")]
        public async Task<IActionResult> Update(int personId, int groupId){
            if(personId <= 0) return BadRequest("Invalid value for the person ID.");
            if(groupId <= 0) return BadRequest("Invalid value for the group ID.");

            try
            {
                var result = await _service.AddPersonToGroupAsync(personId, groupId);
                return Ok(new { result.Id, result.Name, result.IsGeneratedMatches, result.Description,
                people = result.People.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.GiftDescription,
                    p.GroupId
                }).ToList()});
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

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetGroupById(int groupId)
        {
            var result = await _service.GetGroupByIdAsync(groupId);
            return Ok(new { result.Id, result.Name, result.IsGeneratedMatches, result.Description,
                people = result.People.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.GiftDescription,
                    p.GroupId
                }).ToList()});
        }
    }
    
}