using AgentManagementAPI.Classes;
using AgentManagementAPI.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace AgentManagementAPI.Models
{
    public class Agent : BaseModel
    {
        public string nickname {  get; set; }

        public AgentStatus AgentStatus { get; set; }
        
    }
}
