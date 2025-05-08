using Quarter.Core.Entites;
using Quarter.Repostory.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quarter.Repostory
{
    public static class StoreDbContextSeed
    {
        public async static Task SeedAsync(QuarterDbContexts _context)
        {
            if (_context.EstateLocations.Count() == 0)
            {

                var EstateLocationsData = File.ReadAllText("C:\\Users\\AHMEDASHRAF\\source\\repos\\QuarterEstate.APIS\\Quarter.Repostory\\Data\\DataSeed\\EstateLocationss.json");
                var EstateLocationss = JsonSerializer.Deserialize<List<EstateLocation>>(EstateLocationsData);
                if (EstateLocationss is not null && EstateLocationss.Count() > 0)
                {
                    await _context.EstateLocations.AddRangeAsync(EstateLocationss);
                    await _context.SaveChangesAsync();
                }
            }
            if (_context.EstateTypes.Count() == 0)
            {
                var EstateTypesData = File.ReadAllText("C:\\Users\\AHMEDASHRAF\\source\\repos\\QuarterEstate.APIS\\Quarter.Repostory\\Data\\DataSeed\\EstateTypess.json");
                var EstateTypess = JsonSerializer.Deserialize<List<EstateType>>(EstateTypesData);
                if (EstateTypess is not null && EstateTypess.Count() > 0)
                {
                    await _context.EstateTypes.AddRangeAsync(EstateTypess);
                    await _context.SaveChangesAsync();
                }
            }
            if (_context.Estates.Count() == 0)
            {
                var EstateData = File.ReadAllText("C:\\Users\\AHMEDASHRAF\\source\\repos\\QuarterEstate.APIS\\Quarter.Repostory\\Data\\DataSeed\\Estatess.json");
                var Estatess = JsonSerializer.Deserialize<List<Estate>>(EstateData);
                if (Estatess is not null && Estatess.Count() > 0)
                {
                    await _context.Estates.AddRangeAsync(Estatess   );
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
