using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SecretSanta.DTO;
using SecretSanta.Exceptions;
using SecretSanta.Services;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpGet]
        [SwaggerOperation(Summary = "Find all created groups.", Description = "Returns every group in the database.")]
        [SwaggerResponse(200, "Successfully returned all groups.", typeof(GroupDTO))]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetAllGroups(){
            var groups = await _service.GetAllGroupsAsync();
            return Ok(groups);
        }

        [HttpGet("/name/{groupName}")]
        [SwaggerOperation(Summary = "Find created groups by name.", Description = "Returns every group which matches the name passed by user.")]
        [SwaggerResponse(200, "Successfully returned all groups.", typeof(GroupDTO))]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetGroupsByName(string groupName){
            if(groupName==null) return BadRequest("Invalid group name.");
            try
            {
                var groups = await _service.GetGroupsByNameAsync(groupName);
                return Ok(groups);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost("{groupId}")]
        public async Task<IActionResult> GetGroupById(int groupId, [FromBody] string password)
        {
            var result = await _service.ValidateGroupPasswordAsync(groupId, password);
            return Ok(new { result.Id, result.Name, result.IsGeneratedMatches, result.Description,
                people = result.People.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.GiftDescription,
                    p.GroupId
                }).ToList()});
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] GroupCreateDTO dto){
            if(dto.Name == null) return BadRequest("The group name is required for its creation.");
            if(dto.Password == null) return BadRequest("The group password is required for its creation.");
            return CreatedAtAction(nameof(Create), await _service.CreateGroupAsync(dto));
        }
        [HttpPost("generate/{groupId}")]
        public async Task<IActionResult> GenerateMatch(int groupId){
            return Ok(await _service.GenerateMatchAsync(groupId));
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
        [HttpPatch("{personId}/{groupId}")]
        public async Task<IActionResult> RemovePersonFromGroup(int personId, int groupId)
        {
            var result = await _service.RemovePersonFromGroupAsync(personId, groupId);
            return Ok(new { result.Id, result.Name, result.IsGeneratedMatches, result.Description,
            people = result.People.Select(p => new
            {
                p.Id,
                p.Name,
                p.GiftDescription,
                p.GroupId
            }).ToList()});
        }

        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateGroup(int groupId, GroupUpdateDTO dto){
            if(dto.Name == null) return BadRequest("The group name is required for its update.");
            var result = await _service.UpdateGroupAsync(groupId, dto);
            return Ok(new { result.Id, result.Name, result.IsGeneratedMatches, result.Description,
                people = result.People.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.GiftDescription,
                    p.GroupId
                }).ToList()});
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId){
            await _service.DeleteGroupAsync(groupId);
            return NoContent();
        }

    }
    
}