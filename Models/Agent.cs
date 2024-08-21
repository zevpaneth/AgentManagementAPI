using AgentManagementAPI.Classes;
using AgentManagementAPI.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace AgentManagementAPI.Models
{
    public class Agent
    {
        [Key]
        public Guid Id { get; set; }

        public string Image { get; set; } // => Link to the picture

        public string Nickname {  get; set; }

        public Location? Location { get; set; }

        public AgentStatus AgentStatus { get; set; }
        
    }
}
