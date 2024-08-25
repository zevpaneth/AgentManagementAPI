using AgentManagementAPI.Classes;
using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentManagementAPI.Services
{
    public class AgentMissionsCreator : BaseMissionsCreator
    {
        private readonly AgentManagementAPIContext _context;


        public AgentMissionsCreator(AgentManagementAPIContext context)
        {
            _context = context;

        }


        public async Task CreateMissions(Agent agent)
        {
            var targets = await _context.Target.Include(t => t.Location).ToListAsync();
            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                var distance = CheckDistanceFunction(agent.Location, target.Location);
                if (distance <= 200)
                {
                    Mission mission = new Mission
                    {
                        AgentId = agent.Id,
                        TargetId = target.Id,
                        MissionStatus = Enums.MissionStatus.Advise_mission
                    };
                    _context.Mission.Add(mission);
                }
            }
            var mapping = _context.Model.FindEntityType(typeof(Mission));
            string schema = mapping.GetSchema();
            string table = mapping.GetTableName();

            _context.SaveChanges();
            await _context.SaveChangesAsync();

        }
    }
}
