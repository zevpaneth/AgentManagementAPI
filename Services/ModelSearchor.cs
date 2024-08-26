using AgentManagementAPI.Classes;
using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AgentManagementAPI.Services
{
    public class ModelSearchor
    {
        private readonly AgentManagementAPIContext _context;


        public ModelSearchor(AgentManagementAPIContext context)
        {
            _context = context;
        }

        public async Task<List<Agent>> AgentsWithLocation()
        {
            var result  = await _context.Agent.Include(a => a.Location).ToListAsync();
            return result;
        }

        public async Task<Agent> AgentHunter(int id)
        {
            List<Agent> agents = await AgentsWithLocation();
            Agent agent = agents.FirstOrDefault(a => a.Id == id);
            return agent;
        }

        public async Task<Location> AgentLocation(int id)
        {
            Agent agent = await AgentHunter(id);
            return agent.Location;
        }

        public async Task<List<Target>> TargetsWithLocation()
        {
            return await _context.Target.Include(t => t.Location).ToListAsync();

        }

        public async Task<Target> TargetHunter(int id)
        {
            var targets = await TargetsWithLocation();
            Target target = targets.FirstOrDefault(t => t.Id == id);
            return target;
        }

        public async Task<Location> TargetLocation(int id)
        {
            Target target = await TargetHunter(id);
            return target.Location;
        }

        public async Task<Mission> MissionHunter(int id)
        {
            return await _context.Mission.FindAsync(id);
        }

        public async Task<Dictionary<string, Location>> MissionTargetsAgentsLocations(int id)
        {
            Dictionary<string, Location> locations = new Dictionary<string, Location>();

            Mission mission = await MissionHunter(id);

            int agentId = mission.AgentId;

            Location agentLocation = await AgentLocation(agentId);

            int targetId = mission.TargetId;

            Location targetLocation = await TargetLocation(targetId);

            locations.Add("agentLocation", agentLocation);
            locations.Add("targetLocation", targetLocation);

            return locations;

        }
    }
}
