using AgentManagementAPI.Classes;
using AgentManagementAPI.Enums;
using NuGet.Common;
using System.ComponentModel.DataAnnotations;

namespace AgentManagementAPI.Models
{
    public class Target
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string PhotoUrl { get; set; }
        public Location? Location { get; set; }
        public TargetStatus TargetStatus { get; set; } 
    }

    
}