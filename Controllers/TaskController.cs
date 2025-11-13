using WorkNest.Enums;
using WorkNest.Models.Dto;
using WorkNest.Services;
using WorkNest.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using WorkNest.Models.Dto;
using WorkNest.Services;

namespace WorkNest.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [Authorize(Roles = ROLE.ADMIN)]
        public async Task<ActionResult<TaskDTO>> CreateTask([FromBody] CreateTaskDTO dto)
        {
            try
            {
                var task = await _taskService.CreateTask(dto);
                return Ok(task);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode((int)ex.StatusCode, new HttpMessage(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = ROLE.ADMIN)]
        public async Task<ActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskService.DeleteTask(id);
                return NoContent();
            }
            catch (HttpResponseError ex)
            {
                return StatusCode((int)ex.StatusCode, new HttpMessage(ex.Message));
            }
        }

        [HttpPatch("{id}/move")]
        public async Task<ActionResult<TaskDTO>> MoveTask(int id, [FromBody] MoveTaskDTO dto)
        {
            try
            {
                var task = await _taskService.MoveTask(id, dto, User);
                return Ok(task);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode((int)ex.StatusCode, new HttpMessage(ex.Message));
            }
        }
    }
}