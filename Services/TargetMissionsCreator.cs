using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Classes;


namespace AgentManagementAPI.Services
{
    public class TargetMissionsCreator
    {
        private readonly AgentManagementAPIContext _context;
        private readonly ModelSearchor _modelSearchor;


        public TargetMissionsCreator(AgentManagementAPIContext context, ModelSearchor modelSearchor)
        {
            _context = context;
            _modelSearchor = modelSearchor;
        }
        public async Task CreateMissions(Target target)
        {
            var agents = await _modelSearchor.AgentsWithLocation();
            if (agents.Count > 0)
            {


                for (int i = 0; i < agents.Count; i++)
                {
                    var agent = agents[i];
                    bool closeDistabce = Distance.CloseDistance(target.Location, agent.Location);
                    if (closeDistabce)
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
}
