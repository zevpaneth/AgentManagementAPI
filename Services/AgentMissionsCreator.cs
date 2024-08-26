using AgentManagementAPI.Classes;
using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentManagementAPI.Services
{
    public class AgentMissionsCreator 
    {
        private readonly AgentManagementAPIContext _context;
        private readonly ModelSearchor _modelSearchor;


        public AgentMissionsCreator(AgentManagementAPIContext context, ModelSearchor modelSearchor)
        {
            _context = context;
            _modelSearchor = modelSearchor;
        }


        public async Task CreateMissions(Agent agent)
        {
            var targets = await _modelSearchor.TargetsWithLocation();
            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                bool closeDistance = Distance.CloseDistance(agent.Location, target.Location);
                if (closeDistance)
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
            await _context.SaveChangesAsync();

        }
    }
}
