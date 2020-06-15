using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using EcsDigitalApi.DbModels;
using EcsDigitalApi.Services;

namespace EcsDigitalApi.Data
{
    public static class DataSeeder
    {

        public static async Task SeedInMemory(this CarsContext context)
        {

            await context.Makers.AddRangeAsync(
                new Maker { Id = 1, Name = "Ford" },
                new Maker { Id = 2, Name = "cooper" });

            await context.Models.AddRangeAsync(
                new Model { Id = 1, Name = "Mini", MakerId = 2, RelatedWords = GetRelatedWords("Mini") },
                new Model { Id = 2, Name = "Focus", MakerId = 1, RelatedWords = GetRelatedWords("Focus") },
                new Model { Id = 3, Name = "Fiesta", MakerId = 1, RelatedWords = GetRelatedWords("Fiesta") });

            await context.Cars.AddRangeAsync(
                    new Car { Id = 1, Colour = Color.Blue, ModelId = 1, Year=2019},
                    new Car { Id = 2, Colour = Color.BlueViolet, ModelId = 2, Year=2018 },
                    new Car { Id = 3, Colour = Color.Black, ModelId = 3, Year=2016 },
                    new Car { Id = 4, Colour = Color.DeepPink, ModelId = 1, Year=2020 },
                    new Car { Id = 5, Colour = Color.Green, ModelId = 2, Year=2009 },
                    new Car { Id = 6, Colour = Color.Yellow, ModelId = 3, Year=2004 });

            await context.SaveChangesAsync();
        }

        private static string GetRelatedWords(string word)
        {
            var relatedService = new RelatedWordsService();
            var relatedWords = relatedService.GetRelatedWords(word).GetAwaiter().GetResult().ToList();
            return relatedWords.Any()
                ? relatedWords.Take(10).Aggregate((p1, p2) => p1 + ", " + p2)
                : string.Empty;
        }

    }
}
