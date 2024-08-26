using AgentManagementAPI.Classes;

namespace AgentManagementAPI.Services
{
    public class Distance
    {
        public static double? CheckDistance(Location location1, Location location2)
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

        public static bool CloseDistance(Location location1, Location location2)
        {
            var distance = CheckDistance(location1, location2);
            return distance <= 200;
        }
    }
}
