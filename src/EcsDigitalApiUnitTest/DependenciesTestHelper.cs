using System.Threading.Tasks;
using AutoMapper;
using EcsDigitalApi.Entities;
using EcsDigitalApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcsDigitalApiTest
{
    public static class DependenciesTestHelper
    {
        public static async Task<CarsContext> GetInMemoryCarsContext()
        {
            var builder = new DbContextOptionsBuilder<CarsContext>();
            builder.UseInMemoryDatabase("CarsContext");
            var options = builder.Options;
            var carsContext = new CarsContext(options);
            await carsContext.Database.EnsureDeletedAsync().ConfigureAwait(false);
            await carsContext.Database.EnsureCreatedAsync().ConfigureAwait(false); ;
            return carsContext;
        }

        public static async Task<ICarRepository> GetInMemoryCarRepository(CarsContext carsContext = null)
        {
            if (carsContext == null)
                carsContext = await GetInMemoryCarsContext();

            return new CarRepository(carsContext);
        }

        public static IMapper GetMapper()
        {
            return new Mapper(new MapperConfiguration(CreateMaps));
        }

        private static void CreateMaps(IMapperConfigurationExpression config)
        {
            config.CreateMap<Car, EcsDigitalApi.ApiModels.Car>();
            config.CreateMap<EcsDigitalApi.ApiModels.Car, Car>();
        }

    }
}
