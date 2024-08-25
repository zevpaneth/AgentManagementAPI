using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Classes;


namespace AgentManagementAPI.Services
{
    public class TargetMissionsCreator : BaseMissionsCreator
    {
        private readonly AgentManagementAPIContext _context;


        public TargetMissionsCreator(AgentManagementAPIContext context)
        {
            _context = context;

        }
        public async Task CreateMissions(Target target)
        {
            var agents = await _context.Agent.Include(a => a.Location).ToListAsync();
            if (agents.Count > 0)
            {


                for (int i = 0; i < agents.Count; i++)
                {
                    var agent = agents[i];
                    var distance = CheckDistanceFunction(target.Location, agent.Location);
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
                string connection = _context.Database.GetConnectionString();
                _context.SaveChanges();
                await _context.SaveChangesAsync();
            }
        }
    }
}
