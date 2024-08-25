using AgentManagementAPI.Classes;
using AgentManagementAPI.Enums;
using NuGet.Common;
using System.ComponentModel.DataAnnotations;

namespace AgentManagementAPI.Models
{
    public class Target : BaseModel
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public TargetStatus TargetStatus { get; set; } 
    }

    
}