using System;
using System.Net;
using System.Threading.Tasks;
using EcsDigitalApi.Services;
using EcsDigitalApi.Controllers;
using EcsDigitalApi.ApiModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EcsDigitalApiUnitTest
{
    public class CarsControllerTest
    {

        /// <summary>
        /// 1.
        /// When: a "get" call to /api/cars,
        /// Then: return a response containing all cars from DB.
        /// </summary>
        [Fact]
        public async Task Get_NoCondition_Calls_CarsService_GetAll()
        {
            var mockCarService = new Mock<ICarService>();
            var carController = new CarController(mockCarService.Object);

            var response = await carController.Get().ConfigureAwait(false);

            mockCarService.Verify(x=> x.GetAll());
            response.Result.Should().BeOfType<OkObjectResult>();
        }

        /// <summary>
        /// 1.1
        /// When: a "get" call to /api/cars,
        /// Given: the service throws an exception
        /// Then: return a response containing all cars from DB.
        /// </summary>
        [Fact]
        public async Task Get_NoCondition_Calls_CarsService_GetAll_ThrowsException_ReturnsInternalServerError()
        {
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.GetAll()).Returns(() => throw new Exception());

            var carController = new CarController(mockCarService.Object);

            var response = await carController.Get().ConfigureAwait(false);
            
            response.Result.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// 2.
        /// When: there's a "get" call to /api/cars/{id}
        /// Given: the id argument is 0 (zero)
        /// Then: return an error response(BadRequest)
        /// </summary>
        [Fact]
        public async Task Get_IdZero_ReturnsError()
        {
            var mockCarService = new Mock<ICarService>();
            var carController = new CarController(mockCarService.Object);

            var response = await carController.Get(0).ConfigureAwait(false);
            
            response.Result.Should().BeOfType<BadRequestResult>();
        }

        /// <summary>
        /// 3.
        /// When: there's a "get" call to /api/cars/{id}
        /// Given: a car with that id does not exist in the DB
        /// Then: return an error response(NotFound)
        /// </summary>
        [Fact]
        public async Task Get_IdNotFound_ReturnsNotFound()
        {
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(() => null);

            var carController = new CarController(mockCarService.Object);

            var response = await carController.Get(1).ConfigureAwait(false);

            mockCarService.Verify(x => x.Get(It.IsAny<int>()));

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        /// 4.
        ///When: there's a "get" call to /api/cars/{id}
        ///Given: a car with that id exists in the DB
        ///Then: return a response containing the car from DB with the matching id.
        /// </summary>
        [Fact]
        public async Task Get_IdFound_Calls_CarsService_Get_ReturnsMatchingCar()
        {
            var expectedCarId = 111;
            var expectedCar = new Car { Id = expectedCarId, Year = 2019 };
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.Get(expectedCarId)).ReturnsAsync(() => expectedCar);

            var carController = new CarController(mockCarService.Object);

            var response = await carController.Get(expectedCarId).ConfigureAwait(false);

            mockCarService.Verify(x => x.Get(expectedCarId));

            response.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<Car>()
                .Which.Id.Should().Be(expectedCarId);
        }

        /// <summary>
        /// 5.
        ///When: there's a "put" call to /api/cars
        ///Given: the request is not valid(required fields and so on...)
        ///Then: return an error response(BadRequest)
        /// </summary>
        [Fact]
        public async Task Put_RequestNotValid_ReturnsBadRequest()
        {
            var mockCarService = new Mock<ICarService>();
            var carController = new CarController(mockCarService.Object);
            // Sufficient for unit tests. Integration tests are required 
            carController.ModelState.AddModelError("Year", "is required");

            var response = await carController.Put(111, new Car { Id = 111 }).ConfigureAwait(false);

            response.Should().BeOfType<BadRequestResult>();
        }

        /// <summary>
        /// 6.
        ///When: there's a "put" call to /api/cars
        ///Given: a.the request is valid(required fields and so on...)
        ///       b.the id field is 0 (zero) and the argument field is 0 (zero)
        ///Then: the post method is called(to add the new car to the DB)
        /// </summary>
        [Fact]
        public async Task Put_IdIsZero_CallsPost()
        {
            var mockCarService = new Mock<ICarService>();
            var carController = new CarController(mockCarService.Object);

            var response = await carController.Put(0, new Car{Id = 0, Year = 2019 }).ConfigureAwait(false);

            mockCarService.Verify(p=> p.Create(It.IsAny<Car>()));
            response.Should().BeOfType<OkResult>();
        }

        /// <summary>
        /// 7.
        ///When: there's a "put" call to /api/cars
        ///Given: a. the request is valid(required fields and so on...)
        ///       b. the id field is not 0 (zero)
        ///       c. a car with that id does not exist in the DB
        ///Then: return an error response (NotFound)
        /// </summary>
        [Fact]
        public async Task Put_IdDoesNotExist_ReturnsNotFound()
        {
            var mockCarService = new Mock<ICarService>();
            var expectedCarId = 111;
            var expectedCar = new Car { Id = expectedCarId, Year = 2019 };
            mockCarService.Setup(x => x.Get(111)).ReturnsAsync(() => null);

            var carController = new CarController(mockCarService.Object);

            var response = await carController.Put(111, expectedCar).ConfigureAwait(false);

            mockCarService.Verify(x => x.Get(111));

            response.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        /// 8.
        ///When: there's a "put" call to /api/cars
        ///Given: a.the request is valid (required fields and so on...)
        ///       b.the id field is not 0 (zero)
        ///       c.a car with that id exists in the DB
        ///Then: edit the existing matching car with the new fields
        /// </summary>
        [Fact]
        public async Task Put_NoCondition_Calls_CarsService_Update()
        {
            var expectedCarId = 111;
            var expectedCar = new Car { Id = expectedCarId, Year = 2019 };
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.Get(expectedCarId)).ReturnsAsync(() => expectedCar);

            var carController = new CarController(mockCarService.Object);

            var response = await carController.Put(expectedCarId, expectedCar).ConfigureAwait(false);

            mockCarService.Verify(x => x.Update(expectedCar));

            response.Should().BeOfType<OkResult>();
        }

        /// <summary>
        /// 9.
        ///When: there's a "post" call to /api/cars
        ///Given: the request is not valid (required fields and so on...)
        ///Then: return an error response(BadRequest)
        /// </summary>
        [Fact]
        public async Task Post_RequestNotValid_ReturnsBadRequest()
        {
            var expectedCarId = 111;
            var expectedCar = new Car { Id = expectedCarId };
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.Get(expectedCarId)).ReturnsAsync(() => expectedCar);

            var carController = new CarController(mockCarService.Object);

            // Sufficient for unit tests. Integration tests are required 
            carController.ModelState.AddModelError("Year", "is required");

            var response = await carController.Post(expectedCar).ConfigureAwait(false);

            response.Should().BeOfType<BadRequestResult>();
        }

        /// <summary>
        /// 10.
        ///When: there's a "post" call to /api/cars
        ///Given: a.the request is valid(required fields and so on...)
        ///       b.the id field is not 0 (zero)
        ///Then: call the "put" method to update the car in the DB.
        /// </summary>
        [Fact]
        public async Task Post_IdIsNotZero_CallsPut()
        {
            var expectedCarId = 111;
            var expectedCar = new Car { Id = expectedCarId, Year = 2019 };
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.Get(expectedCarId)).ReturnsAsync(() => expectedCar);

            var carController = new CarController(mockCarService.Object);

            var response = await carController.Post(expectedCar).ConfigureAwait(false);

            mockCarService.Verify(x => x.Update(expectedCar));

            response.Should().BeOfType<OkResult>();
        }

        /// <summary>
        /// 11.
        ///When: there's a "delete" call to /api/cars
        ///Given: the id is 0 (zero)
        ///Then: return an error response(BadRequest)
        /// </summary>
        [Fact]
        public async Task Delete_IdIsZero_ReturnsBadRequest()
        {
            var mockCarService = new Mock<ICarService>();
            var carController = new CarController(mockCarService.Object);

            var response = await carController.Delete(0).ConfigureAwait(false);

            response.Should().BeOfType<BadRequestResult>();
        }

        /// <summary>
        /// 12.
        /// When: there's a "delete" call to /api/cars
        /// Given: a. the id is not 0 (zero)
        ///        b. a car with that id does not exist in the DB
        /// Then: return an error response (NotFound)
        /// </summary>
        [Fact]
        public async Task Delete_IdNotFound_ReturnsNotFound()
        {
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(() => null);

            var carController = new CarController(mockCarService.Object);

            var response = await carController.Delete(1).ConfigureAwait(false);

            mockCarService.Verify(x => x.Get(It.IsAny<int>()));
            response.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        /// 13.
        /// When: there's a "delete" call to /api/cars
        /// Given: a. the id is not 0 (zero)
        ///        b. a car with that id exists in the DB
        /// Then: Delete the car from the DB
        /// </summary>
        [Fact]
        public async Task Delete_NoCondition_Calls_CarsService_Delete()
        {
            var mockCarService = new Mock<ICarService>();
            var expectedCarId = 111;
            var expectedCar = new Car { Id = expectedCarId, Year = 2019 };
            mockCarService.Setup(x => x.Get(expectedCarId)).ReturnsAsync(() => expectedCar);

            var carController = new CarController(mockCarService.Object);

            var response = await carController.Delete(expectedCarId).ConfigureAwait(false);

            mockCarService.Verify(x => x.Delete(It.IsAny<int>()));
            response.Should().BeOfType<OkResult>();
        }
        
    }
}

