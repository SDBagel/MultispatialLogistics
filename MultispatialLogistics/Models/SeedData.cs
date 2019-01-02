using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MultispatialLogistics.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MultispatialLogisticsContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MultispatialLogisticsContext>>()))
            {
                // Look for any movies.
                if (context.Stargate.Any())
                {
                    return;   // DB has been seeded
                }

                context.Stargate.AddRange(
                    new Stargate
                    {
                        ParentSystemId = 30004238,
                        StargateId = 50010925,
                        DestinationSystemId = 30004236,
                        DestinationStargateId = 50010610,
                        XPos = 1990315991040,
                        YPos = -85509857280,
                        ZPos = -244676321280,
                    },

                    new Stargate
                    {
                        ParentSystemId = 30004238,
                        StargateId = 50010926,
                        DestinationSystemId = 30004237,
                        DestinationStargateId = 50010791,
                        XPos = 1990302965760,
                        YPos = -85504450560,
                        ZPos = -244692541440,
                    },

                    new Stargate
                    {
                        ParentSystemId = 30004238,
                        StargateId = 50010927,
                        DestinationSystemId = 30004239,
                        DestinationStargateId = 50010939,
                        XPos = 117892177920,
                        YPos = -5075066880,
                        ZPos = -550343516160,
                    },

                    new Stargate
                    {
                        ParentSystemId = 30004240,
                        StargateId = 50010966,
                        DestinationSystemId = 30004239,
                        DestinationStargateId = 50010940,
                        XPos = 1291384381440,
                        YPos = -35176980480,
                        ZPos = 1862695034880,
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
