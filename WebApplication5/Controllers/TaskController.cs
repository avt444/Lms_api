using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication5.Data;
using WebApplication5.Models.Entities;

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<TaskEntity>> CreateTask([FromBody] TaskEntity task)
        {
            
            if (task == null)
            {
                return BadRequest("Task details are required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Log the Task Entity just to see what is being passed
            Console.WriteLine($"Task received: {JsonConvert.SerializeObject(task)}");

            // Generate Task ID based on the auto-incremented ID
            // Assuming that TaskId must also be unique
            var lastTask = await _context.COURSEDETAILS.OrderByDescending(t => t.Id).FirstOrDefaultAsync();
            int newTaskId = (lastTask?.Id ?? 0) + 1; // Note: Ideally, let the database assign the Id

            task.Courseid = $"COURSE{newTaskId:00}";
            //task.Id = newTaskId;
            _context.COURSEDETAILS.Add(task);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log error details
                Console.WriteLine($"Error message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskEntity>> GetTaskById(int id)
        {
            var task = await _context.COURSEDETAILS.FindAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskEntity>>> GetAllTasks()
        {
            var tasks = await _context.COURSEDETAILS.ToListAsync();
            return Ok(tasks);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, [FromBody] TaskEntity updatedTask)
        {
            if (id != updatedTask.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(updatedTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TaskExists(id)) return NotFound();
                throw; // Re-throw the exception for higher-level handling
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var task = await _context.COURSEDETAILS.FindAsync(id);
            if (task == null) return NotFound();

            _context.COURSEDETAILS.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> TaskExists(int id)
        {
            return await _context.COURSEDETAILS.AnyAsync(e => e.Id == id);
        }
    }
}