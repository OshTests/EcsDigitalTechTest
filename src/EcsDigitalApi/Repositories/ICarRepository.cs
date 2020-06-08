using System.Collections.Generic;
using System.Threading.Tasks;
using EcsDigitalApi.Entities;

namespace EcsDigitalApi.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAll();
        Task<Car> GetBiId(int id);
        Task<bool> Update(Car car);
        Task<bool> Add(Car car);
        Task<bool> RemoveCar(int carId);
    }
}