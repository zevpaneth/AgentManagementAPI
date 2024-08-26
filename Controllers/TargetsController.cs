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
using Microsoft.OpenApi.Validations;

namespace AgentManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class targetsController : ControllerBase
    {
        private readonly AgentManagementAPIContext _context;
        private TargetMissionsCreator _targetMissionsCreator;
        private readonly ModelSearchor _modelSearchor;

        public targetsController(AgentManagementAPIContext context, TargetMissionsCreator targetMissionsCreator, ModelSearchor modelSearchor)
        {
            _context = context;
            _targetMissionsCreator = targetMissionsCreator;
            _modelSearchor = modelSearchor;


        }

        // GET: api/Targets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Target>>> GetTarget()
        {
            return await _modelSearchor.TargetsWithLocation();
        }

        // GET: api/Targets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Target>> GetTarget(int id)
        {
            // to include the updated location from all
            var targets = await _modelSearchor.TargetsWithLocation();
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
                await _targetMissionsCreator.CreateMissions(target);
                var retObj = await _context.Target.ToArrayAsync();
                return StatusCode(200, retObj);
                    
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
                Target targetFromDb = await _modelSearchor.TargetHunter(id);

                // save the curent location in a case that didnt change
                int x = targetFromDb.Location.x;
                int y = targetFromDb.Location.y;

                // use the base model for using the move service function
                BaseModel targetToMove = MoveService.MoveServiceFunction(targetFromDb, direction);
                Target targetToDb = targetFromDb;

                // update the location 
                targetToDb.Location = targetToMove.Location;
                _context.Target.Update(targetToDb);

                try
                {
                    await _context.SaveChangesAsync();
                    await _targetMissionsCreator.CreateMissions(targetToDb);

                    var retObj = await _context.Target.ToArrayAsync();

                    return StatusCode(200, retObj);
                }
                catch (DbUpdateException)
                {

                    return StatusCode(400, new { Eror = "Can't be moved outside the matrix", x, y });
                }

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
