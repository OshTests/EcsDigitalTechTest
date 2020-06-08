using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using EcsDigitalApi.Controllers;
using EcsDigitalApi.Entities;
using EcsDigitalApi.Services;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EcsDigitalApiTest
{
    public class CarServiceTest
    {

        [Fact]
        public async Task GetAll_NoCondition()
        {
            var newCar1 = GetNewCar(Color.AntiqueWhite, 1, 2020);
            var newCar2 = GetNewCar(Color.Aqua, 3, 1999);
            var carService = await GetCarServiceWithCars(newCar1, newCar2);

            var response = await carService.GetAll().ConfigureAwait(false);

            response.Should().OnlyContain(car => CarsEqual(car, newCar1) || CarsEqual(car, newCar2));
        }

        [Fact]
        public async Task Get_NoCondition()
        {
            var newCar1 = GetNewCar(Color.AntiqueWhite, 1, 2020);
            var carService = await GetCarServiceWithCars(newCar1);

            var response = await carService.Get(1).ConfigureAwait(false);

            response.Should().BeEquivalentTo(newCar1, options =>
                options.Including(car => car.Colour).Including(car => car.ModelId).Including(car => car.Year));
        }

        [Fact]
        public async Task Add_NoCondition()
        {
            var newCar1 = GetNewApiCar(Color.AntiqueWhite, 1, 2020);
            var carService = await GetCarService();

            var added = await carService.Add(newCar1).ConfigureAwait(false);
            var newCar = await carService.Get(1).ConfigureAwait(false);

            added.Should().BeTrue("Car was not added to the DB");
            newCar.Should().BeEquivalentTo(newCar1, options =>
                    options.Including(car => car.Colour).Including(car => car.ModelId).Including(car => car.Year),
                "Added car was not found in the DB");
        }

        [Fact]
        public async Task Update_NoCondition()
        {
            var newCar1 = GetNewCar(Color.AntiqueWhite, 1, 2020);
            var carService = await GetCarServiceWithCars(newCar1);

            var newCar = await carService.Get(1).ConfigureAwait(false);
            newCar.Year = 22222;
            var updated = await carService.Update(newCar).ConfigureAwait(false);
            var modifiedCar = await carService.Get(1).ConfigureAwait(false);

            updated.Should().BeTrue("Car was not updated in the DB");
            modifiedCar.Should().BeEquivalentTo(newCar, options => options.IncludingProperties());
        }

        [Fact]
        public async Task Remove_NoCondition()
        {
            var newCar1 = GetNewCar(Color.AntiqueWhite, 1, 2020);
            var carService = await GetCarServiceWithCars(newCar1);

            var removed = await carService.Remove(1).ConfigureAwait(false);
            var removedCar = await carService.Get(1).ConfigureAwait(false);

            removed.Should().BeTrue("Car was not removed in the DB");
            removedCar.Should().BeNull();
        }

        private static bool CarsEqual(EcsDigitalApi.ApiModels.Car car, Car newCar1)
        {
            return car.Colour == newCar1.Colour && car.ModelId == newCar1.ModelId && car.Year == newCar1.Year;
        }

        private static async Task<CarService> GetCarService()
        {
            var carRepositoryInMemory = await DependenciesTestHelper.GetInMemoryCarRepository().ConfigureAwait(false);
            var mapper = DependenciesTestHelper.GetMapper();
            var carService = new CarService(mapper, carRepositoryInMemory);
            return carService;
        }

        private static async Task<CarService> GetCarServiceWithCars(params Car[] cars)
        {
            var carsContextInMemory = await DependenciesTestHelper.GetInMemoryCarsContext().ConfigureAwait(false);
            await carsContextInMemory.Cars.AddRangeAsync(cars);
            await carsContextInMemory.SaveChangesAsync().ConfigureAwait(false);
            var carRepositoryInMemory =
                await DependenciesTestHelper.GetInMemoryCarRepository(carsContextInMemory).ConfigureAwait(false);
            var mapper = DependenciesTestHelper.GetMapper();
            var carService = new CarService(mapper, carRepositoryInMemory);
            return carService;
        }

        private Car GetNewCar(Color color, int modelId, int year)
        {
            return new Car { Colour = color, ModelId = modelId, Year = year };
        }

        private EcsDigitalApi.ApiModels.Car GetNewApiCar(Color color, int modelId, int year)
        {
            return new EcsDigitalApi.ApiModels.Car { Colour = color, ModelId = modelId, Year = year };
        }
    }
}
