using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.Data;
using EcsDigitalApi.DbModels;
using Microsoft.EntityFrameworkCore;

namespace EcsDigitalApi.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarsContext _carsContext;

        public CarRepository(CarsContext carsContext)
        {
            _carsContext = carsContext;
        }

        public async Task<IEnumerable<Car>> GetAllCars()
        {
            var cars = await _carsContext.Cars
                .Include(p => p.Model)
                .Include(p => p.Model.Maker)
                .ToListAsync().ConfigureAwait(false);

            return cars;
        }

        public async Task<Car> GetCarById(int carId)
        {
            var car = await _carsContext.Cars.FindAsync(carId).ConfigureAwait(false);

            return car;
        }

        public async Task<bool> AddCar(Car car)
        {
            await _carsContext.Cars.AddAsync(car).ConfigureAwait(false);
            return await _carsContext.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<bool> UpdateCar(Car car)
        {
            _carsContext.Entry(await _carsContext.Cars.FirstOrDefaultAsync(x => x.Id == car.Id)).CurrentValues.SetValues(car);
            return await _carsContext.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<bool> RemoveCar(int carId)
        {
            var car = await GetCarById(carId).ConfigureAwait(false);
            _carsContext.Cars.Remove(car);
            return await _carsContext.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<IEnumerable<Maker>> GetAllMakers()
        {
            var makers = await _carsContext.Makers.ToListAsync().ConfigureAwait(false);

            return makers;
        }

        public async Task<Maker> GetMakerById(int makerId)
        {
            var maker = await _carsContext.Makers.FindAsync(makerId).ConfigureAwait(false);

            return maker;
        }

        public async Task<bool> AddMaker(Maker maker)
        {
            await _carsContext.Makers.AddAsync(maker).ConfigureAwait(false);
            return await _carsContext.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<bool> RemoveMaker(int makerId)
        {
            var maker = await GetMakerById(makerId).ConfigureAwait(false);
            _carsContext.Makers.Remove(maker);
            return await _carsContext.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<IEnumerable<Model>> GetAllModels()
        {
            var models = await _carsContext.Models.Include(p => p.Maker).ToListAsync().ConfigureAwait(false);

            return models;
        }

        public async Task<Model> GetModelById(int modelId)
        {
            var model = await _carsContext.Models.FindAsync(modelId).ConfigureAwait(false);

            return model;
        }

        public async Task<bool> AddModel(Model model)
        {
            await _carsContext.Models.AddAsync(model).ConfigureAwait(false);
            return await _carsContext.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<bool> RemoveModel(int modelId)
        {
            var model = await GetModelById(modelId).ConfigureAwait(false);
            _carsContext.Models.Remove(model);
            return await _carsContext.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
    }
}