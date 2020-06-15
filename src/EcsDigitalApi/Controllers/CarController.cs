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
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> Get()
        {
            try
            {
                var getCarsTask = _carService.GetAll();
                var cars = await getCarsTask;

                if (!getCarsTask.IsCompletedSuccessfully)
                    return StatusCode((int)HttpStatusCode.InternalServerError);

                return Ok(cars);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<Car>> Get(int id)
        {
            if (id == 0)
                return BadRequest();
            
            var getCarTask = _carService.Get(id);
            var car = await getCarTask;

            if (!getCarTask.IsCompletedSuccessfully)
                return StatusCode((int) HttpStatusCode.InternalServerError);

            if (car == null)
                return NotFound();

            return Ok(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post([FromBody] Car car)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (car.Id != 0)
                return await Put(car.Id, car);

            var createTask = _carService.Add(car);
            await createTask;

            return createTask.IsCompletedSuccessfully ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Put(int id, [FromBody] Car car)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id == 0 && car.Id == 0)
                return await Post(car);

            if (car.Id == 0)
                car.Id = id;

            if (await _carService.Get(id) == null)
                return NotFound();

            var updateTask = _carService.Update(car);
            await updateTask;

            return updateTask.IsCompletedSuccessfully ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            if (await _carService.Get(id) == null)
                return NotFound();

            var deletedTask = _carService.Remove(id);
            await deletedTask;

            return deletedTask.IsCompletedSuccessfully ? Ok() : StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}
