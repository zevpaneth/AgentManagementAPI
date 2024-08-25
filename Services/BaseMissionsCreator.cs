using AgentManagementAPI.Classes;
using AgentManagementAPI.Data;

namespace AgentManagementAPI.Services
{
    public class BaseMissionsCreator
    {


        public static double? CheckDistanceFunction(Location location1, Location location2)
        {
            if (location1 != null && location2 != null)
            {
                int x1 = location1.x;
                int y1 = location1.y;
                int x2 = location2.x;
                int y2 = location2.y;
                double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                return distance;
            }
            return null;
        }
    }
}
