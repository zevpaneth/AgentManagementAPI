using AgentManagementAPI.Classes;
using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Reflection;

namespace AgentManagementAPI.Services
{
    public class StatusUpdateMission
    {
        private readonly AgentManagementAPIContext _context;
        private readonly ModelSearchor _modelSearchor;
        private readonly MoveService _moveService;


        public StatusUpdateMission(AgentManagementAPIContext context, ModelSearchor modelSearchor, MoveService moveService)
        {
            _context = context;
            _modelSearchor = modelSearchor;
            _moveService = moveService;
        }
        public async Task<bool> StatusUpdateMissions(int missionId)
        {
            Mission mission = await _modelSearchor.MissionHunter(missionId);
            Agent agent = await _modelSearchor.AgentHunter(mission.AgentId);
            Target target = await _modelSearchor.TargetHunter(mission.TargetId);

            // Clears all tasks with this agent and target
            await ClearMissions(agent.Id, target.Id, mission.Id);
            
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

        public async Task ClearMissions(int agentId, int targetId, int misionId)
        {
            var missions = await _context.Mission.ToListAsync();
            foreach (Mission mission in missions)
            {
                if (mission.TargetId == targetId && mission.AgentId == agentId)
                {
                    if (mission.Id != misionId)
                    {
                        await DeleteMission(mission.Id);
                    }
                }

            }
        }

        public async Task MissionsUpdate()
        {
            var missions = await _context.Mission.ToListAsync();
            var missionsToUpdate = missions.Where<Mission>(m => m.MissionStatus == Enums.MissionStatus.assigned);
            foreach (Mission mission in missionsToUpdate)
            {
                Agent agentToMove = await _moveService.MovementToDirection(mission);
                _context.Agent.Update(agentToMove);
                await _context.SaveChangesAsync();
            }
        }
    }
}
