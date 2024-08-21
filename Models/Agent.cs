using AgentManagementAPI.Classes;
using AgentManagementAPI.Enums;


namespace AgentManagementAPI.Models
{
    public class Agent
    {
        public string Image { get; set; } // => Link to the picture
        public string Nickname {  get; set; }
        public Location Location { get; set; }
        public AgentStatus AgentStatus { get; set; }
        
    }
}
