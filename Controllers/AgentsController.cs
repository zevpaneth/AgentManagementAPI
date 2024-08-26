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


namespace AgentManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class agentsController : ControllerBase
    {
        private readonly AgentManagementAPIContext _context;
        private readonly AgentMissionsCreator _agentMissionsCreator;
        private readonly ModelSearchor _modelSearchor;


        public agentsController(AgentManagementAPIContext context, AgentMissionsCreator agentMissionsCreator, ModelSearchor modelSearchor)
        {
            _context = context;
            _agentMissionsCreator = agentMissionsCreator;
            _modelSearchor = modelSearchor;

        }

        // GET: api/Agents
        [HttpGet]
        public async Task<IActionResult> GetAgent()
        {
            var result = await _modelSearchor.AgentsWithLocation();
            return StatusCode(StatusCodes.Status200OK, new { result = result });
        }

        // GET: api/Agents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agent>> GetAgent(int id)
        {
            var agent = await _modelSearchor.AgentHunter(id);
            if (agent == null)
            {
                return NotFound();
            }

            return agent;
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
                
                await _agentMissionsCreator.CreateMissions (agent);


                return StatusCode(200, _context.Agent.ToArray());

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
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
                Agent agentFromDb = await _modelSearchor.AgentHunter(id);
                if (agentFromDb.AgentStatus == Enums.AgentStatus.Active )
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { Eror = "The agent cannot be moved in action" });
                }
                int currentX = agentFromDb.Location.x;
                int currentY = agentFromDb.Location.y;

                BaseModel agentToMove = MoveService.MoveServiceFunction(agentFromDb, direction);
                Agent agentToDb = agentFromDb;
                agentToDb.Location = agentToMove.Location;

                _context.Agent.Update(agentToDb);

                try
                {
                    await _context.SaveChangesAsync();
                    
                    await _agentMissionsCreator.CreateMissions(agentToDb);
                    return StatusCode(200, _context.Target.ToArray());
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {

                    return StatusCode(400, new { Eror = "Can't be moved outside the matrix", currentX, currentY });
                }

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
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
