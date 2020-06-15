using System;
using System.Net;
using System.Threading.Tasks;
using EcsDigitalApi.Controllers;
using EcsDigitalApi.Domain;
using EcsDigitalApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EcsDigitalApiTest
{
    public class MakerControllerTest
    {

        /// <summary>
        /// When: a "get" call to /api/maker,
        /// Then: return a response containing all makers from DB.
        /// </summary>
        [Fact]
        public async Task Get_NoCondition_Calls_MakersService_GetAll()
        {
            var mockMakerService = new Mock<IMakerService>();
            var makerController = new MakerController(mockMakerService.Object);

            var response = await makerController.Get().ConfigureAwait(false);

            mockMakerService.Verify(x=> x.GetAll());
            response.Result.Should().BeOfType<OkObjectResult>();
        }

        /// <summary>
        /// When: a "get" call to /api/maker,
        /// Given: the service throws an exception
        /// Then: return a response containing all makers from DB.
        /// </summary>
        [Fact]
        public async Task Get_NoCondition_Calls_MakersService_GetAll_ThrowsException_ReturnsInternalServerError()
        {
            var mockMakerService = new Mock<IMakerService>();
            mockMakerService.Setup(x => x.GetAll()).Returns(() => throw new Exception());

            var makerController = new MakerController(mockMakerService.Object);

            var response = await makerController.Get().ConfigureAwait(false);
            
            response.Result.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// When: there's a "get" call to /api/maker/{id}
        /// Given: the id argument is 0 (zero)
        /// Then: return an error response(BadRequest)
        /// </summary>
        [Fact]
        public async Task Get_IdZero_ReturnsError()
        {
            var mockMakerService = new Mock<IMakerService>();
            var makerController = new MakerController(mockMakerService.Object);

            var response = await makerController.Get(0).ConfigureAwait(false);
            
            response.Result.Should().BeOfType<BadRequestResult>();
        }

        /// <summary>
        /// When: there's a "get" call to /api/maker/{id}
        /// Given: a maker with that id does not exist in the DB
        /// Then: return an error response(NotFound)
        /// </summary>
        [Fact]
        public async Task Get_IdNotFound_ReturnsNotFound()
        {
            var mockMakerService = new Mock<IMakerService>();
            mockMakerService.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(() => null);

            var makerController = new MakerController(mockMakerService.Object);

            var response = await makerController.Get(1).ConfigureAwait(false);

            mockMakerService.Verify(x => x.Get(It.IsAny<int>()));

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        ///When: there's a "get" call to /api/maker/{id}
        ///Given: a maker with that id exists in the DB
        ///Then: return a response containing the maker from DB with the matching id.
        /// </summary>
        [Fact]
        public async Task Get_IdFound_Calls_MakersService_Get_ReturnsMatchingMaker()
        {
            var expectedMakerId = 111;
            var expectedMaker = new Maker { Id = expectedMakerId, Name = "maker Name" };
            var mockMakerService = new Mock<IMakerService>();
            mockMakerService.Setup(x => x.Get(expectedMakerId)).ReturnsAsync(() => expectedMaker);

            var makerController = new MakerController(mockMakerService.Object);

            var response = await makerController.Get(expectedMakerId).ConfigureAwait(false);

            mockMakerService.Verify(x => x.Get(expectedMakerId));

            response.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<Maker>()
                .Which.Id.Should().Be(expectedMakerId);
        }

        /// <summary>
        ///When: there's a "post" call to /api/maker
        ///Given: the request is not valid (required fields and so on...)
        ///Then: return an error response(BadRequest)
        /// </summary>
        [Fact]
        public async Task Post_RequestNotValid_ReturnsBadRequest()
        {
            var expectedMakerId = 111;
            var expectedMaker = new Maker { Id = expectedMakerId };
            var mockMakerService = new Mock<IMakerService>();
            mockMakerService.Setup(x => x.Get(expectedMakerId)).ReturnsAsync(() => expectedMaker);

            var makerController = new MakerController(mockMakerService.Object);

            // Sufficient for unit tests. Integration tests are required 
            makerController.ModelState.AddModelError("Year", "is required");

            var response = await makerController.Post(expectedMaker).ConfigureAwait(false);

            response.Should().BeOfType<BadRequestResult>();
        }

        /// <summary>
        ///When: there's a "delete" call to /api/maker
        ///Given: the id is 0 (zero)
        ///Then: return an error response(BadRequest)
        /// </summary>
        [Fact]
        public async Task Delete_IdIsZero_ReturnsBadRequest()
        {
            var mockMakerService = new Mock<IMakerService>();
            var makerController = new MakerController(mockMakerService.Object);

            var response = await makerController.Delete(0).ConfigureAwait(false);

            response.Should().BeOfType<BadRequestResult>();
        }

        /// <summary>
        /// When: there's a "delete" call to /api/maker
        /// Given: a. the id is not 0 (zero)
        ///        b. a maker with that id does not exist in the DB
        /// Then: return an error response (NotFound)
        /// </summary>
        [Fact]
        public async Task Delete_IdNotFound_ReturnsNotFound()
        {
            var mockMakerService = new Mock<IMakerService>();
            mockMakerService.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(() => null);

            var makerController = new MakerController(mockMakerService.Object);

            var response = await makerController.Delete(1).ConfigureAwait(false);

            mockMakerService.Verify(x => x.Get(It.IsAny<int>()));
            response.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        /// When: there's a "delete" call to /api/maker
        /// Given: a. the id is not 0 (zero)
        ///        b. a maker with that id exists in the DB
        /// Then: Delete the maker from the DB
        /// </summary>
        [Fact]
        public async Task Delete_NoCondition_Calls_MakersService_Delete()
        {
            var mockMakerService = new Mock<IMakerService>();
            var expectedMakerId = 111;
            var expectedMaker = new Maker { Id = expectedMakerId, Name = "maker name" };
            mockMakerService.Setup(x => x.Get(expectedMakerId)).ReturnsAsync(() => expectedMaker);

            var makerController = new MakerController(mockMakerService.Object);

            var response = await makerController.Delete(expectedMakerId).ConfigureAwait(false);

            mockMakerService.Verify(x => x.Remove(It.IsAny<int>()));
            response.Should().BeOfType<OkResult>();
        }
        
    }
}

