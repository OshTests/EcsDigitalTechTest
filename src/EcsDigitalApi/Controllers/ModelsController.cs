using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EcsDigitalApi.Domain;
using EcsDigitalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcsDigitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model>>> Get()
        {
            try
            {
                var getCarsTask = _modelService.GetAll();
                var models = await getCarsTask;

                if (!getCarsTask.IsCompletedSuccessfully)
                    return StatusCode((int)HttpStatusCode.InternalServerError);

                return Ok(models);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> Get(int id)
        {
            if (id == 0)
                return BadRequest();
            
            var getCarTask = _modelService.Get(id);
            var models = await getCarTask;

            if (!getCarTask.IsCompletedSuccessfully)
                return StatusCode((int) HttpStatusCode.InternalServerError);

            if (models == null)
                return NotFound();

            return Ok(models);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Model model)
        {
            if (model.Id != 0 || !ModelState.IsValid)
                return BadRequest();

            var createTask = _modelService.Add(model);
            await createTask;

            return createTask.IsCompletedSuccessfully ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            if (await _modelService.Get(id) == null)
                return NotFound();

            var deletedTask = _modelService.Remove(id);
            await deletedTask;

            return deletedTask.IsCompletedSuccessfully ? Ok() : StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}
