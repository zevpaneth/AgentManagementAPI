using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Data;
using AgentManagementAPI.Models;

namespace AgentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private readonly AgentManagementAPIContext _context;

        public TargetsController(AgentManagementAPIContext context)
        {
            _context = context;
        }

        // GET: api/Targets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Target>>> GetTarget()
        {
            return await _context.Target.ToListAsync();
        }

        // GET: api/Targets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Target>> GetTarget(Guid id)
        {
            var target = await _context.Target.FindAsync(id);

            if (target == null)
            {
                return NotFound();
            }

            return target;
        }

        // PUT: api/Targets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarget(Guid id, Target target)
        {
            if (id != target.Id)
            {
                return BadRequest();
            }

            _context.Entry(target).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TargetExists(id))
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

        // POST: api/Targets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Target>> PostTarget(Target target)
        {
            _context.Target.Add(target);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarget", new { id = target.Id }, target);
        }

        // DELETE: api/Targets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarget(Guid id)
        {
            var target = await _context.Target.FindAsync(id);
            if (target == null)
            {
                return NotFound();
            }

            _context.Target.Remove(target);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TargetExists(Guid id)
        {
            return _context.Target.Any(e => e.Id == id);
        }
    }
}
