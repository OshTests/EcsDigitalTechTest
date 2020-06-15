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
    public class ModelControllerTest
    {

        /// <summary>
        /// When: a "get" call to /api/model,
        /// Then: return a response containing all models from DB.
        /// </summary>
        [Fact]
        public async Task Get_NoCondition_Calls_ModelsService_GetAll()
        {
            var mockModelService = new Mock<IModelService>();
            var modelController = new ModelController(mockModelService.Object);

            var response = await modelController.Get().ConfigureAwait(false);

            mockModelService.Verify(x=> x.GetAll());
            response.Result.Should().BeOfType<OkObjectResult>();
        }

        /// <summary>
        /// When: a "get" call to /api/model,
        /// Given: the service throws an exception
        /// Then: return a response containing all models from DB.
        /// </summary>
        [Fact]
        public async Task Get_NoCondition_Calls_ModelsService_GetAll_ThrowsException_ReturnsInternalServerError()
        {
            var mockModelService = new Mock<IModelService>();
            mockModelService.Setup(x => x.GetAll()).Returns(() => throw new Exception());

            var modelController = new ModelController(mockModelService.Object);

            var response = await modelController.Get().ConfigureAwait(false);
            
            response.Result.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// When: there's a "get" call to /api/model/{id}
        /// Given: the id argument is 0 (zero)
        /// Then: return an error response(BadRequest)
        /// </summary>
        [Fact]
        public async Task Get_IdZero_ReturnsError()
        {
            var mockModelService = new Mock<IModelService>();
            var modelController = new ModelController(mockModelService.Object);

            var response = await modelController.Get(0).ConfigureAwait(false);
            
            response.Result.Should().BeOfType<BadRequestResult>();
        }

        /// <summary>
        /// When: there's a "get" call to /api/model/{id}
        /// Given: a model with that id does not exist in the DB
        /// Then: return an error response(NotFound)
        /// </summary>
        [Fact]
        public async Task Get_IdNotFound_ReturnsNotFound()
        {
            var mockModelService = new Mock<IModelService>();
            mockModelService.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(() => null);

            var modelController = new ModelController(mockModelService.Object);

            var response = await modelController.Get(1).ConfigureAwait(false);

            mockModelService.Verify(x => x.Get(It.IsAny<int>()));

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        ///When: there's a "get" call to /api/model/{id}
        ///Given: a model with that id exists in the DB
        ///Then: return a response containing the model from DB with the matching id.
        /// </summary>
        [Fact]
        public async Task Get_IdFound_Calls_ModelsService_Get_ReturnsMatchingModel()
        {
            var expectedModelId = 111;
            var expectedModel = new Model { Id = expectedModelId, Name = "model Name" };
            var mockModelService = new Mock<IModelService>();
            mockModelService.Setup(x => x.Get(expectedModelId)).ReturnsAsync(() => expectedModel);

            var modelController = new ModelController(mockModelService.Object);

            var response = await modelController.Get(expectedModelId).ConfigureAwait(false);

            mockModelService.Verify(x => x.Get(expectedModelId));

            response.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<Model>()
                .Which.Id.Should().Be(expectedModelId);
        }

        /// <summary>
        ///When: there's a "post" call to /api/model
        ///Given: the request is not valid (required fields and so on...)
        ///Then: return an error response(BadRequest)
        /// </summary>
        [Fact]
        public async Task Post_RequestNotValid_ReturnsBadRequest()
        {
            var expectedModelId = 111;
            var expectedModel = new Model { Id = expectedModelId };
            var mockModelService = new Mock<IModelService>();
            mockModelService.Setup(x => x.Get(expectedModelId)).ReturnsAsync(() => expectedModel);

            var modelController = new ModelController(mockModelService.Object);

            // Sufficient for unit tests. Integration tests are required 
            modelController.ModelState.AddModelError("Year", "is required");

            var response = await modelController.Post(expectedModel).ConfigureAwait(false);

            response.Should().BeOfType<BadRequestResult>();
        }

        /// <summary>
        ///When: there's a "delete" call to /api/model
        ///Given: the id is 0 (zero)
        ///Then: return an error response(BadRequest)
        /// </summary>
        [Fact]
        public async Task Delete_IdIsZero_ReturnsBadRequest()
        {
            var mockModelService = new Mock<IModelService>();
            var modelController = new ModelController(mockModelService.Object);

            var response = await modelController.Delete(0).ConfigureAwait(false);

            response.Should().BeOfType<BadRequestResult>();
        }

        /// <summary>
        /// When: there's a "delete" call to /api/model
        /// Given: a. the id is not 0 (zero)
        ///        b. a model with that id does not exist in the DB
        /// Then: return an error response (NotFound)
        /// </summary>
        [Fact]
        public async Task Delete_IdNotFound_ReturnsNotFound()
        {
            var mockModelService = new Mock<IModelService>();
            mockModelService.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(() => null);

            var modelController = new ModelController(mockModelService.Object);

            var response = await modelController.Delete(1).ConfigureAwait(false);

            mockModelService.Verify(x => x.Get(It.IsAny<int>()));
            response.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        /// When: there's a "delete" call to /api/model
        /// Given: a. the id is not 0 (zero)
        ///        b. a model with that id exists in the DB
        /// Then: Delete the model from the DB
        /// </summary>
        [Fact]
        public async Task Delete_NoCondition_Calls_ModelsService_Delete()
        {
            var mockModelService = new Mock<IModelService>();
            var expectedModelId = 111;
            var expectedModel = new Model { Id = expectedModelId, Name = "model name" };
            mockModelService.Setup(x => x.Get(expectedModelId)).ReturnsAsync(() => expectedModel);

            var modelController = new ModelController(mockModelService.Object);

            var response = await modelController.Delete(expectedModelId).ConfigureAwait(false);

            mockModelService.Verify(x => x.Remove(It.IsAny<int>()));
            response.Should().BeOfType<OkResult>();
        }
        
    }
}

