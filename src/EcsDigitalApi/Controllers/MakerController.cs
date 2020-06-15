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
    public class MakerController : ControllerBase
    {
        private readonly IMakerService _makerService;

        public MakerController(IMakerService makerService)
        {
            _makerService = makerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maker>>> Get()
        {
            try
            {
                var getCarsTask = _makerService.GetAll();
                var makers = await getCarsTask;

                if (!getCarsTask.IsCompletedSuccessfully)
                    return StatusCode((int)HttpStatusCode.InternalServerError);

                return Ok(makers);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Maker>> Get(int id)
        {
            if (id == 0)
                return BadRequest();
            
            var getCarTask = _makerService.Get(id);
            var makers = await getCarTask;

            if (!getCarTask.IsCompletedSuccessfully)
                return StatusCode((int) HttpStatusCode.InternalServerError);

            if (makers == null)
                return NotFound();

            return Ok(makers);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Maker makers)
        {
            if (makers.Id != 0 || !ModelState.IsValid)
                return BadRequest();

            var createTask = _makerService.Add(makers);
            await createTask;

            return createTask.IsCompletedSuccessfully ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            if (await _makerService.Get(id) == null)
                return NotFound();

            var deletedTask = _makerService.Remove(id);
            await deletedTask;

            return deletedTask.IsCompletedSuccessfully ? Ok() : StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}
