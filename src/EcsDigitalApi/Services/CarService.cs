using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.ApiModels;

namespace EcsDigitalApi.Services
{
    public class CarService : ICarService
    {
        public async Task<IEnumerable<Car>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Car> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task Create(Car car)
        {
            throw new System.NotImplementedException();
        }

        public async Task Update(Car car)
        {
            throw new System.NotImplementedException();
        }

        public async Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}