using AgentManagementAPI.Classes;
using AgentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace AgentManagementAPI.Services
{
    public class MoveService
    {
        private readonly ModelSearchor _modelSearchor;
        public MoveService(ModelSearchor modelSearchor) {
            _modelSearchor = modelSearchor;
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

            Location agentLocation = await _modelSearchor.AgentLocation(agentId);
            Location targetLocation = await _modelSearchor.TargetLocation(targetId);

            var directionY = agentLocation.y - targetLocation.y;
            var directionX = agentLocation.x - targetLocation.x;

            Direction directionClass = new Direction();

            string direction = directionClass.direction;


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

            Agent agent = await _modelSearchor.AgentHunter(agentId);
            BaseModel baseModel = MoveFunction(agent, directionClass);
            agent.Location = baseModel.Location;
            return agent;

        }



    }
}
