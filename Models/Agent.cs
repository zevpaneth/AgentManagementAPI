using AgentManagementAPI.Classes;
using AgentManagementAPI.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace AgentManagementAPI.Models
{
    public class Agent
    {
        //{"nickname":"Agent010","photoUrl":"https://randomuser.me/api/portraits/women/10.jpg"}

        [Key]
        public Guid Id { get; set; }

        public string photoUrl { get; set; } // => Link to the picture

        public string nickname {  get; set; }

        public Location? Location { get; set; }

        public AgentStatus AgentStatus { get; set; }
        
    }
}
