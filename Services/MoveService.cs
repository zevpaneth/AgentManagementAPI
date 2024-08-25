using AgentManagementAPI.Classes;
using AgentManagementAPI.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AgentManagementAPI.Services
{
    public class MoveService
    {

        public static BaseModel MoveServiceFunction(BaseModel model, Direction direction)
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


    }
}
