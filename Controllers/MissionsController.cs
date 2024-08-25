using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using NetTopologySuite.Operation.Distance3D;
using AgentManagementAPI.Services;

namespace AgentManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController : ControllerBase
    {
        private readonly AgentManagementAPIContext _context;


        public MissionsController(AgentManagementAPIContext context)
        {
            _context = context;
        }

        // GET: api/Missions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mission>>> GetMission()
        {
            return await _context.Mission.ToListAsync();
        }

        // GET: api/Missions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mission>> GetMission(int id)
        {
            var mission = await _context.Mission.FindAsync(id);

            if (mission == null)
            {
                return NotFound();
            }

            return mission;
        }

        // PUT: Missions/5/ to activizing a mission
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMission(int id, Mission mission)
        {
            if (id != mission.Id)
            {
                return BadRequest();
            }
            var agentId = mission.AgentId;

            // to include the updated location from all
            var agents = await _context.Agent.Include(a => a.Location).ToArrayAsync();
            // to find our agent
            Agent agent = agents.FirstOrDefault(a => a.Id == agentId);
            // to include the updated location from all
            var targets = await _context.Target.Include(t => t.Location).ToArrayAsync();
            // to find our target
            var targetId = mission.TargetId;
            Target target = targets.FirstOrDefault(t => t.Id == targetId);
            var distance = BaseMissionsCreator.CheckDistanceFunction(agent.Location, target.Location);

            if (distance > 200)
            {
                _context.Mission.Remove(mission);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status404NotFound, new { eror = "They are not within 200 kilometers" });
            }
            
            if (distance <= 200)
            {
                mission.MissionStatus = Enums.MissionStatus.assigned;
                _context.Update(mission);
                agent.AgentStatus = Enums.AgentStatus.Active;
                _context.Update(agent);
                var missionsToDelete = await _context.FindAsync(agentId)
                await _context.SaveChangesAsync();

            }
            try
            {
                

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MissionExists(id))
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

        // POST: api/Missions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mission>> PostMission(Mission mission)
        {
            _context.Mission.Add(mission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMission", new { id = mission.Id }, mission);
        }

        // DELETE: api/Missions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMission(int id)
        {
            var mission = await _context.Mission.FindAsync(id);
            if (mission == null)
            {
                return NotFound();
            }

            _context.Mission.Remove(mission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MissionExists(int id)
        {
            return _context.Mission.Any(e => e.Id == id);
        }
    }
}
