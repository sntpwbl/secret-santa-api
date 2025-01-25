using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.DTO;
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
            return Ok(await _service.GetAllGroupsAsync());
        }

        [HttpGet("/name/{groupName}")]
        [SwaggerOperation(Summary = "Find created groups by name.", Description = "Returns every group which matches the name passed by user.")]
        [SwaggerResponse(200, "Successfully returned all groups.", typeof(GroupDTO))]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetGroupsByName(string groupName){
            if(groupName==null) return BadRequest("Invalid group name.");
            return Ok(await _service.GetGroupsByNameAsync(groupName));
        }

        [HttpPost("{groupId}")]
        [SwaggerOperation(Summary = "Returns a group based on ID and password.", Description = "Returns a group based on ID and password.")]
        [SwaggerResponse(200, "Successfully returned all groups.", typeof(GroupDTO))]
        [SwaggerResponse(404, "Group not found.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetGroupById(int groupId, [FromBody] string password)
        {
            return Ok(await _service.ValidateGroupPasswordAsync(groupId, password));
        }

        [HttpPost("create")]
        [SwaggerOperation(Summary = "Creates a group.", Description = "Creates a group with name, description and password.")]
        [SwaggerResponse(201, "Successfully created group.", typeof(GroupDTO))]
        [SwaggerResponse(400, "Required data missing.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> Create([FromBody] GroupCreateDTO dto){
            if(dto.Name == null) return BadRequest("The group name is required for its creation.");
            if(dto.Password == null) return BadRequest("The group password is required for its creation.");
            return CreatedAtAction(nameof(Create), await _service.CreateGroupAsync(dto));
        }
        [HttpPost("generate/{groupId}")]
        [SwaggerOperation(Summary = "Generates the matches.", Description = "Generates the matches between the group participants.")]
        [SwaggerResponse(200, "Successfully generated match.", typeof(GenerateMatchDTO))]
        [SwaggerResponse(403, "Already generated matches for the group.")]
        [SwaggerResponse(403, "Invalid number of participants.")]
        [SwaggerResponse(404, "Group not found.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GenerateMatch(int groupId){
            return Ok(await _service.GenerateMatchAsync(groupId));
        }

        [HttpPost("{personId}/{groupId}")]
        [SwaggerOperation(Summary = "Adds a person to group.", Description = "Adds a person from the group using both group and person ID.")]
        [SwaggerResponse(200, "Successfully added person to group.", typeof(GroupDTO))]
        [SwaggerResponse(404, "Group not found.")]
        [SwaggerResponse(404, "Person not found.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> AddPersonToGroup(int personId, int groupId)
        {
            if(personId <= 0) return BadRequest("Invalid value for the person ID.");
            if(groupId <= 0) return BadRequest("Invalid value for the group ID.");

            return Ok(await _service.AddPersonToGroupAsync(personId, groupId));
        }

        [HttpPatch("{personId}/{groupId}")]
        [SwaggerOperation(Summary = "Removes a person from the group.", Description = "Removes a person from the group using both group and person ID.")]
        [SwaggerResponse(200, "Successfully removed person from group.", typeof(GroupDTO))]
        [SwaggerResponse(404, "Group not found.")]
        [SwaggerResponse(404, "Person not found.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> RemovePersonFromGroup(int personId, int groupId)
        {
            return Ok(await _service.RemovePersonFromGroupAsync(personId, groupId));
        }

        [HttpPut("{groupId}")]
        [SwaggerOperation(Summary = "Updates a group.", Description = "Updates a group's name and description.")]
        [SwaggerResponse(200, "Successfully updated group.", typeof(GroupDTO))]
        [SwaggerResponse(404, "Group not found.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> UpdateGroup(int groupId, GroupUpdateDTO dto)
        {
            if(dto.Name == null) return BadRequest("The group name is required for its update.");
            return Ok(await _service.UpdateGroupAsync(groupId, dto));
        }

        [HttpDelete("{groupId}")]
        [SwaggerOperation(Summary = "Deletes a group.", Description = "Deletes a group using its ID.")]
        [SwaggerResponse(204, "Successfully deleted group.")]
        [SwaggerResponse(404, "Group not found.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            await _service.DeleteGroupAsync(groupId);
            return NoContent();
        }

    }
    
}