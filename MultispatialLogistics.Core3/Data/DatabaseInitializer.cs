using MultispatialLogistics.Core3.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultispatialLogistics.Core3.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(MultispatialLogisticsContext context, string seedJson)
        {
            // Make sure database exists
            context.Database.EnsureCreated();

            // If true, database has already been seeded
            if (context.Stargate.Any())
                return;

            // Get data, add it, and save
            List<Stargate> stargates = JsonConvert.DeserializeObject<List<Stargate>>(seedJson);
            context.Stargate.AddRange(stargates); 
            context.SaveChanges(); 
        }
    }
}
