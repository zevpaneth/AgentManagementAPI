using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using System.Collections;

using AgentManagementAPI.Classes;
using AgentManagementAPI.Services;

namespace AgentManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class targetsController : ControllerBase
    {
        private readonly AgentManagementAPIContext _context;

        public targetsController(AgentManagementAPIContext context)
        {
            _context = context;
        }

        // GET: api/Targets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Target>>> GetTarget()
        {
            return await _context.Target.Include(T => T.Location)?.ToArrayAsync();
        }

        // GET: api/Targets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Target>> GetTarget(int id)
        {
            // to include the updated location from all
            var targets = await _context.Target.Include(t => t.Location)?.ToArrayAsync();
            // to find our target
            var target = targets.FirstOrDefault(t => t.Id == id);

            if (target == null)
            {
                return NotFound();
            }

            return target;
        }

        // PUT: api/Targets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> PutTarget(int id, Location location)
        {
            try

            { 
                Target target = await _context.Target.FindAsync(id);
                target.Location = location;
                _context.Target.Update(target);

                await _context.SaveChangesAsync();
                return StatusCode(200, _context.Target.ToArray());
                    
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


        [HttpPut("{id}/move")]
        public async Task<IActionResult> MoveTarget(int id, Direction direction)
        {
            try

            {
                // to include the updated location from all
                var targets = await _context.Target.Include(t => t.Location)?.ToArrayAsync();
                // to find our target
                var target = targets.FirstOrDefault(t => t.Id == id);

                target  = MoveService.MoveServiceFunction(target, direction);
                
                _context.Target.Update(target);

                await _context.SaveChangesAsync();
                return StatusCode(200, _context.Target.ToArray());

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

            target.TargetStatus = Enums.TargetStatus.Alive;

            var result = await _context.Target.AddAsync(target);

            await _context.SaveChangesAsync();



            return StatusCode(StatusCodes.Status201Created, new { id = result.Entity.Id}
            );

        }
        
        
        
        // DELETE: api/Targets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarget(int id)
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

        private bool TargetExists(int id)
        {
            return _context.Target.Any(e => e.Id == id);
        }
    }
}
