using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using AgentManagementAPI.Classes;
using AgentManagementAPI.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AgentManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class agentsController : ControllerBase
    {
        private readonly AgentManagementAPIContext _context;

        public agentsController(AgentManagementAPIContext context)
        {
            _context = context;
        }

        // GET: api/Agents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAgent()
        {
            return await _context.Agent.ToListAsync();
        }

        // GET: api/Agents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agent>> GetAgent(int id)
        {
            var agent = await _context.Agent.FindAsync(id);

            if (agent == null)
            {
                return NotFound();
            }

            return agent;
        }

        // PUT: api/Agents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgent(int id, Agent agent)
        {
            if (id != agent.Id)
            {
                return BadRequest();
            }

            _context.Entry(agent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentExists(id))
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

    // POST: Agents
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
        public async Task<ActionResult<Agent>> PostAgent(Agent agent)
        {
            _context.Agent.Add(agent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgent", new { id = agent.Id });
        }

        // DELETE: api/Agents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgent(int id)
        {
            var agent = await _context.Agent.FindAsync(id);
            if (agent == null)
            {
                return NotFound();
            }

            _context.Agent.Remove(agent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/pin")]
        public async Task<IActionResult> PutAgent(int id, Location location)
        {
            try

            {
                Agent agent = await _context.Agent.FindAsync(id);
                agent.Location = location;
                _context.Agent.Update(agent);

                await _context.SaveChangesAsync();
                return StatusCode(200, _context.Agent.ToArray());

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentExists(id))
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

        private bool AgentExists(int id)
        {
            return _context.Agent.Any(e => e.Id == id);
        }

        [HttpPut("{id}/move")]
        public async Task<IActionResult> MoveTarget(int id, Direction direction)
        {
            try

            {
                // to include the updated location from all
                var agents = await _context.Agent.Include(a => a.Location).ToArrayAsync();
                // to find our agent
                Agent agentFromDb = agents.FirstOrDefault(a => a.Id == id);
                int currentX = agentFromDb.Location.x;
                int currentY = agentFromDb.Location.y;

                BaseModel agentToMove = MoveService.MoveServiceFunction(agentFromDb, direction);
                Agent agentToDb = agentFromDb;
                agentToDb.Location = agentToMove.Location;

                _context.Agent.Update(agentToDb);

                try
                {
                    await _context.SaveChangesAsync();
                    return StatusCode(200, _context.Target.ToArray());
                }
                catch (DbUpdateException)
                {

                    return StatusCode(400, new { Eror = "Can't be moved outside the matrix", currentX, currentY });
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentExists(id))
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
    }
}
