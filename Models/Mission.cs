using Humanizer;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AgentManagementAPI.Enums;
using System.ComponentModel;

namespace AgentManagementAPI.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Agent"), TypeConverter("int")]
        public int AgentId { get; set; }
        
        [ForeignKey("Target"), TypeConverter("int")]
        public int TargetId { get; set; }

        public double? Timer { get; set; }

        public double? ExecutionTime { get; set; }

        public MissionStatus MissionStatus { get; set; }
    }
}
