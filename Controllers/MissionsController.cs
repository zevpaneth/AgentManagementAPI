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
using AgentManagementAPI.Classes;

namespace AgentManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController : ControllerBase
    {
        private readonly AgentManagementAPIContext _context;
        private readonly ModelSearchor _modelSearchor;
        private readonly StatusUpdateMission _updateMission;


        public MissionsController(AgentManagementAPIContext context, ModelSearchor modelSearchor, StatusUpdateMission updateMission )
        {
            _context = context;
            _modelSearchor = modelSearchor;
            _updateMission = updateMission;
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
        [HttpPut("{missionId}")]
        public async Task<IActionResult> PutMission(int missionId)
        {

            bool result = await _updateMission.StatusUpdateMissions(missionId);

            if (!result)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { eror = "They are not within 200 kilometers" });
            }

            if (result)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            try
            {


                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MissionExists(missionId))
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

        [HttpPost("Update")]
        public async Task<ActionResult> UpdateMissions()
        {
            _updateMission.MissionsUpdate();
                
        }

        
    }
}
