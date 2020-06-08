using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.Entities;
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

        public async Task<IEnumerable<Car>> GetAll()
        {
            var cars = await _carsContext.Cars.ToListAsync().ConfigureAwait(false);

            return cars;
        }

        public async Task<Car> GetBiId(int carId)
        {
            var car = await _carsContext.Cars.FindAsync(carId).ConfigureAwait(false);

            return car;
        }

        public async Task<bool> Add(Car car)
        {
            await _carsContext.Cars.AddAsync(car).ConfigureAwait(false);
            return await _carsContext.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<bool> Update(Car car)
        {
            _carsContext.Entry(await _carsContext.Cars.FirstOrDefaultAsync(x => x.Id == car.Id)).CurrentValues.SetValues(car);
            return await _carsContext.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<bool> RemoveCar(int carId)
        {
            var car = await GetBiId(carId).ConfigureAwait(false);
            _carsContext.Cars.Remove(car);
            return await _carsContext.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
    }
}