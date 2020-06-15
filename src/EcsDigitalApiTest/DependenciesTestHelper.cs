using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcsDigitalApi;
using EcsDigitalApi.Data;
using EcsDigitalApi.DbModels;
using EcsDigitalApi.Repositories;
using EcsDigitalApi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EcsDigitalApiTest
{
    public static class DependenciesTestHelper
    {
        public static async Task<CarsContext> GetInMemorySeededCarsContext(string contextName)
        {
            var carsContext = await GetInMemoryCarsContext(contextName);
            await carsContext.SeedInMemory();
            return carsContext;
        }

        public static async Task<CarsContext> GetInMemoryCarsContext(string contextName)
        {
            var builder = new DbContextOptionsBuilder<CarsContext>();
            builder.UseInMemoryDatabase(contextName);
            var options = builder.Options;
            var carsContext = new CarsContext(options);
            await carsContext.Database.EnsureDeletedAsync().ConfigureAwait(false);
            await carsContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
            return carsContext;
        }

        public static async Task<ICarRepository> GetInMemoryCarRepository(CarsContext carsContext)
        {
            await Task.Delay(10);
            return new CarRepository(carsContext);
        }

        public static async Task<ICarRepository> GetInMemoryCarRepository(string contextName)
        {
            var carsContext = await GetInMemoryCarsContext(contextName);

            return new CarRepository(carsContext);
        }

        public static IMapper GetMapper()
        {
            return new Mapper(new MapperConfiguration(Mappers.CreateMaps));
        }

        public static async Task<CarService> GetSeededCarService(string contextName)
        {
            var carsContextInMemory = await GetInMemorySeededCarsContext(contextName).ConfigureAwait(false);
            var carRepositoryInMemory = new CarRepository(carsContextInMemory);
            var mapper = GetMapper();
            var carService = new CarService(mapper, carRepositoryInMemory);
            return carService;
        }

        public static async Task<CarService> GetCarService(string contextName)
        {
            var carRepositoryInMemory = await GetInMemoryCarRepository(contextName).ConfigureAwait(false);
            var mapper = GetMapper();
            var carService = new CarService(mapper, carRepositoryInMemory);
            return carService;
        }

        public static async Task<ModelService> GetModelService(string contextName, IRelatedWordsService relatedWordsService)
        {
            var carRepositoryInMemory = await GetInMemoryCarRepository(contextName).ConfigureAwait(false);
            var mapper = GetMapper();
            var modelService = new ModelService(mapper, carRepositoryInMemory, relatedWordsService);
            return modelService;
        }

        public static async Task<CarService> GetCarServiceWithCars(string contextName, params Car[] cars)
        {
            var carsContextInMemory = await GetInMemoryCarsContext(contextName).ConfigureAwait(false);
            await AddMissingModels(cars, carsContextInMemory);
            await carsContextInMemory.Cars.AddRangeAsync(cars);
            await carsContextInMemory.SaveChangesAsync().ConfigureAwait(false);
            var carRepositoryInMemory = await GetInMemoryCarRepository(carsContextInMemory).ConfigureAwait(false);
            var mapper = GetMapper();
            var carService = new CarService(mapper, carRepositoryInMemory);
            return carService;
        }

        public static async Task<ModelService> GetModelServiceWithModels(string contextName, params Model[] models)
        {
            var carsContextInMemory = await GetInMemoryCarsContext(contextName).ConfigureAwait(false);
            await AddMissingMakers(models, carsContextInMemory);
            await carsContextInMemory.Models.AddRangeAsync(models);
            await carsContextInMemory.SaveChangesAsync().ConfigureAwait(false);
            var carRepositoryInMemory = await GetInMemoryCarRepository(carsContextInMemory).ConfigureAwait(false);
            var mapper = GetMapper();
            var modelService = new ModelService(mapper, carRepositoryInMemory, new Mock<IRelatedWordsService>().Object);
            return modelService;
        }

        public static async Task<ModelService> GetModelServiceWithMakers(CarsContext carsContextInMemory, IRelatedWordsService relatedWordsService, params Maker[] makers)
        {
            await carsContextInMemory.Makers.AddRangeAsync(makers);
            await carsContextInMemory.SaveChangesAsync().ConfigureAwait(false);
            var carRepositoryInMemory = await GetInMemoryCarRepository(carsContextInMemory).ConfigureAwait(false);
            var mapper = GetMapper();
            var modelService = new ModelService(mapper, carRepositoryInMemory, relatedWordsService);
            return modelService;
        }

        public static async Task<ModelService> GetModelServiceWithMakers(string contextName, IRelatedWordsService relatedWordsService, params Maker[] makers)
        {
            var carsContextInMemory = await GetInMemoryCarsContext(contextName).ConfigureAwait(false);
            return await GetModelServiceWithMakers(carsContextInMemory, relatedWordsService, makers);
        }

        public static async Task<MakerService> GetMakerServiceWithMakers(string contextName, params Maker[] makers)
        {
            var carsContextInMemory = await GetInMemoryCarsContext(contextName).ConfigureAwait(false);
            await carsContextInMemory.Makers.AddRangeAsync(makers);
            await carsContextInMemory.SaveChangesAsync().ConfigureAwait(false);
            var carRepositoryInMemory = await GetInMemoryCarRepository(carsContextInMemory).ConfigureAwait(false);
            var mapper = GetMapper();
            var makerService = new MakerService(mapper, carRepositoryInMemory);
            return makerService;
        }

        private static async Task AddMissingModels(Car[] cars, CarsContext carsContextInMemory)
        {
            if (cars.All(p => p.ModelId != 0))
                return;

            var newMaker = await carsContextInMemory.Makers.AddAsync(new Maker {Name = "Maker Name"});
            var newModel = await carsContextInMemory.Models.AddAsync(new Model
                {Name = "Model Name ", MakerId = newMaker.Entity.Id});

            cars.Where(car => car.ModelId == 0).ToList().ForEach(car=>car.ModelId= newModel.Entity.Id);
        }

        private static async Task AddMissingMakers(Model[] models, CarsContext carsContextInMemory)
        {
            if (models.All(p => p.MakerId != 0))
                return;

            var newMaker = await carsContextInMemory.Makers.AddAsync(new Maker {Name = "Maker Name"});
            foreach (var model in models.Where(p => p.MakerId == 0))
                model.MakerId = newMaker.Entity.Id;
        }

    }
}
