using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultispatialLogistics.Core3.Models
{
    public class Stargate
    {
        public int Id { get; set; }

        public int ParentSystemId { get; set; }
        public string ParentSystemName { get; set; }
        public int StargateId { get; set; }
        public int DestinationSystemId { get; set; }
        public int DestinationStargateId { get; set; }

        public long XPos { get; set; }
        public long YPos { get; set; }
        public long ZPos { get; set; }
    }
}
