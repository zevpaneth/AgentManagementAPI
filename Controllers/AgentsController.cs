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
    public class AgentsController : ControllerBase
    {
        private readonly AgentManagementAPIContext _context;

        public AgentsController(AgentManagementAPIContext context)
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
        public async Task<ActionResult<Agent>> GetAgent(Guid id)
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
        public async Task<IActionResult> PutAgent(Guid id, Agent agent)
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

        // POST: api/Agents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Agent>> PostAgent(Agent agent)
        {
            _context.Agent.Add(agent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgent", new { id = agent.Id }, agent);
        }

        // DELETE: api/Agents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgent(Guid id)
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

        private bool AgentExists(Guid id)
        {
            return _context.Agent.Any(e => e.Id == id);
        }
    }
}
