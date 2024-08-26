using AgentManagementAPI.Classes;
using AgentManagementAPI.Data;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace AgentManagementAPI.Services
{
    public class MoveService
    {
        private readonly AgentManagementAPIContext _context;
        private readonly ModelSearchor _modelSearchor;
        public MoveService(ModelSearchor modelSearchor, AgentManagementAPIContext context)
        {
            _modelSearchor = modelSearchor;
            _context = context;
        }

        public static BaseModel MoveFunction(BaseModel model, Direction direction)
        {
            
            string di = direction.direction;
            if (di.Contains("n"))
            {
                model.Location.y += 1;
            }
            if (di.Contains("s"))
            {
                model.Location.y -= 1;
            }
            if (di.Contains("w"))
            {
                model.Location.x -= 1;
            }
            if (di.Contains("e"))
            {
                model.Location.x += 1;
            }
            return model;

        }

        public async Task<Agent> MovementToDirection(Mission mission)
        {
            int agentId = mission.AgentId;
            int targetId = mission.TargetId;

            Agent agent = await _modelSearchor.AgentHunter(agentId);

            Location agentLocation = await _modelSearchor.AgentLocation(agentId);
            Location targetLocation = await _modelSearchor.TargetLocation(targetId);

            var directionY = agentLocation.y - targetLocation.y;
            var directionX = agentLocation.x - targetLocation.x;

            Direction directionClass = new Direction();

            string direction = directionClass.direction;



            if (directionX == 0 && directionY == 0)
            {

                Target target = await _modelSearchor.TargetHunter(targetId);
                target.TargetStatus = Enums.TargetStatus.Eliminated;
                _context.Target.Update(target);
                agent.AgentStatus = Enums.AgentStatus.Dormant;
                _context.Agent.Update(agent);
                await _context.SaveChangesAsync();
                return agent;

            }

            if (directionY > 0)
            {
                direction += "n";
            }
            else if (directionX > 0)
            {
                direction += "s";
            }
            if (directionX > 0)
            {
                direction += "e";
            }
            else if (directionX < 0)
            {
                direction += "w";
            }

            BaseModel baseModel = MoveFunction(agent, directionClass);
            agent.Location = baseModel.Location;
            return agent;

        }



    }
}
