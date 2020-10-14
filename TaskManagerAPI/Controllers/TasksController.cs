using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Tasks.Interfaces;
using TaskManager.Db;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        #region CreateTask
        /// <summary>
        /// Create a new task or subtask
        /// </summary>
        /// <remarks>
        /// some remarks
        /// </remarks>
        /// <param name="id"></param>
        /// <returns> new task info </returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request, check payload</response>
        [HttpPost("new", Name = "CreateNewTask")]
        [HttpPost("{id}/new", Name = "CreateNewSubtaskForTaskWithID")]
        [ProducesResponseType(typeof(TaskResponse), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request, [FromServices] ICreateTaskCommand command, Guid id = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TaskResponse response = await command.ExecuteAsync(request);

            return CreatedAtAction(nameof(CreateTask), response.Id, response);
        }
        #endregion
        
        #region DeleteTask
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult> DeleteTask(Guid id, [FromBody] DeleteTaskRequest request, [FromServices] IDeleteTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)
            {
                return BadRequest("URL ID and body's ID should be the same");
            }

            if (!request.Confirmed)
            {
                return BadRequest("Confirmation is required");
            }
            var result = await command.ExecuteAsync(request);

            if (result.TotalItemsCount > 0)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        #endregion

        #region GetTasksList
        // GET: api/Tasks
        [HttpGet]
        [Consumes("application/json")]
        public async Task<IActionResult> GetTasksList([FromServices] IGetListQuery query)
        {
            var res = await _taskService.Get();
            //ListResponse<TaskResponse> response = await query.RunAsync();
            return Ok(value: res);
        }
        #endregion

        #region GetTasksById
        // GET: api/Tasks/5
        [HttpGet("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetTask(Guid id, [FromServices] IGetListQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ListResponse<TaskResponse> response = await query.RunAsync(id);

             if (response.Items.Count > 0)
                return Ok(response);
             else
             {
                 return NotFound(response);
             }
        }
        #endregion

        #region UpdateTask
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request, [FromServices] IUpdateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)
            {
                return BadRequest("URL ID and body's ID should be the same");
            }

            var result = await command.ExecuteAsync(request);

            if (result != null)
                return Ok(result);

            return NotFound();
        }
        #endregion

    }
}
