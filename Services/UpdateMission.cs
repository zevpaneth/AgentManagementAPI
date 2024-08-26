using AgentManagementAPI.Classes;
using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity;

namespace AgentManagementAPI.Services
{
    public class UpdateMission
    {
        private readonly AgentManagementAPIContext _context;
        private readonly ModelSearchor _modelSearchor;


        public UpdateMission(AgentManagementAPIContext context, ModelSearchor modelSearchor)
        {
            _context = context;
            _modelSearchor = modelSearchor;
        }
        public async Task<bool> UpdateMissions(int missionId)
        {
            Mission mission = await _modelSearchor.MissionHunter(missionId);
            Agent agent = await _modelSearchor.AgentHunter(mission.AgentId);
            var locations = await _modelSearchor.MissionTargetsAgentsLocations(missionId);

            Location agentLocation = locations["agentLocation"];
            Location targetLocation = locations["targetLocation"];

            bool closeDistance = Distance.CloseDistance(agentLocation, targetLocation);

            if (!closeDistance)
            {
                _context.Mission.Remove(mission);
                await _context.SaveChangesAsync();
                return false;
            }

            if (closeDistance)
            {
                mission.MissionStatus = Enums.MissionStatus.assigned;
                _context.Update(mission);
                agent.AgentStatus = Enums.AgentStatus.Active;
                _context.Update(agent);
                _context.SaveChangesAsync();

                var missionsToDelete = await _context.Mission.ToArrayAsync();
                foreach (Mission missionDelete in missionsToDelete)
                {
                    if (missionDelete.MissionStatus == Enums.MissionStatus.Advise_mission && missionDelete.AgentId == mission.AgentId)
                    {
                        await DeleteMission(missionDelete.Id);

                    }
                }
                return true;

            }
            return false;
           
        }

        public async Task DeleteMission(int missionId)
        {
            var mission = await _context.Mission.FindAsync(missionId);
       
            _context.Mission.Remove(mission);
            await _context.SaveChangesAsync();
        }
    }
}
