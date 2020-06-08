using System.Drawing;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EcsDigitalApi.Entities
{
    public static class DataSeeder
    {

        public static async Task SeedInMemory(this CarsContext context)
        {
            await context.Cars.AddRangeAsync(
                    new Car { Id = 1, Colour = Color.Black, ModelId = 33019, Year=2019 },
                    new Car { Id = 2, Colour = Color.Black, ModelId = 44129, Year=2019 },
                    new Car { Id = 3, Colour = Color.Black, ModelId = 55519, Year=2019 },
                    new Car { Id = 4, Colour = Color.Black, ModelId = 66269, Year=2019 },
                    new Car { Id = 5, Colour = Color.Black, ModelId = 73279, Year=2019 },
                    new Car { Id = 6, Colour = Color.Black, ModelId = 90919, Year=2019 });

            await context.SaveChangesAsync();
        }

    }
}
