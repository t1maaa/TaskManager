using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using TaskManager.DAL.Interfaces;
using TaskManager.Db;
using TaskManager.ViewModel;
using TaskManager.ViewModel.Tasks;

namespace TaskManagerAPI.Controllers //
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public TasksController(ApplicationDbContext context)
        {
            _db = context;
        }

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
        public async Task<IActionResult> CreateTask([FromBody]CreateTaskRequest request, [FromServices]ICreateTaskCommand command, Guid id = default) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //request.ParentId = id;
            TaskResponse response = await command.ExecuteAsync(request);

            /*if (response == null)
            {
                return NotFound();
            }*/
            return CreatedAtAction(nameof(CreateTask), response.Id, response);
            // _db.Tasks.Add(task);
            //  await _db.SaveChangesAsync();
            //  return task;
        }

        [HttpDelete("{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult> DeleteTask(Guid id, [FromBody]DeleteTaskRequest request,
            [FromServices] IDeleteTaskCommand command) //TODO: Guid сделать не Nullable и не парить мозг? Реализацию CreateTask чекай тудушку. и JWT пили
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

            if(result.TotalItemsCount > 0)
            {
                return Ok(result);
            }
            return NotFound(result);


        }

        #region GetTasksList
        // GET: api/Tasks
        [HttpGet]
        [Consumes("application/json")]
        public async Task<IActionResult> GetTasksList([FromServices] ITaskListQuery query) 
        {
            ListResponse<TaskResponse> response = await query.RunAsync();
            return Ok(value: response);
        }
        #endregion

        #region GetTasksById
        // GET: api/Tasks/5
        [HttpGet("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetTask(Guid id, [FromServices] ITaskListQuery query)
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


        // PUT: api/Tasks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(Guid id, TaskManager.Db.Models.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _db.Entry(task).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tasks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<TaskManager.Db.Models.Task>> PostTask(TaskManager.Db.Models.Task task)
        //{
        //    return Ok();

        //    /*
        //    _context.Tasks.Add(task);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTask", new { id = task.Id }, task);*/


        //}

        /*

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskManager.Db.Models.Task>> DeleteTask(int id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();

            return task;
        }
        */
        private bool TaskExists(Guid id)
        {
            return _db.Tasks.Any(e => e.Id == id);
        }
    }
}
