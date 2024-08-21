using AgentManagementAPI.Enums;
using System.Drawing;

namespace AgentManagementAPI.Models
{
    public class Target
    {
        public string Name { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public Point Location { get; set; } = new Point();
        public TargetStatus TargetStatus { get; set; }
    }
}