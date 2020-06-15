using System.Drawing;
using System.Threading.Tasks;
using EcsDigitalApi.DbModels;
using FluentAssertions;
using Xunit;

namespace EcsDigitalApiTest
{
    public class CarServiceTest
    {

        [Fact]
        public async Task GetAll_NoCondition()
        {
            var newCar1 = GetNewCar(Color.AntiqueWhite, 2020);
            var newCar2 = GetNewCar(Color.Aqua, 1999);
            var carService = await DependenciesTestHelper.GetCarServiceWithCars(nameof(CarServiceTest) + nameof(GetAll_NoCondition), newCar1, newCar2);

            var response = await carService.GetAll().ConfigureAwait(false);

            response.Should().OnlyContain(car => CarsEqual(car, newCar1) || CarsEqual(car, newCar2));
        }

        [Fact]
        public async Task Get_NoCondition()
        {
            var newCar1 = GetNewCar(Color.AntiqueWhite, 2020);
            var carService = await DependenciesTestHelper.GetCarServiceWithCars(nameof(CarServiceTest) + nameof(Get_NoCondition), newCar1);

            var response = await carService.Get(1).ConfigureAwait(false);

            CarsEqual(response, newCar1).Should().BeTrue("");
        }

        [Fact]
        public async Task Add_NoCondition()
        {
            var newCar1 = GetNewApiCar(Color.AntiqueWhite, 1, 2020);
            var carService = await DependenciesTestHelper.GetCarService(nameof(CarServiceTest) + nameof(Add_NoCondition));

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
            var newCar1 = GetNewCar(Color.AntiqueWhite, 2020);
            var carService = await DependenciesTestHelper.GetCarServiceWithCars(nameof(CarServiceTest) + nameof(Update_NoCondition), newCar1);

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
            var newCar1 = GetNewCar(Color.AntiqueWhite, 2020);
            var carService = await DependenciesTestHelper.GetCarServiceWithCars(nameof(CarServiceTest) + nameof(Remove_NoCondition), newCar1);

            var newCar = await carService.Get(1).ConfigureAwait(false);
            var removed = await carService.Remove(1).ConfigureAwait(false);
            var removedCar = await carService.Get(1).ConfigureAwait(false);

            newCar.Should().NotBeNull("Car was not added... Problem with test");
            removed.Should().BeTrue("Car was not removed in the DB");
            removedCar.Should().BeNull();
        }

        private static bool CarsEqual(EcsDigitalApi.Domain.Car car, Car newCar1)
        {
            return car.Colour == newCar1.Colour.Name && car.ModelId == newCar1.ModelId && car.Year == newCar1.Year;
        }


        private Car GetNewCar(Color color, int year)
        {
            return new Car { Colour = color, Year = year };
        }

        private EcsDigitalApi.Domain.Car GetNewApiCar(Color color, int modelId, int year)
        {
            return new EcsDigitalApi.Domain.Car { Colour = color.Name, ModelId = modelId, Year = year };
        }
    }
}
