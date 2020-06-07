using System.Threading.Tasks;
using EcsDigitalApi.Services;
using EcsDigitalApi.Controllers;
using Moq;
using Xunit;

namespace EcsDigitalApiUnitTest
{
    public class CarsControllerTest
    {
        // When: a "get" call to /api/cars,
        // Then: return a response containing all cars from DB.
        [Fact]
        public async Task Get_NoCondition_Calls_CarsService_GetAll()
        {
            var mockCarService = new Mock<ICarService>();
            var carController = new CarController(mockCarService.Object);

            await carController.Get().ConfigureAwait(false);

            mockCarService.Verify(x=> x.GetAll());
        }
    }
}

