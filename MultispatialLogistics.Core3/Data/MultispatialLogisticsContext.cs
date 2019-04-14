using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MultispatialLogistics.Core3.Models
{
    public class MultispatialLogisticsContext : DbContext
    {
        public MultispatialLogisticsContext (DbContextOptions<MultispatialLogisticsContext> options)
            : base(options)
        {
        }

        public DbSet<MultispatialLogistics.Core3.Models.Stargate> Stargate { get; set; }
    }
}
