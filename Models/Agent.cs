using AgentManagementAPI.Enums;
using System.Drawing;

namespace AgentManagementAPI.Models
{
    public class Agent
    {
        public string Image = string.Empty; // => Link to the picture
        public string Nickname = string.Empty;
        public Point Location = new Point();
        public AgentStatus AgentStatus;
        
    }
}
