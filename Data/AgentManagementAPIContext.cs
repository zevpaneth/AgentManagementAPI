using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AgentManagementAPI.Models;

namespace AgentManagementAPI.Data
{
    public class AgentManagementAPIContext : DbContext
    {
        public AgentManagementAPIContext (DbContextOptions<AgentManagementAPIContext> options)
            : base(options)
        {
        }

        public DbSet<AgentManagementAPI.Models.Target> Target { get; set; } = default!;
        public DbSet<AgentManagementAPI.Models.Agent> Agent { get; set; } = default!;
    }
}
