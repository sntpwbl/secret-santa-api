using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromBody] string groupName){
            if(groupName == null)
            {
                return BadRequest("The group name is required for its creation.");
            }
            return CreatedAtAction(nameof(Create), await _service.CreateGroupAsync(groupName));
        }
    }
}