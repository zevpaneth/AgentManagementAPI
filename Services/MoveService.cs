using AgentManagementAPI.Classes;
using AgentManagementAPI.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AgentManagementAPI.Services
{
    public class MoveService
    {

        public static Target MoveServiceFunction(Target target, Direction direction)
        {
            
            string di = direction.direction;
            if (di.Contains("n"))
            {
                target.Location.y += 1;
            }
            if (di.Contains("s"))
            {
                target.Location.y -= 1;
            }
            if (di.Contains("w"))
            {
                target.Location.x -= 1;
            }
            if (di.Contains("e"))
            {
                target.Location.x += 1;
            }
            return target;

        }
    }
}
