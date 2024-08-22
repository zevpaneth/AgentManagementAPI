using AgentManagementAPI.Classes;
using AgentManagementAPI.Enums;

namespace AgentManagementAPI.Models
{
    public class Target
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public Location Location { get; set; }
        public TargetStatus TargetStatus { get; set; } 
    }

    
}